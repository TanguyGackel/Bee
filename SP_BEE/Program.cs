using System.Net;
using SP_BEE;

internal class Program
{
    static void Main(string[] args)
    {
        using StreamReader reader = new StreamReader("./.config");
        LoadBalancer.ip = reader.ReadLine();
        IPAddress ipMS = IPAddress.Parse(LoadBalancer.ip);
        int portMS = int.Parse(reader.ReadLine());
        
        IPAddress ipF = IPAddress.Parse(reader.ReadLine());
        int portF = int.Parse(reader.ReadLine());
        
        Authentication authentication = Authentication.Instance;
        Console.WriteLine("Saisir AD username");
        string username = Authentication.ReadPassword();
        Console.WriteLine("Saisir AD password");
        string password = Authentication.ReadPassword();
        
        authentication.fill(username, password, "bee.bee", "srvbee01",389);
        
        DateTime d = DateTime.Now;
        
        StreamWriter Output = new StreamWriter("./Output." + d.Day + d.Hour + d.Minute + d.Second + ".txt")
        {
            AutoFlush = true
        };
        StreamWriter Error = new StreamWriter("./Error." + d.Day + d.Hour + d.Minute + d.Second + ".txt")
        {
            AutoFlush = true    
        };

        Console.SetOut(Output);
        Console.SetError(Error);
        
        MSServer msServer = MSServer.Instance;
        _ = Task.Run(() => msServer.Start(ipMS, portMS));
        
        FrontServer fServer = FrontServer.Instance;
        fServer.Start(ipF, portF);
    }
}