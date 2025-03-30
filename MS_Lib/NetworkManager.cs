using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using MSLib.Proto;

namespace MS_Lib;

public class NetworkManager
{
    private NetworkManager()
    {
        Cts = new CancellationTokenSource();
        _threadPool = new ThreadPool(Cts.Token);
        _routes = new Dictionary<string, Route>();
        _self = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    private static NetworkManager? _instance;
    public static NetworkManager Instance => _instance ??= new NetworkManager();

    public readonly CancellationTokenSource Cts;
    public readonly Dictionary<string, Route> _routes;
    
    private readonly Socket _self;
    private readonly ThreadPool _threadPool;

    public void AddRoute(string s, Route r)
    {
        _routes.Add(s, r);
    }
    
    public void Start(IPAddress ip, int port, IEnumerable<Client> clients)
    {
        foreach (var t in clients)
            Handshake(t);
        
        CreateServer(ip, port);
    }

    public void CreateServer(IPAddress ip, int port)
    {
        
        IPEndPoint localEndPoint = new IPEndPoint(ip, port);
        _self.Bind(localEndPoint);
        Console.WriteLine("Ready to listen");
        _self.Listen();

        while (!Cts.Token.IsCancellationRequested)
        {
            try
            {
                _threadPool.EnqueueTask(_self.Accept());
                Console.WriteLine("Received a request, enqueued it");
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
        int length = await client.ReceiveAsync(buffer);
        byte[] body = buffer.ToArray();
            
        while (length > 0)
        {
            length = await client.ReceiveAsync(buffer);
            byte[] temp = new byte[body.Length + length];
            Buffer.BlockCopy(body, 0, temp, 0, body.Length);
            Buffer.BlockCopy(buffer, 0, temp, body.Length, buffer.Length);
            body = temp;
        }

        Packet packet = Packet.Parser.ParseFrom(body);
        byte[] response = await NetworkManager.Instance._routes.First(r => r.Key.Equals(packet.Route)).Value.Call(packet);

        await client.SendAsync(response);
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
    
    public Client(string? domainName, IPAddress? ip, int port)
    {
        this.ip = ip;
        this.domainName = domainName;
        this.port = port;
    }
    
    public IPAddress? ip { get; private set; }
    public int port { get; private set; }
    public string? domainName { get; private set; }
}