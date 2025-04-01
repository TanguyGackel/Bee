using System.Net;
using System.Net.Sockets;
using BEE;
using Google.Protobuf;
using MSLib.Proto;
using MSTest.Proto;

namespace Machin;

internal class Program
{
    static void Main(string[] args)
    {
        // Type type = Type.GetType("Machin.Program");
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
        
        socket.Connect(ipEndPoint);
        
        Packet packet = new Packet()
        {
            Route = "Freezbee",
            Fonction = "GetFreezbee",
            BodyType = "Freezbee"
        };

        SPPacket spPacket = new SPPacket()
        {
            Msname = "TEST",
            Body = packet.ToByteString()
        };

        byte[] test = spPacket.ToByteArray();
        socket.Send(spPacket.ToByteArray());

        byte[] buffer = new byte[512];
        int length = socket.Receive(buffer, buffer.Length, SocketFlags.None);
        int maxLength = length;
        byte[] bodyTemp = buffer.ToArray();
            
        while (length == 512)
        {
            try
            {
                length = socket.Receive(buffer, buffer.Length, SocketFlags.None);
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

        Response resp = Response.Parser.ParseFrom(body);
        Console.WriteLine(resp.StatusCode);

        foreach (ByteString b in resp.Body)
        {
            Freezbee f = Freezbee.Parser.ParseFrom(b);
        }


        // Freezbee f = new Freezbee()
        // {
        //     IdModele = 2,
        // };
        //
        // Test test = new Test()
        // {
        //     IdTest = 2,
        // };
        //
        // Packet p = new Packet()
        // {
        //     Route = "Freezbee",
        //     Fonction = "GetFreezbeeTestById",
        //     BodyType = "Freezbee",
        //     Body = f.ToByteString()
        // };
        //
        // client.Send(p.ToByteArray());
        //
        // byte[] t = new byte[2048];
        // int length = client.Receive(t, 2048, SocketFlags.None);
        //
        // byte[] a = new byte[length];
        // Buffer.BlockCopy(t, 0, a, 0, length);
        //
        // Response packet = Response.Parser.ParseFrom(a);
        //
        // Console.WriteLine("Status code : " + packet.StatusCode);
        // Console.WriteLine("Status description : " + packet.StatusDescription);
        // Console.WriteLine("Status bodyType : " + packet.BodyType);
        //
        // // foreach (ByteString b in packet.Body)
        // // {
        // //     Freezbee r = Freezbee.Parser.ParseFrom(b);
        // //     
        // //     Console.WriteLine("Freezbee " + r.IdModele + " nom " + r.NameModele + " description " + r.Description);
        // // }
        //
        // foreach (ByteString b in packet.Body)
        // {
        //     TestFreezbee tf = TestFreezbee.Parser.ParseFrom(b);
        //     Console.WriteLine("Test id " + tf.Id + "Name Test " + tf.Name + " Description " + tf.Description + " Type " + tf.Type);
        // }
        //
        // // foreach (ByteString b in packet.Body)
        // // {
        // //     Test tf = Test.Parser.ParseFrom(b);
        // //     Console.WriteLine("Id: " + tf.IdTest + " Name " + tf.NameTest + " Validate " + tf.Validate);
        // // }
        //
        // // foreach (ByteString b in packet.Body)
        // // {
        // //     ProcedeFabrication pf = ProcedeFabrication.Parser.ParseFrom(b);
        // //     Console.WriteLine("ID: " + pf.Id + " Name " + pf.Name + " Description " + pf.Description);
        // // }

    }
}