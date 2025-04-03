using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BEE;
using Client;
using Google.Protobuf;
using MSLib.Proto;
using MSTest.Proto;
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
        ListVisible = true;
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
    
    
    private ObservableCollection<ObservableObject> _collection = new();
    
    public ObservableCollection<ObservableObject> OCollection
    {
        get => _collection;
        set
        {
            _collection = value;
            OnPropertyChanged();
        }
    }

    private ObservableObject _selectedObject;

    public ObservableObject SelectedObject
    {
        get => _selectedObject;
        set
        {
            _selectedObject = value;
            OnPropertyChanged();
        }
    }

    public void SeeMore()
    {
        ListVisible = !ListVisible;
        DetailVisible = true;
    }

    public void Back()
    {
        ListVisible = !ListVisible;
        DetailVisible = !DetailVisible;
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
    
    private string _dataGridId = "ID";
    public string DataGridId
    {
        get => _dataGridId;
        set
        {
            _dataGridId = value;
            OnPropertyChanged();
        }
    }
    public MainWindowViewModel()
    {

        OCollection = new ObservableCollection<ObservableObject>();
    }

    private bool _listVisible = false;

    public bool ListVisible
    {
        get => _listVisible;
        set
        {
            _listVisible = value;
            OnPropertyChanged();
        }
    }
    
    private bool _detailVisible = false;

    public bool DetailVisible
    {
        get => _detailVisible;
        set
        {
            _detailVisible = value;
            OnPropertyChanged();
        }
    }

    public async void LoadFreezbee()
    {
        Packet packet =ProxyClient.PreparePacket("Freezbee", "GetFreezbee", "Freezbee", new Freezbee());
        SPPacket spPacket = ProxyClient.PrepareSPPacket("TEST", packet, Login, Password);
        byte[]? resp = await ProxyClient.SendPacket(spPacket.ToByteArray());
        if (resp != null)
        {
            Response r = Response.Parser.ParseFrom(resp);

            if (r.StatusCode == 200 && r.BodyType == "Freezbee")
            {
                OCollection = new ObservableCollection<ObservableObject>();
                foreach (ByteString b in r.Body)
                {
                    Freezbee f = Freezbee.Parser.ParseFrom(b);
                    OFreezbee o = new OFreezbee(f.IdModele, f.NameModele, "", 0, "");
                    OCollection.Add(o);
                }
            }
        }
    }
}