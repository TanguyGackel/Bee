using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Data.SqlClient;
internal class Program  
{
    static void Main(string[] args)
    {
        // IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
        // Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // s.Connect(ipEndPoint);
        //
        // byte[] messageBytes = "Hello World"u8.ToArray();
        //
        // s.Send(messageBytes);

        string str = "Data Source=localhost; Initial Catalog=Bee; Integrated Security=SSPI; Encrypt=False";
        SqlConnection connection = new SqlConnection(str);
        connection.Open();


    }
}