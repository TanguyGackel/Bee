using System.Net;
using System.Net.Sockets;

internal class Program
{
    static void Main(string[] args)
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipEndPoint = new(IPAddress.Parse("127.0.0.1"), 9999);
        socket.Connect(ipEndPoint);

        byte[] messageBytes = "Hello World"u8.ToArray();

        socket.Send(messageBytes);

    }
}