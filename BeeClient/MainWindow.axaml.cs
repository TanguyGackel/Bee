using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BeeClient;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new MainWindowViewModel();
    }
}

public sealed class MainWindowViewModel : ViewModel
{
    private ObservableCollection<ObjectList> _lists;

    public ObservableCollection<ObjectList> Lists
    {
        get => _lists;
        set
        {
            _lists = value;
            OnPropertyChanged();
        }
    }

    private string _dataGridId = "Id";

    public string DatagridId
    {
        get => _dataGridId;
        set
        {
            _dataGridId = value;
            OnPropertyChanged();
        }
    }
    
    private string _dataGridName = "Name";

    public string DatagridName
    {
        get => _dataGridName;
        set
        {
            _dataGridName = value;
            OnPropertyChanged();
        }
    }

    public MainWindowViewModel()
    {
        _lists = new ObservableCollection<ObjectList>();        
    }
    
    
}