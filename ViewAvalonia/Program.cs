using System.Net;
using Avalonia;
using Client;
using ViewAvalonia.Views;

namespace ViewAvalonia;

public class Program
{

    
    [STAThread]
    static void Main(string[] args)
    {
        try
        {
            ProxyClient.AddProxy(IPAddress.Parse("127.0.0.1"), 9001);
            // Console.WriteLine("New proxy added");
        }
        catch (Exception e)
        {
            // Console.WriteLine("Problem in adding new proxy : " + e);
            // Console.WriteLine("New proxy added");
        }
        try
        {
            ProxyClient.AddProxy(IPAddress.Parse("10.0.50.23"), 9001);
            // Console.WriteLine("New proxy added");
        }
        catch (Exception e)
        {
            // Console.WriteLine("Problem in adding new proxy : " + e);
        }        
        try
        {
        
            ProxyClient.AddProxy(IPAddress.Parse("10.0.50.24"), 9001);
            // Console.WriteLine("New proxy added");
        }
        catch (Exception e)
        {
            // Console.WriteLine("Problem in adding new proxy : " + e);
        }
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}