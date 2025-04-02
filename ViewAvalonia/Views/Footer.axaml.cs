using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ViewAvalonia.Views;

public class Footer : UserControl
{
    public Footer()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new FooterViewModel();
    }
}

public sealed class FooterViewModel : ViewModel
{
    public FooterViewModel()
    {
    }
    

    private string _textExit = "Exit";

    public string TextExit
    {
        get => _textExit;
        set
        {
            _textExit = value;
            OnPropertyChanged();
        }
    }
    
    public void ClickExit()
    {
        Environment.Exit(0);
    }
} 