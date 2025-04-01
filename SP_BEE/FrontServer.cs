using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using BEE;

namespace SP_BEE;

internal class FrontServer
{
    private FrontServer()
    {
        _threadPool = new ThreadPoolFront();
    }

    private static FrontServer? _instance;
    internal static FrontServer Instance => _instance ??= new FrontServer();
    private readonly ThreadPoolFront _threadPool;
    
    internal void Start(IPAddress ip, int port)
    {
        IPEndPoint localEndPoint = new IPEndPoint(ip, port);
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        server.Bind(localEndPoint);
        server.Listen();
        Console.WriteLine("Ready To Listen");
        while (true)
        {
            try
            {
                _threadPool.EnqueueTask(server.Accept());
                Console.WriteLine("Enqueued Task");

            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Couldn't enqueue a new task : " + e);
            }
        }
    }
}

internal class ThreadPoolFront
{
    private readonly BlockingCollection<Socket> _tasksQueue = new BlockingCollection<Socket>();
    private readonly Thread[] _threads = new Thread[Environment.ProcessorCount];
    
    internal ThreadPoolFront()
    {
        for (int i = 0; i < Environment.ProcessorCount; i++)
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
        try
        {
            while (client.Connected)
            {
                byte[] buffer = new byte[512];
                int length;

                length = await client.ReceiveAsync(buffer, SocketFlags.None);
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

                SPPacket packet;
                byte[] resp;

                try
                {
                    Console.WriteLine("Received : ");
                    foreach (byte b in body)
                        Console.Write(b);
                    Console.WriteLine();
                    packet = SPPacket.Parser.ParseFrom(body);
                }
                catch (Exception)
                {
                    await client.SendAsync("FO"u8.ToArray());
                    return;
                }
                //TODO
                Authentication authentication = Authentication.Instance;
                try
                {
                    User user = await authentication.searchAD(packet.Username);
                    //TODO parse ad group
                    if (await authentication.AuthenticateUser(user.dn, packet.Password))
                    {
                        Console.WriteLine("Client authenticated");
                    }

                }
                catch(Exception e) 
                {
                    await client.SendAsync("GTFO"u8.ToArray());
                    Console.Error.WriteLine("Authentication failed");
                    return;
                }
                
                try
                {
                    resp = LoadBalancer.SendRequest(packet);
                    Console.WriteLine("Ready to send back : ");
                    foreach (byte b in resp)
                        Console.Write(b);
                    Console.WriteLine();
                }
                catch (Exception)
                {
                    await client.SendAsync("GTFO"u8.ToArray());
                    return;
                }

                await client.SendAsync(resp);
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("Couldn't process an incoming connection : " + e);
        }
        finally
        {
            client.Close();
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