using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ViewAvalonia.ToolBox;

namespace ViewAvalonia.Views;

public class MainWindow : Window
{
    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new MainWindowViewModel();
    }
}

public sealed class MainWindowViewModel : ViewModel
{
    #region Login
    private string _login = "";

    public string Login
    {
        get => _login;
        set
        {
            _login = value;
            OnPropertyChanged();
        }
    }   
    
    private string _password = "";

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    public void Connection()
    {
        ConnectionPageVisible = !ConnectionPageVisible;
    }
    
    private bool _connectionPageVisible = true;
    
    public bool ConnectionPageVisible
    {
        get => _connectionPageVisible;
        set
        {
            _connectionPageVisible = value;
            OnPropertyChanged();
        }
    }
    #endregion
    
    private ObservableCollection<Backup> _backups = new ObservableCollection<Backup>();

    
    private ObservableCollection<ObservableObject> _collection = new();
    
    
    public ObservableCollection<Backup> Backups
    {
        get => _backups;
        set
        {
            _backups = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<ObservableObject> OCollection
    {
        get => _collection;
        set
        {
            _collection = value;
            OnPropertyChanged();
        }
    }

    private bool _visible = true;
    public bool Visible
    {
        get => _visible;
        set
        {
            _visible = value;
            OnPropertyChanged();
        }
    }
    
    private bool _buttonVisible = true;

    public bool ButtonVisible
    {
        get => _buttonVisible;
        set
        {
            _buttonVisible = value;
            OnPropertyChanged();
        }
    }

    private Backup _selectedBackup;

    public Backup SelectedBackup
    {
        get => _selectedBackup;
        set
        {
            _selectedBackup = value;
            OnPropertyChanged();
            if (_selectedBackup != null && _selectedBackup.Etat == State.INACTIVE)
            {
                StartAuth = true;
                StopAuth = false;
                RestartAuth = false;
                ButtonVisible = true;
            }
            else if (_selectedBackup != null && _selectedBackup.Etat == State.ACTIVE)
            {
                StartAuth = false;
                StopAuth = true;
                RestartAuth = false;
                ButtonVisible = true;
            }
            else if(_selectedBackup != null && _selectedBackup.Etat == State.PAUSED)
            {
                StartAuth = false;
                StopAuth = false; 
                RestartAuth = true;
                ButtonVisible = false;
            }
            else
            {
                StartAuth = false;
                StopAuth = false; 
                RestartAuth = false;
            }
        }
    }

    private bool _startAuth;

    public bool StartAuth
    {
        get => _startAuth;
        set
        {
            _startAuth = value;
            OnPropertyChanged();
        }
    }
    
    private bool _stopAuth;

    public bool StopAuth
    {
        get => _stopAuth;
        set
        {
            _stopAuth = value;
            OnPropertyChanged();
        }
    }
    
    private bool _restartAuth;

    public bool RestartAuth
    {
        get => _restartAuth;
        set
        {
            _restartAuth = value;
            OnPropertyChanged();
        }
    }

    private string _welcomeText = "Connection";

    public string WelcomeText
    {
        get => _welcomeText;
        set
        {
            _welcomeText = value;
            OnPropertyChanged();
        }
    }

    private string _freezbeeText = "Freezbees";

    public string FreezbeeText
    {
        get => _freezbeeText;
        set
        {
            _freezbeeText = value;
            OnPropertyChanged();
        }
    }

    private string _ingredientText = "Ingrédients";

    public string IngredientText
    {
        get => _ingredientText;
        set
        {
            _ingredientText = value;
            OnPropertyChanged();
        }
    }
    
    private string _procedeText = "Procédés de fabrication";

    public string ProcedeText
    {
        get => _procedeText;
        set
        {
            _procedeText = value;
            OnPropertyChanged();
        }
    }
    
    private string _testText = "Tests";

    public string TestText
    {
        get => _testText;
        set
        {
            _testText = value;
            OnPropertyChanged();
        }
    }

    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private string _source;

    public string Source
    {
        get => _source;
        set
        {
            _source = value;
            OnPropertyChanged();
        }
    }

    private string _target;

    public string Target
    {
        get => _target;
        set
        {
            _target = value;
            OnPropertyChanged();
        }
    }

    private string _typeBackup;

    public string TypeBackup
    {
        get => _typeBackup;
        set
        {
            _typeBackup = value;
            OnPropertyChanged();
        }
    }

    private string _dataGridName = "Name";
    public string DataGridName
    {
        get => _dataGridName;
        set
        {
            _dataGridName = value;
            OnPropertyChanged();
        }
    }
    
    private string _dataGridSource = "Source";
    public string DataGridSource
    {
        get => _dataGridSource;
        set
        {
            _dataGridSource = value;
            OnPropertyChanged();
        }
    }
    private string _dataGridTarget = "Target";
    public string DataGridTarget
    {
        get => _dataGridTarget;
        set
        {
            _dataGridTarget = value;
            OnPropertyChanged();
        }
    }
    private string _dataGridType = "Type";
    public string DataGridType
    {
        get => _dataGridType;
        set
        {
            _dataGridType = value;
            OnPropertyChanged();
        }
    }
    private string _dataGridState = "State";
    public string DataGridState
    {
        get => _dataGridState;
        set
        {
            _dataGridState = value;
            OnPropertyChanged();
        }
    }
    private string _dataGridProgression = "Progress";
    public string DataGridProgression
    {
        get => _dataGridProgression;
        set
        {
            _dataGridProgression = value;
            OnPropertyChanged();
        }
    }
    public MainWindowViewModel()
    {
        
        ListTypeBackup = new List<string>()
        {
            "FullBackup", "DiffBackup"
        };

        OCollection = new ObservableCollection<ObservableObject>();
        Freezbee freezbee = new Freezbee(1, "Freezbee 1", "description", 5, "gamme");
        OCollection.Add(freezbee);
    }

    public List<string> ListTypeBackup { get; set; }
    
    
    public void modifyVisible() => Visible = !Visible;
    public void modifyButtonVisible() => ButtonVisible = !ButtonVisible;
    

    public void startBackup()
    {
        StartAuth = false;
        StopAuth = true;
        string s = "1&start&" + SelectedBackup.Name;
        // Client.packets.Enqueue(s);
        // Client.wait.Set();
    }

    public void stopBackup()
    {
        StopAuth = false;
        RestartAuth = true;
        modifyButtonVisible();
        string s = "1&stop&" + SelectedBackup.Name;
        // Client.packets.Enqueue(s);
        // Client.wait.Set();
    }
    public void restartBackup()
    {
        StopAuth = true;
        RestartAuth = false;
        modifyButtonVisible();
        string s = "1&restart&" + SelectedBackup.Name;
        // Client.packets.Enqueue(s);
        // Client.wait.Set();
    }
    
    public void leaveNewBackup()
    {
        Name = "";
        Source = "";
        Target = "";
        TypeBackup = "";
        modifyVisible();
    }
}