using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using BEE;
using Google.Protobuf;
using MSLib.Proto;

namespace Client;

internal static class ProxyClient
{
    private static List<Socket> proxys = new List<Socket>();

    internal static void AddProxy(IPAddress ip, int port)
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipEndPoint = new IPEndPoint(ip, port);
        socket.Connect(ipEndPoint);
        proxys.Add(socket);
    }

    internal static SPPacket PrepareSPPacket(string Msname, Packet packet)
    {
        return new SPPacket()
        {
            Msname = Msname,
            Body = packet.ToByteString()
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

    internal static async Task<Response> SendPacket(SPPacket packet)
    {
        foreach (Socket s in proxys)
        {
            try
            {
                await s.SendAsync(packet.ToByteArray());
                return await retrieveResp(s);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Couldnt send packet, trying another proxy");
            }
        }

        throw new Exception("No working proxy found");
    }
    
    private static async Task<Response> retrieveResp(Socket socket)
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

        return Response.Parser.ParseFrom(body);
    }
}