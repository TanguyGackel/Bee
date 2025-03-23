using System.Data;
using Microsoft.Data.SqlClient;
using MS_Lib;

namespace MS_RD.Models;

internal class Ingredient
{
    private static readonly DatabaseConnector DbConnector = DatabaseConnector.Instance;

    internal Ingredient()
    {
        Nom = "";
        Description = "";
    }

    internal int Id;
    internal string Nom;
    internal string Description;
    internal int Grammage;
    
    #region Get
    internal static List<Ingredient> GetIngredients()
    {
        SqlCommand cmd = new SqlCommand("get_ingredients");
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader result = DbConnector.SendQueryRequest(cmd);

        List<Ingredient> toReturn = new List<Ingredient>();
        while (result.Read())
        {
            Ingredient i = new Ingredient()
            {
                Id = (int)result["id"],
                Nom = (string)result["nom"],
            };
            toReturn.Add(i);

        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
    internal static Ingredient? GetIngredientById(int idIngredient)
    {
        if (idIngredient < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idIngredient), (int)idIngredient, "idIngredient should be >= 0");
        }
        
        SqlCommand cmd = new SqlCommand("get_modele_by_id");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", idIngredient);
        cmd.Parameters["@id"].Direction = ParameterDirection.Input;

        SqlDataReader result = DbConnector.SendQueryRequest(cmd);

        Ingredient? toReturn = null;

        while (result.Read())
        {
            toReturn = new Ingredient()
            {
                Nom = (string)result["nom"],
                Description = (string)result["description"],
            };
        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static List<Ingredient> GetIngredientByName(string nameIngredient)
    {
        SqlCommand cmd = new SqlCommand("get_modeles_by_name");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@nom", nameIngredient);
        cmd.Parameters["@nom"].Direction = ParameterDirection.Input;

        SqlDataReader result = DbConnector.SendQueryRequest(cmd);

        List<Ingredient> toReturn = new List<Ingredient>();

        while (result.Read())
        {
            Ingredient i = new Ingredient()
            {
                Id = (int)result["id"],
                Nom = (string)result["nom"]
            };
            toReturn.Add(i);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
    internal static List<Freezbee> GetFreezbeesFromIngredient(int idIngredient)
    {
        SqlCommand cmd = new SqlCommand("get_ingredient_modeles");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_ingredient", idIngredient);
        cmd.Parameters["@id_ingredient"].Direction = ParameterDirection.Input;

        SqlDataReader result = DbConnector.SendQueryRequest(cmd);
        
        List<Freezbee> toReturn = new List<Freezbee>();

        while (result.Read())
        {
            Freezbee i = new Freezbee()
            {
                Id = (int)result["id"],
                Nom = (string)result["nom"],
            };
              
            toReturn.Add(i);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    #endregion
    
    #region Add
    internal static void AddIngredient(string nom, string description)
    {
        SqlCommand cmd = new SqlCommand("add_ingredient");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@nom", nom);
        cmd.Parameters["@nom"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@description", description);
        cmd.Parameters["@description"].Direction = ParameterDirection.Input;
        
        DbConnector.SendNonQueryRequest(cmd);
    }
    #endregion
    
    #region Delete
    internal static void DeleteIngredient(int id)
    {
        if (id < 0)
            throw new ArgumentOutOfRangeException(nameof(id), id, "id should be >= 0");
        
        SqlCommand cmd = new SqlCommand("delete_ingredient");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters["@id"].Direction = ParameterDirection.Input;

        DbConnector.SendNonQueryRequest(cmd);
    }
    #endregion
    
    #region Update
    internal static void UpdateModele(int id, string nom, string description)
    {
        if (id < 0)
            throw new ArgumentOutOfRangeException(nameof(id), id, "id should be >= 0");
        
        SqlCommand cmd = new SqlCommand("update_ingredient");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters["@id"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@nom", nom);
        cmd.Parameters["@nom"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@description", description);
        cmd.Parameters["@description"].Direction = ParameterDirection.Input;
        
        DbConnector.SendNonQueryRequest(cmd);
    }
    #endregion
}