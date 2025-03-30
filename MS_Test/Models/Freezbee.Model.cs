using System.Data;
using Microsoft.Data.SqlClient;
using MS_Lib;
using MSTest.Proto;

namespace MS_Test.Models;

internal static class FreezbeeModel
{
    
    private static readonly DatabaseConnector dbConnector = DatabaseConnector.Instance;
    
    internal static async Task<List<Freezbee>> GetFreezbee()
    {
        SqlCommand cmd = new SqlCommand("get_modeles");
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader result = await dbConnector.SendQueryRequest(cmd);

        List<Freezbee> toReturn = new List<Freezbee>();
        while (result.Read())
        {
            Freezbee f = new Freezbee()
            {
                IdModele = (int)result["id"],
                NameModele = (string)result["nom"],
            };
            toReturn.Add(f);

        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static async Task<Freezbee?> GetFreezbeeById(int idModele)
    {
        if (idModele < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idModele), (int)idModele, "idModel should be >= 0");
        }
        
        SqlCommand cmd = new SqlCommand("get_modele_by_id");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", idModele);
        cmd.Parameters["@id"].Direction = ParameterDirection.Input;

        SqlDataReader result = await dbConnector.SendQueryRequest(cmd);

        Freezbee? toReturn = null;

        while (result.Read())
        {
            toReturn = new Freezbee()
            {
                NameModele = (string)result["nom"],
                Description = (string)result["description"],
                PUHT = (int)result["pUHT"],
                GammeModele = (string)result["gamme"]
            };
        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static async Task<List<Freezbee>> GetFreezbeeByName(string nameModele)
    {
        SqlCommand cmd = new SqlCommand("get_modeles_by_name");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@nom", nameModele);
        cmd.Parameters["@nom"].Direction = ParameterDirection.Input;

        SqlDataReader result = await dbConnector.SendQueryRequest(cmd);

        List<Freezbee> toReturn = new List<Freezbee>();

        while (result.Read())
        {
            Freezbee f = new Freezbee()
            {
                IdModele = (int)result["id"],
                NameModele = (string)result["nom"]
            };
            toReturn.Add(f);
        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static async Task<List<Freezbee>> GetFreezbeeByGamme(string gammeModele)
    {
        SqlCommand cmd = new SqlCommand("get_modeles_by_gamme");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@gamme", gammeModele);
        cmd.Parameters["@gamme"].Direction = ParameterDirection.Input;

        SqlDataReader result = await dbConnector.SendQueryRequest(cmd);

        List<Freezbee> toReturn = new List<Freezbee>();

        while (result.Read())
        {
            Freezbee f = new Freezbee()
            {
                IdModele = (int)result["id"],
                NameModele = (string)result["nom"],
                GammeModele = (string)result["gamme"]
            };
            toReturn.Add(f);
        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static async Task<List<TestFreezbee>> GetFreezbeeTestById(int idModele)
    {
        if (idModele < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idModele), (int)idModele, "idModel should be >= 0");
        }
        
        SqlCommand cmd = new SqlCommand("get_modele_test");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@id_modele"].Direction = ParameterDirection.Input;

        SqlDataReader result = await dbConnector.SendQueryRequest(cmd);

        List<TestFreezbee> toReturn = new List<TestFreezbee>();

        while (result.Read())
        {
            TestFreezbee t = new TestFreezbee()
            {
                Id = (int)result["id"],
                Name = (string)result["nom"],
                Description = (string)result["description"],
                Type = (string)result["type"]
            };
            
            toReturn.Add(t);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
    
    
}