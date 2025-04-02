using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BeeClient;

public abstract class ViewModel : INotifyPropertyChanged
{
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
}