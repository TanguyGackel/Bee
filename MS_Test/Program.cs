using MS_Lib;

internal class Program  
{
    static void Main(string[] args)
    {
        DatabaseConnector db = DatabaseConnector.Instance;
        db.Type = ConnectionType.WindowsAuthentication;
        db.Credentials = "";
        db.Source = "local";
        db.DB = "BeeDB";
        db.Encryption = false;
        db.ConstructConnectionString();

    }
}