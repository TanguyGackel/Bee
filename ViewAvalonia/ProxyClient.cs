using System.Net;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using Avalonia.Logging;
using BEE;
using Google.Protobuf;
using MS_Lib;
using MSLib.Proto;
using ViewAvalonia;

namespace Client;

internal static class ProxyClient
{
    private static List<Socket> proxys = new List<Socket>();
    // private static int count = 0;

    public static List<Pair> keystore = new List<Pair>();
    
    internal static async Task AddProxy(IPAddress ip, int port)
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipEndPoint = new IPEndPoint(ip, port);
        await socket.ConnectAsync(ipEndPoint);
        proxys.Add(socket);

        try
        {

            socket.Send("Handshake"u8.ToArray());
            
            byte[] rsaKey = await retrieveResp(socket);
            using RSA rsa = RSA.Create();

            socket.Send(rsa.ExportRSAPublicKey());

            using Aes aes = Aes.Create();
            Console.WriteLine("Sending AES key : " + Convert.ToBase64String(aes.Key));

            using RSA rsa2 = RSA.Create();
            rsa2.ImportRSAPublicKey(rsaKey, out _);
            byte[] encryptedAes = rsa2.Encrypt(aes.Key, RSAEncryptionPadding.Pkcs1);
            socket.Send(encryptedAes);

            byte[] encryptedAesKey = await retrieveResp(socket);
            byte[] aesKey = rsa.Decrypt(encryptedAesKey, RSAEncryptionPadding.Pkcs1);
            Console.WriteLine("Received AES key : " + Convert.ToBase64String(aesKey));

            Pair p = new Pair()
            {
                cypherKey = aes.Key,
                decypherKey = aesKey,
                ip = ip.ToString()
            };
            keystore.Add(p);
        }
        catch (Exception e)
        {
            Console.WriteLine("BAAAAAA");

            Log.WriteLog(LogLevel.Error, "Keys exchange failed " + e);
        }

        Console.WriteLine("Did a handshake");
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
        
        foreach (Socket s in proxys)
        {
            try
            {
                IPEndPoint ipEndPointserver = (IPEndPoint) s.RemoteEndPoint;
                string serverIp = ipEndPointserver.Address.ToString();
                
                Console.WriteLine("Server ip : " + serverIp);


                Pair p = keystore.Find(p => p.ip == serverIp);
                byte[] cypher = AES.chiffre(packet, p.cypherKey, AES.getIV(0));
                
                Console.WriteLine("Cypher's size : " + cypher.Length);
                await s.SendAsync(cypher);
                byte[] resp = await retrieveResp(s);

                
                byte[] decypher = AES.dechiffre(resp, p.decypherKey, AES.getIV(0));
                // count++;
                return decypher;
            }
            catch (Exception e)
            {
                
                Console.WriteLine("Error my captain");
                // Console.Error.WriteLine("Coulnd't send to proxy, trying another one");
            }
        }
        Console.WriteLine("Returning null");

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

internal class Pair
{
    internal byte[] cypherKey;
    internal byte[] decypherKey;
    internal string ip;
}