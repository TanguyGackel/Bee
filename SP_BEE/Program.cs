using System.Net;
using SP_BEE;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("IP du server qui réceptionne les MS :");
        IPAddress ipMS = IPAddress.Parse(Console.ReadLine());
        Console.WriteLine("Port du server qui réceptionne les MS :");
        int portMS = int.Parse(Console.ReadLine());
        Console.WriteLine("IP du server qui réceptionne les front :");
        IPAddress ipF = IPAddress.Parse(Console.ReadLine());
        Console.WriteLine("Port du server qui réceptionne les front :");
        int portF = int.Parse(Console.ReadLine());

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