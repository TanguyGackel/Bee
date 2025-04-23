using System.Net;
using System.Net.Sockets;
using System.Text;
using BEE;
using Google.Protobuf;
using MSLib.Proto;
using ViewAvalonia;

namespace Client;

internal static class ProxyClient
{
    private static List<Socket> proxys = new List<Socket>();
    // private static int count = 0;
    
    
    internal static async Task AddProxy(IPAddress ip, int port)
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipEndPoint = new IPEndPoint(ip, port);
        await socket.ConnectAsync(ipEndPoint);
        proxys.Add(socket);
    }

    internal static SPPacket PrepareSPPacket(string Msname, Packet packet, string username, string password)
    {
        return new SPPacket()
        {
            Msname = Msname,
            Body = packet.ToByteString(),
            Username = username,
            Password = password
        };
    }

    internal static Packet PreparePacket<T>(string route, string fonction, string bodyType, T body) where T : IMessage
    {
        return new Packet()
        {
            Route = route,
            Fonction = fonction,
            BodyType = bodyType,
            Body = body.ToByteString()
        };
    }

    internal static async Task<byte[]?> SendPacket(byte[] packet)
    {

        var host = Dns.GetHostEntry(Dns.GetHostName());
        var ipAddress = host.AddressList.First(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
        // Console.WriteLine("My ip" + ipAddress);
        
        byte[] keyClient = AES.getKey(ipAddress.ToString());
        byte[] cypher = AES.chiffre(packet,keyClient, AES.getIV(0));
        
        foreach (Socket s in proxys)
        {
            try
            {
                await s.SendAsync(cypher);
                byte[] resp = await retrieveResp(s);
                // Console.WriteLine(Encoding.UTF8.GetString(resp));
                IPEndPoint ipEndPoint = (IPEndPoint)s.RemoteEndPoint;
                byte[] keyserver = AES.getKey(ipEndPoint.Address.ToString());
                byte[] decypher = AES.dechiffre(resp, keyserver, AES.getIV(0));
                // count++;
                return decypher;
            }
            catch (Exception e)
            {
                // Console.Error.WriteLine("Coulnd't send to proxy, trying another one");
            }
        }

        return null;
    }
    
    
    private static async Task<byte[]> retrieveResp(Socket socket)
    {
        byte[] buffer = new byte[512];
        int length = await socket.ReceiveAsync(buffer, SocketFlags.None);
        int maxLength = length;
        byte[] bodyTemp = buffer.ToArray();
            
        while (length == 512)
        { 
            length = await socket.ReceiveAsync(buffer, SocketFlags.None);

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