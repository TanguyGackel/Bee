using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Data.SqlClient;
using MS_Lib;
using MS_Test.Models;

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
        
        DatabaseConnector db = DatabaseConnector.Instance;
        db.Type = ConnectionType.WindowsAuthentication;
        db.Credentials = "";
        db.Source = "local";
        db.DB = "BeeDB";
        db.Encryption = false;
        db.ConstructConnectionString();


        List<ProcedeFabrication> procede =  Test.GetTestProcedeById(2);

    }
}