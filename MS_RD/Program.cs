using System.Net;
using Microsoft.IdentityModel.Tokens;
using MS_Lib;
using MS_RD.Routes;

internal class Program
{
    internal static void Main(string[] args)
    {
        DateTime d = DateTime.Now;
        
        StreamWriter Output = new StreamWriter("./Output." + d.Day + d.Hour + d.Minute + d.Second + ".txt")
        {
            AutoFlush = true
        };
        StreamWriter Error = new StreamWriter("./Error." + d.Day + d.Hour + d.Minute + d.Second + ".txt")
        {
            AutoFlush = true
        };
        
        
        Dictionary<string, string> conf = new Dictionary<string, string>();


        if (args.Length >= 2)
        {
            if (args[0] == "-f")
                Tools.ReadConfFile(conf, args[1]);
            else
                throw new Exception();
        }
        else
            Tools.ReadInput(conf);
        Console.SetOut(Output);
        Console.SetError(Error);

        DatabaseConnector db = DatabaseConnector.Instance;
        db.Type = conf["dbConnectionType"] == "Credentials" ? ConnectionType.Password : ConnectionType.WindowsAuthentication;
        if (db.Type == ConnectionType.Password)
        {
            Console.Write("\nlogin :");
            string? login = Console.ReadLine();
            Console.Write("\npassword :");
            string passwd = Tools.ReadPassword();
            db.Credentials = login + "," + passwd;
        }
        db.Source = conf["dbSource"];
        db.DB = conf["db"];
        db.Encryption = false;
        db.ConstructConnectionString();

        NetworkManager nm = NetworkManager.Instance;
        nm.AddRoute("Freezbee", new FreezbeeRoutes());

        List<Client> clients = new List<Client>();
        Dictionary<string, string> confClients = conf.Where(c => c.Key.Contains("clientConf")).ToDictionary();

        foreach (string v in confClients.Values)
        {
            string[] s = v.Split(",");  //hostname,ip,port

            IPAddress cip = null;
            if (s[0].IsNullOrEmpty())
                IPAddress.TryParse(s[1], out cip);
            int.TryParse(s[2], out int cport);
            
            clients.Add(new Client(s[0], cip, cport));
        }
        
        IPAddress.TryParse(conf["ipServer"], out IPAddress ip);
        int.TryParse(conf["portServer"], out int port);
        
        nm.Start(conf["instanceName"], "RD", ip, port, clients);
    }

    

}