using System.Data;
using Microsoft.Data.SqlClient;
using MS_Lib;
using MSTest.Proto;


namespace MS_Factory.Models;

internal static class FreezbeeModel
{
    private static readonly DatabaseConnector DbConnector = DatabaseConnector.Instance;
    
    #region Get
    internal static async Task<List<Freezbee>> GetFreezbee()
    {
        SqlCommand cmd = new SqlCommand("get_modeles");
        cmd.CommandType = CommandType.StoredProcedure;
        
        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);

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
            throw new ArgumentOutOfRangeException(nameof(idModele), (int)idModele, "idModele should be >= 0");
        }
        
        SqlCommand cmd = new SqlCommand("get_modele_by_id");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", idModele);
        cmd.Parameters["@id"].Direction = ParameterDirection.Input;

        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);

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

        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);

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

        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);

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

    internal static async Task<List<IngredientFreezbee>> GetIngredientsFromFreezbee(int idModele)
    {
        SqlCommand cmd = new SqlCommand("get_modeles_ingredients");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@id_modele"].Direction = ParameterDirection.Input;

        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);
        
        List<IngredientFreezbee> toReturn = new List<IngredientFreezbee>();

        while (result.Read())
        {
            IngredientFreezbee i = new IngredientFreezbee()
            {
                Id = (int)result["id"],
                Name = (string)result["nom"],
                Grammage = (int)result["grammage"]
            };
              
            toReturn.Add(i);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
    internal static async Task<List<CaracteristiqueFreezbee>> GetCaracteristiquesFromFreezbee(int idModele)
    {
        SqlCommand cmd = new SqlCommand("get_modeles_caracteristiques");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@id_modele"].Direction = ParameterDirection.Input;

        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);
        
        List<CaracteristiqueFreezbee> toReturn = new List<CaracteristiqueFreezbee>();

        while (result.Read())
        {
            CaracteristiqueFreezbee c = new CaracteristiqueFreezbee()
            {
                Id = (int)result["id"],
                Name = (string)result["nom"]
            };
            toReturn.Add(c);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
    internal static async Task<List<ProcedeFabricationFreezbee>> GetProcedeFabricationsFromFreezbee(int idModele)
    {
        SqlCommand cmd = new SqlCommand("get_modeles_procedeFabrications");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@id_modele"].Direction = ParameterDirection.Input;

        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);
        
        List<ProcedeFabricationFreezbee> toReturn = new List<ProcedeFabricationFreezbee>();

        while (result.Read())
        {
            ProcedeFabricationFreezbee pf = new ProcedeFabricationFreezbee()
            {
                Id = (int)result["id"],
                Name = (string)result["nom"]
            };
            toReturn.Add(pf);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    #endregion