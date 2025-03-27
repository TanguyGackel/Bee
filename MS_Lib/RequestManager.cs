using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MSLib.Proto;

namespace MS_Lib;

public class RequestManager
{
    private RequestManager()
    {
    }

    private static RequestManager? _instance;
    public static RequestManager Instance => _instance ??= new RequestManager();

    private static readonly Socket Self = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    public static readonly CancellationTokenSource Cts = new CancellationTokenSource();
    private static readonly ThreadPool threadPool = new ThreadPool(Cts.Token);
    
    public void Start(IPAddress ip, int port, IEnumerable<Client> clients)
    {
        foreach (var t in clients)
            Handshake(t);
        
        CreateServer(ip, port);
    }

    public async void CreateServer(IPAddress ip, int port)
    {
        
        IPEndPoint localEndPoint = new IPEndPoint(ip, port);
        Self.Bind(localEndPoint);
        Self.Listen();

        while (!Cts.Token.IsCancellationRequested)
        {
            try
            {
                threadPool.EnqueueTask(Self.Accept());
            }
            catch (Exception) //TODO
            {
                
            }
        }


    }
    
    public void Handshake(Client c)
    {
        IPEndPoint ipEndPoint;
        
        if (c.ip != null)
            ipEndPoint = new IPEndPoint(c.ip, c.port);
        else if (c.domainName != null)
            ipEndPoint = new IPEndPoint(Dns.GetHostAddresses(c.domainName)[0], c.port);
        else
            throw new ArgumentException("Service should have either an ip address or a domain name");

        Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client.Connect(ipEndPoint);
        
        byte[] aoe = "<|AOE|>"u8.ToArray();     //acknowledgement of establishment
        client.Send(aoe);       //TODO authentification, chiffrement
    }
}

internal class ThreadPool
{
    private readonly BlockingCollection<Socket> _tasksQueue = new BlockingCollection<Socket>();
    private readonly Thread[] _threads = new Thread[Environment.ProcessorCount];
    private CancellationToken ct;
    
    internal ThreadPool(CancellationToken token)
    {
        ct = token;
        for (int i = 0; i < Environment.ProcessorCount; i++)
        {
            _threads[i] = new Thread(Work) { IsBackground = true };
            _threads[i].Start(token);
        }
    }

    private void Work()
    {
        byte[] body = new byte[512];
        byte[] buffer = new byte[512];
        foreach (Socket task in _tasksQueue.GetConsumingEnumerable(ct))
        {
            int length = task.Receive(buffer);
            body = buffer.ToArray();
            
            while (length > 0)
            {
                length = task.Receive(buffer);
                byte[] temp = new byte[body.Length + length];
                Buffer.BlockCopy(body, 0, temp, 0, body.Length);
                Buffer.BlockCopy(buffer, 0, temp, body.Length, buffer.Length);
                body = temp;
            }

            Packet packet = Packet.Parser.ParseFrom(body);

        }
    }

    public void EnqueueTask(Socket task)
    {
        _tasksQueue.Add(task, ct);
    }
    
    public void Shutdown()
    {
        _tasksQueue.CompleteAdding();
    }
}

public class Client
{
    public Client(IPAddress ip, int port)
    {
        this.ip = ip;
        this.port = port;
    }

    public Client(string domainName, int port)
    {
        this.domainName = domainName;
        this.port = port;
    }
    
    public IPAddress? ip { get; private set; }
    public int port { get; private set; }
    public string? domainName { get; private set; }
}