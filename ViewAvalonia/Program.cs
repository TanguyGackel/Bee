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
        ProxyClient.AddProxy(IPAddress.Parse("127.0.0.1"), 9001);
        // ProxyClient.AddProxy(IPAddress.Parse("10.0.50.23"), 9001);
        // ProxyClient.AddProxy(IPAddress.Parse("10.0.50.24"), 9001);
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}