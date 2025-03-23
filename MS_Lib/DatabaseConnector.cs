using Microsoft.Data.SqlClient;

namespace MS_Lib;

public sealed class DatabaseConnector
{
    private DatabaseConnector()
    {
    }

    private static DatabaseConnector? _instance;
    public static DatabaseConnector Instance => _instance ??= new DatabaseConnector();

    private string _connString = "";

    public void ConstructConnectionString()
    {
        _connString = _source + _db + _credentials + _encryption;
        SqlConnection temp = new SqlConnection(_connString);
        temp.Open();
        temp.Close();
    }

    public string GetConnectionString()
    {
        return _connString;
    }
    
    private string _source = "";
    public string Source
    {
        set => _source = "Data Source=(" + value + ");";
    }

    private string _db = "";
    public string DB
    {
        set => _db = "Initial Catalog=" + value + ";";
    }

    private string _credentials = "Integrated Security=false;";

    public string Credentials
    {
        set
        {
            switch (Type)
            {
                case ConnectionType.WindowsAuthentication:
                    _credentials = "Integrated Security=SSPI;";
                    break;
                case ConnectionType.Password:
                {
                    string[] temp = value.Split(',');
                    _credentials = "Integrated Security=false; ID=" + temp[0] + "; Password=" + temp[1] + ";";
                    break;
                }
            }
        }
    }

    private string _encryption = "Encrypt=True";

    public bool Encryption
    {
        set => _encryption = "Encrypt=" + (value ? "True" : "False") + ";";
    }
    
    public ConnectionType Type = ConnectionType.Password;

    public SqlDataReader SendQueryRequest(SqlCommand cmd)
    {
        try
        {
            SqlConnection conn = new SqlConnection(_connString);
            conn.Open();
            cmd.Connection = conn;
            SqlDataReader result = cmd.ExecuteReader();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void SendNonQueryRequest(SqlCommand cmd)
    {
        try
        {
            using SqlConnection conn = new SqlConnection(_connString);
            conn.Open();

            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

public enum ConnectionType
{
    WindowsAuthentication,
    Password
}