using System.Net;
using System.Net.Sockets;
using System.Text;
using BEE;
using Google.Protobuf;
using MS_Lib;
using MSLib.Proto;
using MSTest.Proto;

namespace Machin;

internal class Program
{
    static byte[] retrieveResp(Socket socket)
    {
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

        return body;
    }
    
    
    static async Task Main(string[] args)
    {
        
        Socket socket1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipEndPoint1 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9001);
        socket1.Connect(ipEndPoint1);
        // //
        // // Socket socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // // IPEndPoint ipEndPoint2 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8001);
        // // // socket2.Connect(ipEndPoint2);
        // //
        Packet packetGetFreezbee = new Packet()
        {
            Route = "Freezbee",
            Fonction = "GetFreezbee",
            BodyType = "Freezbee"
        };
        
        SPPacket spPacketGetFreezbee = new SPPacket()
        {
            Msname = "TEST",
            Body = packetGetFreezbee.ToByteString()
        };
        //
        byte[] keyclient = AES.getKey("127.0.0.1");
        byte[] iv = AES.getIV(0);
        //
        byte[] cypher = AES.chiffre(spPacketGetFreezbee.ToByteArray(), keyclient, iv);
        socket1.Send(cypher);
        //
        byte[] resp = retrieveResp(socket1);
        byte[] decypher = AES.dechiffre(resp, keyclient, iv);
        //
        Response sp = Response.Parser.ParseFrom(decypher);
        Console.WriteLine(sp.StatusCode);
        //
        // // SPPacket D = SPPacket.Parser.ParseFrom(c);
        // Console.WriteLine("d : " + D.Msname);


        // for (;;)
        // {
        //     Console.WriteLine("Sending a new request :");
        //     socket1.Send(getFreezbee);
        //     Response r = retrieveResp(socket1);
        //     Console.WriteLine("code : " + r.StatusCode + ", description : " + r.StatusDescription);
        //
        //     foreach (ByteString b in r.Body)
        //     {
        //         Freezbee f = Freezbee.Parser.ParseFrom(b);
        //         Console.WriteLine("id : " + f.IdModele + ", nom : " + f.NameModele);
        //     }
        //     Console.WriteLine("Sleeping 50 ms");
        //     Thread.Sleep(1000);
        // }

        // socket2.Send(getFreezbee);
        // r = retrieveResp(socket2);
        // Console.WriteLine("code : " + r.StatusCode + ", description : " + r.StatusDescription);
        // foreach (ByteString b in r.Body)
        // {
        //     Freezbee f = Freezbee.Parser.ParseFrom(b);
        //     Console.WriteLine("id : " + f.IdModele + ", nom : " + f.NameModele);
        // }

        // Task t = Task.Run(() =>
        //     {
        //         socket2.Send(getFreezbee);
        //         r = retrieveResp(socket2);
        //         Console.WriteLine("code : " + r.StatusCode + ", description : " + r.StatusDescription);
        //         foreach (ByteString b in r.Body)
        //         {
        //             Freezbee f = Freezbee.Parser.ParseFrom(b);
        //             Console.WriteLine("id : " + f.IdModele + ", nom : " + f.NameModele);
        //         }
        //     } 
        //     ); 
        
        
        // socket1.Send(getFreezbee);
        // r = retrieveResp(socket1);
        // Console.WriteLine("code : " + r.StatusCode + ", description : " + r.StatusDescription);
        // foreach (ByteString b in r.Body)
        // {
        //     Freezbee f = Freezbee.Parser.ParseFrom(b);
        //     Console.WriteLine("id : " + f.IdModele + ", nom : " + f.NameModele);
        // }

        // t.Wait();

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
        
        Authentication authentication = Authentication.Instance;

        string usernameDN = "svcBee";
        string password = "sdfgSDFG1&";
        
        authentication.fill(usernameDN, password, "bee.bee", "srvbee01.bee.bee",389);

        User user = await authentication.searchAD("james.brown");
        
        Console.WriteLine("User:" + user.dn + "Group: " + user.groups[0] + " SAM: " + user.sam);

        bool check = await authentication.AuthenticateUser(user.sam, "cr$3Dkv4*");
        
        Console.WriteLine(check);

        bool isGroup = authentication.CheckGroup("GG_SHARE_PROD", user.groups);
        
        Console.WriteLine("is in group: " + isGroup);

    }
}