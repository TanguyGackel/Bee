using Avalonia.Controls;
using Avalonia.Markup.Xaml;


namespace Client;

public class MainWindow : Window
{
    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new MainWindowViewModel();
    }
}