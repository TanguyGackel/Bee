using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
using MSLib.Proto;
using MSTest.Proto;

namespace Machin;

internal class Program
{
    static void Main(string[] args)
    {
        // Type type = Type.GetType("Machin.Program");
        Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
        
        client.Connect(ipEndPoint);
        
        Freezbee f = new Freezbee();
        
        Packet p = new Packet()
        {
            Route = "Freezbee",
            Fonction = "GetFreezbee",
            BodyType = "Freezbee",
            Body = f.ToByteString()
        };
        
        client.Send(p.ToByteArray());

        byte[] t = new byte[2048];
        int length = client.Receive(t, 2048, SocketFlags.None);

        byte[] a = new byte[length];
        Buffer.BlockCopy(t, 0, a, 0, length);
        
        Response packet = Response.Parser.ParseFrom(a);
        
        Console.WriteLine("Status code : " + packet.StatusCode);
        Console.WriteLine("Status description : " + packet.StatusDescription);
        Console.WriteLine("Status bodyType : " + packet.BodyType);

        foreach (ByteString b in packet.Body)
        {
            Freezbee r = Freezbee.Parser.ParseFrom(b);
            
            Console.WriteLine("Freezbee " + r.IdModele + " nom " + r.NameModele);
        }

    }
}