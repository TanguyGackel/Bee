using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using BEE;

namespace SP_BEE;

internal class FrontServer
{
    private FrontServer()
    {
        Cts = new CancellationTokenSource();
        _threadPool = new ThreadPoolFront(Cts.Token);
    }

    private static FrontServer? _instance;
    internal static FrontServer Instance => _instance ??= new FrontServer();
    private readonly ThreadPoolFront _threadPool;
    public readonly CancellationTokenSource Cts;
    
    internal void Start(IPAddress ip, int port)
    {
        IPEndPoint localEndPoint = new IPEndPoint(ip, port);
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        server.Bind(localEndPoint);
        Console.WriteLine("Ready to listen");
        server.Listen();

        while (true)
        {
            try
            {
                _threadPool.EnqueueTask(server.Accept());
                Console.WriteLine("Received a request, enqueued it");
            }
            catch (Exception e)
            {
                
            }
        }
    }
}

internal class ThreadPoolFront
{
    private readonly BlockingCollection<Socket> _tasksQueue = new BlockingCollection<Socket>();
    private readonly Thread[] _threads = new Thread[Environment.ProcessorCount];
    private CancellationToken ct;
    
    internal ThreadPoolFront(CancellationToken token)
    {
        ct = token;
        for (int i = 0; i < Environment.ProcessorCount; i++)
        {
            _threads[i] = new Thread(StartWorking) { IsBackground = true };
            _threads[i].Start();
        }
    }

    private void StartWorking()
    {
        foreach (Socket task in _tasksQueue.GetConsumingEnumerable(ct))
        {
            Console.WriteLine("Begin process of a new task");
            HandleRequest(task);
        }
    }

    private async void HandleRequest(Socket client)
    {
        byte[] buffer = new byte[512];
        int length = client.Receive(buffer, buffer.Length, SocketFlags.None);
        int maxLength = length;
        byte[] bodyTemp = buffer.ToArray();
            
        while (length == 512)
        {
            try
            {
                length = client.Receive(buffer, buffer.Length, SocketFlags.None);
            }
            catch(Exception e)
            {
                Console.WriteLine("Erreur qui me casse les bonbons mais pas le temps de corriger : " + e.Message);
            }

            maxLength += length;
            byte[] temp = new byte[bodyTemp.Length + length];
            Buffer.BlockCopy(bodyTemp, 0, temp, 0, bodyTemp.Length);
            Buffer.BlockCopy(buffer, 0, temp, bodyTemp.Length, buffer.Length);
            bodyTemp = temp;
        }

        byte[] body = new byte[maxLength];
        Buffer.BlockCopy(bodyTemp, 0, body, 0, maxLength);

        SPPacket packet = SPPacket.Parser.ParseFrom(body);
        byte[] resp = LoadBalancer.SendRequest(packet);
        
        await client.SendAsync(resp);
    }
    
    

    internal void EnqueueTask(Socket task)
    {
        _tasksQueue.Add(task, ct);
    }
    
    internal void Shutdown()
    {
        _tasksQueue.CompleteAdding();
    }
}