using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using BEE;

namespace SP_BEE;

internal class MSServer
{
    private MSServer()
    {
        Cts = new CancellationTokenSource();
        _threadPool = new ThreadPoolMS(Cts.Token);
    }

    private static MSServer? _instance;
    internal static MSServer Instance => _instance ??= new MSServer();
    private readonly ThreadPoolMS _threadPool;
    internal readonly CancellationTokenSource Cts;
    
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

internal class ThreadPoolMS
{
    private readonly BlockingCollection<Socket> _tasksQueue = new BlockingCollection<Socket>();
    private readonly Thread[] _threads = new Thread[Environment.ProcessorCount];
    private CancellationToken ct;
    
    internal ThreadPoolMS(CancellationToken token)
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

    private void HandleRequest(Socket client)
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

        ToRegister tr = ToRegister.Parser.ParseFrom(body);
        MSRegister.Instance.RegisterNewMicroService(tr);
        LoadBalancer.ReloadLB();
        byte[] ack = "<|ACK|>"u8.ToArray();

        client.Send(ack);
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