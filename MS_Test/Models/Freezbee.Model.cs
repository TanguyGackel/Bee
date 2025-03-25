using System.Data;
using System.Data.Common;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using MS_Lib;

namespace MS_Test.Models;

internal class FreezbeeModel
{
    private FreezbeeModel()
    {
        name = "";
        description = "";
        gamme = "";
    }
    
    private static readonly DatabaseConnector dbConnector = DatabaseConnector.Instance;
    
    internal int id;
    internal string name;
    internal string description;
    internal int UHT_price;
    internal string gamme;
    
    internal static List<FreezbeeModel> GetFreezbee()
    {
        SqlCommand cmd = new SqlCommand("get_modeles");
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader result = dbConnector.SendQueryRequest(cmd);

        List<FreezbeeModel> toReturn = new List<FreezbeeModel>();
        while (result.Read())
        {
            FreezbeeModel f = new FreezbeeModel()
            {
                id = (int)result["id"],
                name = (string)result["nom"],
            };
            toReturn.Add(f);

        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static FreezbeeModel? GetFreezbeeById(int idModele)
    {
        if (idModele < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idModele), (int)idModele, "idModel should be >= 0");
        }
        
        SqlCommand cmd = new SqlCommand("get_modele_by_id");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", idModele);
        cmd.Parameters["@id"].Direction = ParameterDirection.Input;

        SqlDataReader result = dbConnector.SendQueryRequest(cmd);

        FreezbeeModel? toReturn = null;

        while (result.Read())
        {
            toReturn = new FreezbeeModel()
            {
                name = (string)result["nom"],
                description = (string)result["description"],
                UHT_price = (int)result["pUHT"],
                gamme = (string)result["gamme"]
            };
        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static List<FreezbeeModel> GetFreezbeeByName(string nameModele)
    {
        SqlCommand cmd = new SqlCommand("get_modeles_by_name");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@nom", nameModele);
        cmd.Parameters["@nom"].Direction = ParameterDirection.Input;

        SqlDataReader result = dbConnector.SendQueryRequest(cmd);

        List<FreezbeeModel> toReturn = new List<FreezbeeModel>();

        while (result.Read())
        {
            FreezbeeModel f = new FreezbeeModel()
            {
                id = (int)result["id"],
                name = (string)result["nom"]
            };
            toReturn.Add(f);
        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static List<FreezbeeModel> GetFreezbeeByGamme(string gammeModele)
    {
        SqlCommand cmd = new SqlCommand("get_modeles_by_gamme");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@gamme", gammeModele);
        cmd.Parameters["@gamme"].Direction = ParameterDirection.Input;

        SqlDataReader result = dbConnector.SendQueryRequest(cmd);

        List<FreezbeeModel> toReturn = new List<FreezbeeModel>();

        while (result.Read())
        {
            FreezbeeModel f = new FreezbeeModel()
            {
                id = (int)result["id"],
                name = (string)result["nom"],
                gamme = (string)result["gamme"]
            };
            toReturn.Add(f);
        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static List<TestModel> GetFreezbeeTestById(int idModele)
    {
        if (idModele < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idModele), (int)idModele, "idModel should be >= 0");
        }
        
        SqlCommand cmd = new SqlCommand("get_modele_test");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@id_modele"].Direction = ParameterDirection.Input;

        SqlDataReader result = dbConnector.SendQueryRequest(cmd);

        List<TestModel> toReturn = new List<TestModel>();

        while (result.Read())
        {
            TestModel t = new TestModel()
            {
                id = (int)result["id"],
                name = (string)result["nom"],
                description = (string)result["description"],
                type = (string)result["type"]
            };
            
            toReturn.Add(t);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
    
    
}