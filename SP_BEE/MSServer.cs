using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using BEE;
using Google.Protobuf;

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
        Log.WriteLog(LogLevel.Info, "Ready to listen incoming microservices handshake requests");
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
        Log.WriteLog(LogLevel.Info, "Start exchanging keys");
        
        try
        {
            using RSA rsa = RSA.Create();
            client.Send(rsa.ExportRSAPublicKey());
            Log.WriteLog(LogLevel.Info, "Created and sent the RSA public key : " + Convert.ToBase64String(rsa.ExportRSAPublicKey()));
            
            byte[] rsaKey = receiveBytes(client);
            Log.WriteLog(LogLevel.Info, "Received a RSA public key : " + Convert.ToBase64String(rsaKey));

            byte[] encryptedTr = receiveBytes(client);
            Log.WriteLog(LogLevel.Info, "Encrypted tr" + Convert.ToBase64String(encryptedTr));
            byte[] decryptedTr = rsa.Decrypt(encryptedTr, RSAEncryptionPadding.Pkcs1);
            
            Log.WriteLog(LogLevel.Info, "Received and decrypted TR : " + Encoding.UTF8.GetString(decryptedTr));
            
            ToRegister tr;
            try
            {
                tr = ToRegister.Parser.ParseFrom(decryptedTr);
            }
            catch (Exception)
            {
                Log.WriteLog(LogLevel.Warning, "Couldn't parse TR");
                client.Send("FO"u8.ToArray());
                return;
            }
            Log.WriteLog(LogLevel.Info, "Received AES key : " + Convert.ToBase64String(tr.Aes.ToByteArray()));

            using Aes aes = Aes.Create();

            using RSA rsa2 = RSA.Create();
            rsa2.ImportRSAPublicKey(rsaKey, out _);
            byte[] cypheredAesKey = rsa2.Encrypt(aes.Key, RSAEncryptionPadding.Pkcs1);

            client.Send(cypheredAesKey);
            Log.WriteLog(LogLevel.Info, "Created and sent the AES key : " + Convert.ToBase64String(aes.Key));

            try
            {
                MSRegister.Instance.RegisterNewMicroService(tr, aes.Key);
                LoadBalancer.ReloadLB();
            }
            catch (Exception)
            {
                Log.WriteLog(LogLevel.Warning, "Couldn't register new MS");

                client.Send("GTFO"u8.ToArray());
                return;
            }
        }
        catch (Exception e)
        {
            //Console.Error.WriteLine("Couldn't process an incoming connection : " + e);
            Log.WriteLog(LogLevel.Error, "Couldn't process an incoming connection : " + e);
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
    
    internal void EnqueueTask(Socket task)
    {
        _tasksQueue.Add(task);
    }
    
    internal void Shutdown()
    {
        _tasksQueue.CompleteAdding();
    }
}