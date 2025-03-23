using System.Data;
using System.Data.Common;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using MS_Lib;

namespace MS_Test.Models;

internal class Freezbee
{
    private Freezbee()
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
    
    internal static List<Freezbee> GetFreezbee()
    {
        SqlCommand cmd = new SqlCommand("get_modeles");
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader result = dbConnector.SendQueryRequest(cmd);

        List<Freezbee> toReturn = new List<Freezbee>();
        while (result.Read())
        {
            Freezbee f = new Freezbee()
            {
                id = (int)result["id"],
                name = (string)result["nom"],
            };
            toReturn.Add(f);

        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static Freezbee? GetFreezbeeById(int idModel)
    {
        if (idModel < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idModel), (int)idModel, "idModel should be >= 0");
        }
        
        SqlCommand cmd = new SqlCommand("get_modele_by_id");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", idModel);
        cmd.Parameters["@id"].Direction = ParameterDirection.Input;

        SqlDataReader result = dbConnector.SendQueryRequest(cmd);

        Freezbee? toReturn = null;

        while (result.Read())
        {
            toReturn = new Freezbee()
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

    internal static List<Freezbee> GetFreezbeeByName(string nameModel)
    {
        SqlCommand cmd = new SqlCommand("get_modeles_by_name");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@nom", nameModel);
        cmd.Parameters["@nom"].Direction = ParameterDirection.Input;

        SqlDataReader result = dbConnector.SendQueryRequest(cmd);

        List<Freezbee> toReturn = new List<Freezbee>();

        while (result.Read())
        {
            Freezbee f = new Freezbee()
            {
                id = (int)result["id"],
                name = (string)result["nom"]
            };
            toReturn.Add(f);
        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static List<Freezbee> GetFreezbeeByGamme(string gammeModel)
    {
        SqlCommand cmd = new SqlCommand("get_modeles_by_gamme");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@gamme", gammeModel);
        cmd.Parameters["@gamme"].Direction = ParameterDirection.Input;

        SqlDataReader result = dbConnector.SendQueryRequest(cmd);

        List<Freezbee> toReturn = new List<Freezbee>();

        while (result.Read())
        {
            Freezbee f = new Freezbee()
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

    internal static List<Test> GetFreezbeeTestById(int idModele)
    {
        SqlCommand cmd = new SqlCommand("get_modele_test");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@id_modele"].Direction = ParameterDirection.Input;

        SqlDataReader result = dbConnector.SendQueryRequest(cmd);

        List<Test> toReturn = new List<Test>();

        while (result.Read())
        {
            Test t = new Test()
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