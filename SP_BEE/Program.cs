using System.Net;
using SP_BEE;

internal class Program
{
    static void Main(string[] args)
    {
        MSServer msServer = MSServer.Instance;

        msServer.Start(IPAddress.Parse("127.0.0.1"), 9000);
        
        
    }
}