using System.Net;
using Microsoft.IdentityModel.Tokens;
using MS_Lib;
using MS_RD.Routes;

internal class Program
{
    static void Main(string[] args)
    {
        Dictionary<string, string?> conf = new Dictionary<string, string?>();


        if (args[1] == "f" && args[2].IsNullOrEmpty())
            readConfFile(conf, args[2]);
        else
            readInput(conf);


        DatabaseConnector db = DatabaseConnector.Instance;
        db.Type = ConnectionType.WindowsAuthentication;
        db.Credentials = "";
        db.Source = "local";
        db.DB = "beeDB";
        db.Encryption = false;
        db.ConstructConnectionString();

        NetworkManager nm = NetworkManager.Instance;
        nm.AddRoute("Freezbee", new FreezbeeRoutes());

        Console.Write("\nNombre de serveurs à contacter :");
        int.TryParse(Console.ReadLine(), out int n);

        Client[] list = new Client[n];
        for (int i = 0; i < n; i++)
        {
            Console.Write("\nNom de domaine du serveur " + (n+1) + " :");
            string? hostname = Console.ReadLine();

            IPAddress? ip = null;
            if (hostname.IsNullOrEmpty())
            {
                Console.Write("\nIp du serveur " + (n + 1) + " :");
                IPAddress.TryParse(Console.ReadLine(), out ip);
            }
            
            Console.Write("\nPort du serveur " + (n+1) + " :");
            int.TryParse(Console.ReadLine(), out int port);

            list[i] = new Client(hostname, ip, port);
        }




        nm.Start();
    }

    static void readConfFile(Dictionary<string, string> conf, string path)
    {
        using StreamReader reader = new StreamReader(path);
        conf.Add("ipServer", reader.ReadLine());
        conf.Add("portServer", reader.ReadLine());
        conf.Add("dbConnectionType", reader.ReadLine());
        conf.Add("dbSource", reader.ReadLine());
        conf.Add("db", reader.ReadLine());
        conf.Add("clientNumber", reader.ReadLine());

        int.TryParse(conf["clientNumber"], out int n);
        for (int i = 0; i < n; i++)
        {
            conf.Add("client" + i, reader.ReadLine());
        }
    }

    static void readInput(Dictionary)
    {
        
    }
}