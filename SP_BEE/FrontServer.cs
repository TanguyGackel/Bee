using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;
using System.Text;
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
        _threadPool.SetIP(ip.ToString());
        
        IPEndPoint localEndPoint = new IPEndPoint(ip, port);
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        server.Bind(localEndPoint);
        server.Listen();
        //Console.WriteLine("Ready To Listen");
        Log.WriteLog(LogLevel.Info, "Ready to listen");
        while (true)
        {
            try
            {
                _threadPool.EnqueueTask(server.Accept());
                //Console.WriteLine("Enqueued Task");
                Log.WriteLog(LogLevel.Info, "Enqueue task");

            }
            catch (Exception e)
            {
                //Console.Error.WriteLine("Couldn't enqueue a new task : " + e);
                Log.WriteLog(LogLevel.Error, "Couldn't enqueue a new task : " + e);
            }
        }
    }
}

internal class ThreadPoolFront
{
    private readonly BlockingCollection<Socket> _tasksQueue = new BlockingCollection<Socket>();
    private readonly Thread[] _threads = new Thread[Environment.ProcessorCount];
    private string ip;
    
    public List<Pair> keystore = new List<Pair>();
    
    internal ThreadPoolFront()
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
                //Console.WriteLine("Start new task");
                Log.WriteLog(LogLevel.Info, "Start new task");
                HandleRequest(task);
            }
            catch (Exception e)
            {
                //Console.Error.WriteLine("Couldn't process an incoming connection : " + e);
                Log.WriteLog(LogLevel.Error, "Couldn't process an incoming connection : " + e );
            }
        }
    }

    private async Task HandleRequest(Socket client)
    {
        int count = 0;
        IPEndPoint ipEndPointClient = (IPEndPoint) client.RemoteEndPoint;
        string clientIp = ipEndPointClient.Address.ToString();

        
        Log.WriteLog(LogLevel.Info, "Ready to handle client's requests :" + clientIp);

        try
        {
            while (client.Connected)
            {
                byte[] iv = AES.getIV(0);

                byte[] buffer = new byte[512];
                int length;
                
                Log.WriteLog(LogLevel.Info, "Ready to receive message from client");

                length = await client.ReceiveAsync(buffer, SocketFlags.None);
                
                Log.WriteLog(LogLevel.Info, "Received message length: " + length);
                
                int maxLength = length;
                byte[] bodyTemp = buffer.ToArray();

                while (length == 512)
                {
                    Log.WriteLog(LogLevel.Info, "Waiting for the entire message");
                    length = await client.ReceiveAsync(buffer, SocketFlags.None);
                    
                    Log.WriteLog(LogLevel.Info, "Processing the rest of the message");
                    
                    maxLength += length;
                    byte[] temp = new byte[bodyTemp.Length + length];
                    Buffer.BlockCopy(bodyTemp, 0, temp, 0, bodyTemp.Length);
                    Buffer.BlockCopy(buffer, 0, temp, bodyTemp.Length, buffer.Length);
                    bodyTemp = temp;
                }

                byte[] body = new byte[maxLength];
                Buffer.BlockCopy(bodyTemp, 0, body, 0, maxLength);

                if (Encoding.UTF8.GetString(body) == "Handshake")
                {
                    Log.WriteLog(LogLevel.Info, "Client wants to do a handshake");
                    KeyExchange(client);
                    continue;
                }
                
                
                SPPacket packet;
                byte[] resp;
                Pair p = keystore.Find(p => p.ip == clientIp);

                try
                {
                    //Console.WriteLine("Received : ");
                    Log.WriteLog(LogLevel.Info, "Decypher key used : " + Convert.ToBase64String(p.decypherKey));
                    
                    string bodyInLog = "Received: " + Convert.ToBase64String(body, 0, body.Length);
                    
                    Log.WriteLog(LogLevel.Info, bodyInLog);
                    
                    byte[] decyphered = AES.dechiffre(body, p.decypherKey, iv);

                    packet = SPPacket.Parser.ParseFrom(decyphered);
                }
                catch (Exception)
                {
                    await client.SendAsync("FO"u8.ToArray());
                    return;
                }
                
                //Console.WriteLine("Debut Auth");
                Log.WriteLog(LogLevel.Info, "Beginning AD auth part");
                
                // Authentication authentication = Authentication.Instance;
                // try
                // {
                //     //Console.WriteLine("SearchAD");
                //     Log.WriteLog(LogLevel.Info, "Enumerate AD users and groups");
                //     User user = await authentication.searchAD(packet.Username);
                //     
                //     //Console.WriteLine("CheckGroup");
                //     Log.WriteLog(LogLevel.Info, "Checking Groups");
                //     if (!authentication.CheckGroup(packet.Msname, user.groups))
                //     {
                //         //Console.Error.WriteLine("Client doesn't have the rights");
                //         Log.WriteLog(LogLevel.Error, $"This user ({packet.Username}) doesn't have the rights access to the ERP");
                //         await client.SendAsync("On t as dit degage"u8.ToArray());
                //         return;
                //     }
                //     //Console.WriteLine("AuthenticateUser");
                //     Log.WriteLog(LogLevel.Info, $"Begining the authentication of user {packet.Username}");
                //     if (!await authentication.AuthenticateUser(user.sam, packet.Password))
                //     {
                //         await client.SendAsync("Oh eh oh"u8.ToArray());
                //
                //         //Console.Error.WriteLine("Client not authenticated");
                //         Log.WriteLog(LogLevel.Error, "user provides the wrong username or password");
                //         return;
                //
                //     }
                //
                // }
                // catch(Exception e) 
                // {
                //     await client.SendAsync("GTFO"u8.ToArray());
                //     //Console.Error.WriteLine("Authentication failed " + e);
                //     Log.WriteLog(LogLevel.Error, $"Authentication of {packet.Username} failed" + e);
                //     return;
                // }
                //Console.WriteLine("Fin Auth");
                Log.WriteLog(LogLevel.Info, "End of process of authentication");
                
                try
                {
                    resp = LoadBalancer.SendRequest(packet);
                    //Console.WriteLine("Ready to send back : ");
                    string respInLog = "Ready to send back : " + Convert.ToBase64String(resp, 0, resp.Length);
                    Log.WriteLog(LogLevel.Info, respInLog);
                }
                catch (Exception)
                {
                    await client.SendAsync("GTFO"u8.ToArray());
                    return;
                }
                Log.WriteLog(LogLevel.Info, "Cypher key used : " + Convert.ToBase64String(p.cypherKey));
                byte[] cypher = AES.chiffre(resp, p.cypherKey, iv);
                
                await client.SendAsync(cypher);

                try
                {
                    count++;
                }
                catch (OverflowException)
                {
                    count = 0;
                }
            }
        }
        catch (Exception e)
        {
            //Console.Error.WriteLine("Couldn't process an incoming connection : " + e);
            Log.WriteLog(LogLevel.Error, "Couldn't process an incoming connection : " + e );
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

    internal void KeyExchange(Socket s)
    {
        try
        {
            using RSA rsa = RSA.Create();
            s.Send(rsa.ExportRSAPublicKey());
            Log.WriteLog(LogLevel.Info, "Exporting public key to client : " + Convert.ToBase64String(rsa.ExportRSAPublicKey()));

            byte[] rsaKey = receiveBytes(s);

            using Aes aes = Aes.Create();
            Log.WriteLog(LogLevel.Info, "Creating AES key : " + Convert.ToBase64String(aes.Key));
            
            byte[] encryptedAesKey = receiveBytes(s);
            byte[] aesKey = rsa.Decrypt(encryptedAesKey, RSAEncryptionPadding.Pkcs1);
            Log.WriteLog(LogLevel.Info, "Received AES key : " + Convert.ToBase64String(aesKey));

            Log.WriteLog(LogLevel.Info, "Importing public key from client : " + Convert.ToBase64String(rsaKey));
            using RSA rsa2 = RSA.Create();
            rsa2.ImportRSAPublicKey(rsaKey, out _);
            byte[] encryptedKey = rsa2.Encrypt(aes.Key, RSAEncryptionPadding.Pkcs1);
            s.Send(encryptedKey);

            Pair p = new Pair()
            {
                cypherKey = aes.Key,
                decypherKey = aesKey,
                ip = ((IPEndPoint)s.RemoteEndPoint).Address.ToString()
            };
            keystore.Add(p);
        }
        catch (Exception e)
        {
            Console.WriteLine("");
        }
    }
    
    public byte[] receiveBytes(Socket client)
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

        return body;
    }
}

internal class Pair
{
    internal byte[] cypherKey;
    internal byte[] decypherKey;
    internal string ip;
}