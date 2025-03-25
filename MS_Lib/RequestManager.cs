using System.Net;
using System.Net.Sockets;

namespace MS_Lib;

public class RequestManager
{
    private RequestManager()
    {
    }

    private static RequestManager? _instance;
    public static RequestManager Instance => _instance ??= new RequestManager();

    private static readonly Socket Self = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    
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
        // Self.Accept();
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