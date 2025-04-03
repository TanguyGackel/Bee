namespace ViewAvalonia.ToolBox;

public class ObservableObject : ViewModel
{
    public string type;
    
    private int _id;

    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged();
        }
    }
    private string _name = "";

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }
}