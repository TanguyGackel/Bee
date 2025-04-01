using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
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
        MessageExtensions.ToByteString();
        
        return new SPPacket()
        {
            Msname = Msname,
            Body = packet.ToByteString()
        };
    }

    internal static Packet PreparePacket<T>(string route, string fonction, string bodyType, T body) where T : IMessage
    {
        
    }
}