using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ViewAvalonia.Views;

public class Header : UserControl
{
    public Header()
    {
        AvaloniaXamlLoader.Load(this); 
        DataContext = new HeaderViewModel();
    }
}

public sealed class HeaderViewModel : ViewModel
{
    public HeaderViewModel()
    {
    }
    

}