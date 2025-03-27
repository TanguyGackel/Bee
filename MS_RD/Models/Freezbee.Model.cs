using System.Data;
using Microsoft.Data.SqlClient;
using MS_Lib;

namespace MS_RD.Models;

internal class FreezbeeModel
{
    private static readonly DatabaseConnector DbConnector = DatabaseConnector.Instance;
    
    internal FreezbeeModel()
    {
        Nom = "";
        Description = "";
        Gamme = "";
    }
    
    internal int Id;
    internal string Nom;
    internal string Description;
    internal int Puht;
    internal string Gamme;

    #region Get
    internal static async Task<List<FreezbeeModel>> GetFreezbee()
    {
        SqlCommand cmd = new SqlCommand("get_modeles");
        cmd.CommandType = CommandType.StoredProcedure;
        
        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);

        List<FreezbeeModel> toReturn = new List<FreezbeeModel>();
        while (result.Read())
        {
            FreezbeeModel f = new FreezbeeModel()
            {
                Id = (int)result["id"],
                Nom = (string)result["nom"],
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
            throw new ArgumentOutOfRangeException(nameof(idModele), (int)idModele, "idModele should be >= 0");
        }
        
        SqlCommand cmd = new SqlCommand("get_modele_by_id");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", idModele);
        cmd.Parameters["@id"].Direction = ParameterDirection.Input;

        SqlDataReader result = DbConnector.SendQueryRequest(cmd);

        FreezbeeModel? toReturn = null;

        while (result.Read())
        {
            toReturn = new FreezbeeModel()
            {
                Nom = (string)result["nom"],
                Description = (string)result["description"],
                Puht = (int)result["pUHT"],
                Gamme = (string)result["gamme"]
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

        SqlDataReader result = DbConnector.SendQueryRequest(cmd);

        List<FreezbeeModel> toReturn = new List<FreezbeeModel>();

        while (result.Read())
        {
            FreezbeeModel f = new FreezbeeModel()
            {
                Id = (int)result["id"],
                Nom = (string)result["nom"]
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

        SqlDataReader result = DbConnector.SendQueryRequest(cmd);

        List<FreezbeeModel> toReturn = new List<FreezbeeModel>();

        while (result.Read())
        {
            FreezbeeModel f = new FreezbeeModel()
            {
                Id = (int)result["id"],
                Nom = (string)result["nom"],
                Gamme = (string)result["gamme"]
            };
            toReturn.Add(f);
        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static List<IngredientModel> GetIngredientsFromFreezbee(int idModele)
    {
        SqlCommand cmd = new SqlCommand("get_modeles_ingredients");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@id_modele"].Direction = ParameterDirection.Input;

        SqlDataReader result = DbConnector.SendQueryRequest(cmd);
        
        List<IngredientModel> toReturn = new List<IngredientModel>();

        while (result.Read())
        {
            IngredientModel i = new IngredientModel()
            {
                Id = (int)result["id"],
                Nom = (string)result["nom"],
                Grammage = (int)result["grammage"]
            };
              
            toReturn.Add(i);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
    internal static List<CaracteristiqueModel> GetCaracteristiquesFromFreezbee(int idModele)
    {
        SqlCommand cmd = new SqlCommand("get_modeles_caracteristiques");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@id_modele"].Direction = ParameterDirection.Input;

        SqlDataReader result = DbConnector.SendQueryRequest(cmd);
        
        List<CaracteristiqueModel> toReturn = new List<CaracteristiqueModel>();

        while (result.Read())
        {
            CaracteristiqueModel c = new CaracteristiqueModel()
            {
                Id = (int)result["id"],
                Nom = (string)result["nom"]
            };
            toReturn.Add(c);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
    internal static List<ProcedeFabricationModel> GetProcedeFabricationsFromFreezbee(int idModele)
    {
        SqlCommand cmd = new SqlCommand("get_modeles_procedeFabrications");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@id_modele"].Direction = ParameterDirection.Input;

        SqlDataReader result = DbConnector.SendQueryRequest(cmd);
        
        List<ProcedeFabricationModel> toReturn = new List<ProcedeFabricationModel>();

        while (result.Read())
        {
            ProcedeFabricationModel pf = new ProcedeFabricationModel()
            {
                Id = (int)result["id"],
                Nom = (string)result["nom"]
            };
            toReturn.Add(pf);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    #endregion
    
    #region Add
    internal static void AddFreezbee(string nom, string description, int pUht, string gamme)
    {
        SqlCommand cmd = new SqlCommand("add_modele");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@nom", nom);
        cmd.Parameters["@nom"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@description", description);
        cmd.Parameters["@description"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@pUHT", pUht);
        cmd.Parameters["@pUHT"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@gamme", gamme);
        cmd.Parameters["@gamme"].Direction = ParameterDirection.Input;
        
        DbConnector.SendNonQueryRequest(cmd);
    }

    internal static void AddIngredientToFreezbee(int idModele, int idIngredient, int grammage)
    {
        if (idModele < 0)
            throw new ArgumentOutOfRangeException(nameof(idModele), idModele, "idModele should be >= 0");
        if (idIngredient < 0)
            throw new ArgumentOutOfRangeException(nameof(idIngredient), idIngredient, "idIngredient should be >= 0");
        if (grammage <= 0)
            throw new ArgumentOutOfRangeException(nameof(grammage), grammage, "grammage should be > 0");

        SqlCommand cmd = new SqlCommand("add_ingredient_to_modele");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@id_modele"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@id_ingredient", idIngredient);
        cmd.Parameters["@id_ingredient"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@grammage", grammage);
        cmd.Parameters["@grammage"].Direction = ParameterDirection.Input;
        
        DbConnector.SendNonQueryRequest(cmd);
    }
    
    internal static void AddCaracteristiqueToFreezbee(int idModele, int idCaracteristique)
    {
        if (idModele < 0)
            throw new ArgumentOutOfRangeException(nameof(idModele), idModele, "idModele should be >= 0");
        if (idCaracteristique < 0)
            throw new ArgumentOutOfRangeException(nameof(idCaracteristique), idCaracteristique, "idCaracteristique should be >= 0");

        SqlCommand cmd = new SqlCommand("add_caracteristique_to_modele");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@id_modele"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@id_caracteristique", idCaracteristique);
        cmd.Parameters["@id_ingredient"].Direction = ParameterDirection.Input;

        DbConnector.SendNonQueryRequest(cmd);
    }
    #endregion
    
    #region Delete
    internal static void DeleteModele(int id)
    {
        if (id < 0)
            throw new ArgumentOutOfRangeException(nameof(id), id, "id should be >= 0");
        
        SqlCommand cmd = new SqlCommand("delete_modele");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters["@id"].Direction = ParameterDirection.Input;

        DbConnector.SendNonQueryRequest(cmd);
    }
    
    internal static void DeleteIngredientFromFreezbee(int idModele, int idIngredient)
    {
        if (idModele < 0)
            throw new ArgumentOutOfRangeException(nameof(idModele), idModele, "idModele should be >= 0");
        if (idIngredient < 0)
            throw new ArgumentOutOfRangeException(nameof(idIngredient), idIngredient, "idIngredient should be >= 0");

        SqlCommand cmd = new SqlCommand("delete_ingredient_from_modele");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@id_modele"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@id_ingredient", idIngredient);
        cmd.Parameters["@id_ingredient"].Direction = ParameterDirection.Input;
        
        DbConnector.SendNonQueryRequest(cmd);
    }
    
    internal static void DeleteCaracteristiqueFromFreezbee(int idModele, int idCaracteristique)
    {
        if (idModele < 0)
            throw new ArgumentOutOfRangeException(nameof(idModele), idModele, "idModele should be >= 0");
        if (idCaracteristique < 0)
            throw new ArgumentOutOfRangeException(nameof(idCaracteristique), idCaracteristique, "idCaracteristique should be >= 0");

        SqlCommand cmd = new SqlCommand("delete_caracteristique_from_freezbee");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@id_modele"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@id_caracteristique", idCaracteristique);
        cmd.Parameters["@id_ingredient"].Direction = ParameterDirection.Input;

        DbConnector.SendNonQueryRequest(cmd);
    }
    #endregion
    
    #region Update
    internal static void UpdateModele(int id, string nom, string description, int pUht, string gamme)
    {
        if (id < 0)
            throw new ArgumentOutOfRangeException(nameof(id), id, "id should be >= 0");
        
        SqlCommand cmd = new SqlCommand("update_modele");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters["@id"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@nom", nom);
        cmd.Parameters["@nom"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@description", description);
        cmd.Parameters["@description"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@pUHT", pUht);
        cmd.Parameters["@pUHT"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@gamme", gamme);
        cmd.Parameters["@gamme"].Direction = ParameterDirection.Input;
        
        DbConnector.SendNonQueryRequest(cmd);
    }
    #endregion
}