using System.Collections.ObjectModel;
using Avalonia.Markup.Xaml.MarkupExtensions;

namespace ViewAvalonia.ToolBox;

public class Freezbee : ObservableObject
{
    public Freezbee(int id, string name, string description, int puht, string gamme)
    {
        Id = id;
        Name = name;
        Desription = description;
        Puht = puht;
        Gamme = gamme;
    }
    
    private string _description;

    public string Desription
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }
    private int _puht;

    public int Puht
    {
        get => _puht;
        set
        {
            _puht = value;
            OnPropertyChanged();
        }
    }
    private string _gamme;

    public string Gamme
    {
        get => _gamme;
        set
        {
            _gamme = value;
            OnPropertyChanged();
        }
    }

}