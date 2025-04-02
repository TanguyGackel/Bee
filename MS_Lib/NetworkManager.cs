using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using BEE;
using Google.Protobuf;
using MSLib.Proto;

namespace MS_Lib;

public class NetworkManager
{
    private NetworkManager()
    {
        _threadPool = new ThreadPool();
        _routes = new Dictionary<string, Route>();
        _self = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    private static NetworkManager? _instance;
    public static NetworkManager Instance => _instance ??= new NetworkManager();

    public readonly Dictionary<string, Route> _routes;
    
    private readonly Socket _self;
    private readonly ThreadPool _threadPool;

    public void AddRoute(string s, Route r)
    {
        _routes.Add(s, r);
    }
    
    public void Start(string name, string type, IPAddress ip, int port, string group, List<Client> clients)
    {
        _threadPool.SetIP(ip.ToString());

        int count = 0;
        ToRegister infos = new ToRegister()
        {
            Name = name,
            Ip = ip.ToString(),
            Port = port,
            Type = type,
            Group = group
        };
        foreach (var t in clients)
        {
            try
            {
                Handshake(t, infos);
            }
            catch (Exception e)
            {
                count++;
                Console.Error.WriteLine(DateTime.Now + " " + e);
            }

            if (count >= clients.Count)
                throw new Exception("Couldn't connect to any remote proxy");
        }
        
        CreateServer(ip, port);
    }

    public void CreateServer(IPAddress ip, int port)
    {
        
        IPEndPoint localEndPoint = new IPEndPoint(ip, port);
        _self.Bind(localEndPoint);
        _self.Listen();
        Console.WriteLine("Ready To Listen");

        while (true)
        {
            try
            {
                _threadPool.EnqueueTask(_self.Accept()); 
                Console.WriteLine("Enqueued Task");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Couldn't enqueue a new task : " + e);
            }
            
        }
    }
    
    public void Handshake(Client c, ToRegister infos)
    {
        Console.WriteLine("Doing a handshake");
        IPEndPoint ipEndPoint;
        
        if (c.ip != null)
            ipEndPoint = new IPEndPoint(c.ip, c.port);
        else if (c.domainName != null)
            ipEndPoint = new IPEndPoint(Dns.GetHostAddresses(c.domainName)[0], c.port);
        else
            throw new ArgumentException("Service should have either an ip address or a domain name");

        Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client.Connect(ipEndPoint);

        client.Send(infos.ToByteArray());

    }
}

internal class ThreadPool
{
    private readonly BlockingCollection<Socket> _tasksQueue = new BlockingCollection<Socket>();
    private readonly Thread[] _threads = new Thread[Environment.ProcessorCount];
    private string ip;

    internal ThreadPool()
    {
        for (int i = 0; i < Environment.ProcessorCount; i++)
        {
            _threads[i] = new Thread(StartWorking) { IsBackground = true };
            _threads[i].Start();
        }
    }
    public void SetIP(string ip)
    {
        this.ip = ip;
    }
    private void StartWorking()
    {
        foreach (Socket task in _tasksQueue.GetConsumingEnumerable())
        {
            try
            {
                Console.WriteLine("Start new task");
                HandleRequest(task);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Couldn't process an incoming connection : " + e);
            }
        }
    }

    private async Task HandleRequest(Socket client)
    {
        int count = 0;
        IPEndPoint ipEndPointClient = (IPEndPoint) client.RemoteEndPoint;
        string clientIp = ipEndPointClient.Address.ToString();
        byte[] keyClient = AES.getKey(clientIp);
        byte[] keyServer = AES.getKey(this.ip);
        
        try
        {
            byte[] iv = new byte[16];
            await client.ReceiveAsync(iv, SocketFlags.None);
            
            byte[] buffer = new byte[512];
            int length = await client.ReceiveAsync(buffer, SocketFlags.None);
            int maxLength = length;
            byte[] bodyTemp = buffer.ToArray();

            while (length == 512)
            {
                length = await client.ReceiveAsync(buffer, SocketFlags.None);

                maxLength += length;
                byte[] temp = new byte[bodyTemp.Length + length];
                Buffer.BlockCopy(bodyTemp, 0, temp, 0, bodyTemp.Length);
                Buffer.BlockCopy(buffer, 0, temp, bodyTemp.Length, buffer.Length);
                bodyTemp = temp;
            }

            byte[] body = new byte[maxLength];
            Buffer.BlockCopy(bodyTemp, 0, body, 0, maxLength);

            Packet packet;
            byte[] response;

            try
            {
                Console.WriteLine("Received : ");
                // foreach (byte b in body)
                //     Console.Write(b);
                // Console.WriteLine();
                byte[] decyphered = AES.dechiffre(body, keyClient, iv);

                packet = Packet.Parser.ParseFrom(decyphered);
            }
            catch (Exception)
            {
                Response resp = new Response()
                {
                    StatusCode = 400,
                    StatusDescription = "FO"
                };
                await client.SendAsync(resp.ToByteArray());
                return;
            }

            try
            {
                response = await NetworkManager.Instance._routes.First(r => r.Key.Equals(packet.Route)).Value
                    .Call(packet);
                Console.WriteLine("Ready to send : ");
                // foreach (byte b in response)
                //     Console.Write(b);
                // Console.WriteLine();
            }
            catch (Exception)
            {
                Response resp = new Response()
                {
                    StatusCode = 500,
                    StatusDescription = "GTFO",
                };
                await client.SendAsync(resp.ToByteArray());
                return;
            }

            byte[] cypher = AES.chiffre(response, keyServer, iv);
            await client.SendAsync(cypher);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("Couldn't process an incoming connection : " + e);

        }
        
    }

    public void EnqueueTask(Socket task)
    {
        _tasksQueue.Add(task);
    }
    
    public void Shutdown()
    {
        _tasksQueue.CompleteAdding();
    }
}

public class Client
{
    public Client(string? domainName, IPAddress? ip, int port)
    {
        this.ip = ip;
        this.domainName = domainName;
        this.port = port;
    }
    
    public IPAddress? ip;
    public int port;
    public string? domainName;
}