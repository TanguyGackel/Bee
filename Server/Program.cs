using System.Net;
using System.Net.Sockets;
using System.Text;

internal class Program
{
    static void Main(string[] args)
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipEndPoint = new(IPAddress.Any, 9999);
        socket.Bind(ipEndPoint);
        socket.Listen();

        Socket client = socket.Accept();

        byte[] buffer = new byte[1024];
        int length = client.Receive(buffer);

        string response = Encoding.UTF8.GetString(buffer, 0, length);
        
        Console.WriteLine(response);
    }
}