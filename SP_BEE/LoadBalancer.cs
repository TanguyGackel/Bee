using System.Net;
using System.Net.Sockets;
using BEE;

namespace SP_BEE;

internal static class LoadBalancer
{
    private static List<Count> Counts = new List<Count>();
    private static Mutex mut = new Mutex();

    internal static void ReloadLB()
    {
        mut.WaitOne();
        List<MicroService> mss = MSRegister.Instance.Register;
        Counts.Clear();
        
        foreach (MicroService ms in mss)
        {
            List<Count> list = Counts.Where(p => p.type == ms.type).ToList();

            if (list.Count == 0)
            {
                Count c = new Count()
                {
                    type = ms.type,
                    count = 0,
                    max = 1
                };
                Counts.Add(c);
                ms.id = 0;
            }
            else if (list.Count == 1)
            {
                Count only = list.First();
                only.max++;

                ms.id = only.max-1;
            }

        }

        mut.ReleaseMutex();
    }
    
    internal static byte[] SendRequest(SPPacket packet)
    {
        byte[] iv = AES.getIV("GetTheFOutOfMyNetwork" + DateTime.Now.Millisecond);
        byte[] decypherKey;
        Console.WriteLine("Searching a microservices to send data");
        mut.WaitOne();
        Count c = Counts.Find(p => p.type == packet.Msname);
        mut.ReleaseMutex();
        Console.WriteLine("Found" + c.max + " microservices");
        
        bool flag = false;

        Socket server;
        while (true)
        {
            MicroService ms = MSRegister.Instance.Register.Find(p => p.type == packet.Msname && p.id == c.count) ??
                              throw new InvalidOperationException();
            Console.WriteLine("Ready to contact microservice " + ms.id + ", name : " + ms.name);
            c.count = (c.count + 1) / c.max;

            byte[] cypherKey = ms.cypherKey;
            decypherKey = ms.decypherKey;
            byte[] cyphered = AES.chiffre(packet.Body.ToByteArray(), cypherKey, iv);

            try
            {
                IPEndPoint ipEndPoint = new IPEndPoint(ms.ip, ms.port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipEndPoint);
                Console.WriteLine("Sending packet");

                socket.Send(iv);
                socket.Send(cyphered);
                server = socket;
                break;
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Micro service couldn't be contacted, deleting it from the list");
                MSRegister.Instance.DeleteMicroService(ms.name);
                flag = true;
            }
        }

        if (flag)
        {
            Console.WriteLine("Need to reload LB config");
            ReloadLB();
        }
        
        
        byte[] buffer = new byte[512];
        int length = server.Receive(buffer, buffer.Length, SocketFlags.None);
        int maxLength = length;
        byte[] bodyTemp = buffer.ToArray();
            
        while (length == 512)
        { 
            length = server.Receive(buffer, buffer.Length, SocketFlags.None);

            maxLength += length;
            byte[] temp = new byte[bodyTemp.Length + length];
            Buffer.BlockCopy(bodyTemp, 0, temp, 0, bodyTemp.Length);
            Buffer.BlockCopy(buffer, 0, temp, bodyTemp.Length, buffer.Length);
            bodyTemp = temp;
        }
        
        server.Close();
        byte[] body = new byte[maxLength];
        Buffer.BlockCopy(bodyTemp, 0, body, 0, maxLength);
        
        
        return AES.dechiffre(body, decypherKey, iv);

    }
    
}

internal struct Count
{
    internal string type;
    internal int count;
    internal int max;
}