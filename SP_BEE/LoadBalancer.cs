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
            else
            {
                //TODO throw ? jamais sensé arriver
            }
        }

        mut.ReleaseMutex();
    }
    
    internal static byte[] SendRequest(SPPacket packet)
    {
        mut.WaitOne(0);
        bool flag = false;

        Count c = Counts.Find(p => p.type == packet.Msname);
        Socket client;
        while (true)
        {
            MicroService ms = MSRegister.Instance.Register.Find(p => p.type == packet.Msname && p.id == c.count) ??
                              throw new InvalidOperationException();
            c.count = (c.count + 1) / c.max;

            try
            {
                ms.socket.Send(packet.Body.ToByteArray());
                client = ms.socket;
                break;
            }
            catch (Exception e)
            {
                MSRegister.Instance.DeleteMicroService(ms.name);
                flag = true;
            }
        }
        if(flag)
            ReloadLB();
        
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
        return body;

    }
    
}

internal struct Count
{
    internal string type;
    internal int count;
    internal int max;
}