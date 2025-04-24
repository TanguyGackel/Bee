using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BEE;
using Client;
using Google.Protobuf;
using MSLib.Proto;
using MSRD.Proto;
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
        SelectedTab = 2;
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

    private int _selectedTab;

    public int SelectedTab
    {
        get => _selectedTab;
        set
        {
            _selectedTab = value;
            OnPropertyChanged();
        }
    }

    private int tempTab = 0;
    
    public void SeeMore()
    {
        ListVisible = !ListVisible;
        DetailVisible = true;
        tempTab = SelectedTab;
        SelectedTab = 1;
    }

    public void Back()
    {
        ListVisible = !ListVisible;
        DetailVisible = !DetailVisible;
        SelectedTab = tempTab;
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
        Console.WriteLine("Yo we need to load the freezbees");
        
        Packet packet =ProxyClient.PreparePacket("Freezbee", "GetFreezbee", "Freezbee", new Freezbee());
        SPPacket spPacket = ProxyClient.PrepareSPPacket("TEST", packet, Login, Password);
        
        Console.WriteLine("sppacket : " + spPacket.ToString());

        Console.WriteLine("Loading");

        byte[]? resp = await ProxyClient.SendPacket(spPacket.ToByteArray());
        if (resp != null)
        {
            Response r = Response.Parser.ParseFrom(resp);
            
            Console.WriteLine("Trying to parse the response");
            Console.WriteLine("resp : " + Convert.ToBase64String(resp));

            if (r.StatusCode == 200 && r.BodyType == "Freezbee")
            {
                
                Console.WriteLine("Everything's good");

                OCollection = new ObservableCollection<ObservableObject>();
                foreach (ByteString b in r.Body)
                {
                    Freezbee f = Freezbee.Parser.ParseFrom(b);
                    OFreezbee o = new OFreezbee(f.IdModele, f.NameModele, "", 0, "");
                    OCollection.Add(o);
                }
            }
        }
        else
        {
            Console.WriteLine("Why are you null ?");
        }
    }
    
    public async void LoadIngredient()
    {
        Packet packet =ProxyClient.PreparePacket("Ingredient", "GetIngredients", "Ingredient", new Ingredient());
        SPPacket spPacket = ProxyClient.PrepareSPPacket("RD", packet, Login, Password);
        byte[]? resp = await ProxyClient.SendPacket(spPacket.ToByteArray());
        if (resp != null)
        {
            Response r = Response.Parser.ParseFrom(resp);

            if (r.StatusCode == 200 && r.BodyType == "Ingredient")
            {
                OCollection = new ObservableCollection<ObservableObject>();
                foreach (ByteString b in r.Body)
                {
                    Ingredient f = Ingredient.Parser.ParseFrom(b);
                    OIngredient o = new OIngredient(f.Id, f.Name);
                    OCollection.Add(o);
                }
            }
        }
    }
    public async void LoadTest()
    {
        Packet packet =ProxyClient.PreparePacket("Test", "GetTests", "Test", new Ingredient());
        SPPacket spPacket = ProxyClient.PrepareSPPacket("TEST", packet, Login, Password);
        byte[]? resp = await ProxyClient.SendPacket(spPacket.ToByteArray());
        if (resp != null)
        {
            Response r = Response.Parser.ParseFrom(resp);

            if (r.StatusCode == 200 && r.BodyType == "Test")
            {
                OCollection = new ObservableCollection<ObservableObject>();
                foreach (ByteString b in r.Body)
                {
                    Test t = Test.Parser.ParseFrom(b);
                    OTest o = new OTest(t.IdTest, t.NameTest);
                    OCollection.Add(o);
                }
            }
        }
    }

    public async void LoadProcede()
    {
        Packet packet =ProxyClient.PreparePacket("ProcedeFabrication", "GetProcedeFabrications", "ProcedeFabrication", new ProcedeFabrication());
        SPPacket spPacket = ProxyClient.PrepareSPPacket("RD", packet, Login, Password);
        byte[]? resp = await ProxyClient.SendPacket(spPacket.ToByteArray());
        if (resp != null)
        {
            Response r = Response.Parser.ParseFrom(resp);

            if (r.StatusCode == 200 && r.BodyType == "ProcedeFabrication")
            {
                OCollection = new ObservableCollection<ObservableObject>();
                foreach (ByteString b in r.Body)
                {
                    ProcedeFabrication t = ProcedeFabrication.Parser.ParseFrom(b);
                    OProcedeFabrication o = new OProcedeFabrication(t.Id, t.Name);
                    OCollection.Add(o);
                }
            }
        }
    }
}