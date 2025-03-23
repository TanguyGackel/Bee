using System.Net;
using System.Net.Sockets;

internal class Program
{
    static void Main(string[] args)
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress hostip = (Dns.Resolve(IPAddress.Any.ToString())).AddressList[0];
        IPEndPoint ep = new IPEndPoint(hostip, 3700);
        socket.Bind(ep);
        
        
        socket.Listen();
    }
}