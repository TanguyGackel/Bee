using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using BEE;

namespace SP_BEE;

internal class MSServer
{
    private MSServer()
    {
        _threadPool = new ThreadPoolMS();
    }

    private static MSServer? _instance;
    internal static MSServer Instance => _instance ??= new MSServer();
    private readonly ThreadPoolMS _threadPool;
    
    internal void Start(IPAddress ip, int port)
    {
        IPEndPoint localEndPoint = new IPEndPoint(ip, port);
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        server.Bind(localEndPoint);
        server.Listen();

        while (true)
        {
            try
            {
                _threadPool.EnqueueTask(server.Accept());
                //Console.WriteLine("Proxy has been contacted by a new microservice");
                Log.WriteLog(LogLevel.Info, "Proxy has been contacted by a new microservice" );
            }
            catch (Exception e)
            {
                //Console.Error.WriteLine("Couldn't enqueue a new task : " + e);
                Log.WriteLog(LogLevel.Error, "Couldn't enqueue a new task : " + e);
            }
        }
    }
}

internal class ThreadPoolMS
{
    private readonly BlockingCollection<Socket> _tasksQueue = new BlockingCollection<Socket>();
    private readonly Thread[] _threads = new Thread[Environment.ProcessorCount];
    
    internal ThreadPoolMS()
    {
        for (int i = 0; i < 1; i++)
        {
            _threads[i] = new Thread(StartWorking) { IsBackground = true };
            _threads[i].Start();
        }
    }

    private void StartWorking()
    {
        foreach (Socket task in _tasksQueue.GetConsumingEnumerable())
        {
            try
            {
                HandleRequest(task);
            }
            catch (Exception e)
            {
                //Console.Error.WriteLine("Couldn't process an incoming connection : " + e);
                Log.WriteLog(LogLevel.Error, "Couldn't process an incoming connection : " + e);
            }
        }
    }

    private void HandleRequest(Socket client)
    {
        try
        {
            byte[] buffer = new byte[512];
            int length = client.Receive(buffer, buffer.Length, SocketFlags.None);
            int maxLength = length;
            byte[] bodyTemp = buffer.ToArray();

            while (length == 512)
            {
                length = client.Receive(buffer, buffer.Length, SocketFlags.None);

                maxLength += length;
                byte[] temp = new byte[bodyTemp.Length + length];
                Buffer.BlockCopy(bodyTemp, 0, temp, 0, bodyTemp.Length);
                Buffer.BlockCopy(buffer, 0, temp, bodyTemp.Length, buffer.Length);
                bodyTemp = temp;
            }

            byte[] body = new byte[maxLength];
            Buffer.BlockCopy(bodyTemp, 0, body, 0, maxLength);

            byte[] resp;
            ToRegister tr;
            try
            {
                tr = ToRegister.Parser.ParseFrom(body);
            }
            catch (Exception)
            {
                resp = "FO"u8.ToArray();
                client.Send(resp);
                return;
            }

            try
            {
                MSRegister.Instance.RegisterNewMicroService(tr);
                LoadBalancer.ReloadLB();
            }
            catch (Exception)
            {
                resp = "GTFO"u8.ToArray();
                client.Send(resp);
                return;
            }

            resp = "<|ACK|>"u8.ToArray();
            client.Send(resp);
        }
        catch (Exception e)
        {
            //Console.Error.WriteLine("Couldn't process an incoming connection : " + e);
            Log.WriteLog(LogLevel.Error, "Couldn't process an incoming connection : " + e);
        }
        
    }
    
    internal void EnqueueTask(Socket task)
    {
        _tasksQueue.Add(task);
    }
    
    internal void Shutdown()
    {
        _tasksQueue.CompleteAdding();
    }
}