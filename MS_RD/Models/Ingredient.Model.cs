using System.Data;
using Microsoft.Data.SqlClient;
using MS_Lib;
using MSRD.Proto;

namespace MS_RD.Models;

internal static class IngredientModel
{
    private static readonly DatabaseConnector DbConnector = DatabaseConnector.Instance;
    
    #region Get
    internal static async Task<List<Ingredient>> GetIngredients()
    {
        SqlCommand cmd = new SqlCommand("get_ingredients");
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);

        List<Ingredient> toReturn = new List<Ingredient>();
        while (result.Read())
        {
            Ingredient i = new Ingredient()
            {
                Id = (int)result["id"],
                Name = (string)result["nom"],
            };
            toReturn.Add(i);

        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
    internal static async Task<Ingredient>? GetIngredientById(int idIngredient)
    {
        if (idIngredient < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idIngredient), (int)idIngredient, "idIngredient should be >= 0");
        }
        
        SqlCommand cmd = new SqlCommand("get_modele_by_id");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", idIngredient);
        cmd.Parameters["@id"].Direction = ParameterDirection.Input;

        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);

        Ingredient? toReturn = null;

        while (result.Read())
        {
            toReturn = new Ingredient()
            {
                Name = (string)result["nom"],
                Description = (string)result["description"],
            };
        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static async Task<List<Ingredient>> GetIngredientByName(string nameIngredient)
    {
        SqlCommand cmd = new SqlCommand("get_modeles_by_name");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@nom", nameIngredient);
        cmd.Parameters["@nom"].Direction = ParameterDirection.Input;

        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);

        List<Ingredient> toReturn = new List<Ingredient>();

        while (result.Read())
        {
            Ingredient i = new Ingredient()
            {
                Id = (int)result["id"],
                Name = (string)result["nom"]
            };
            toReturn.Add(i);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
    internal static async Task<List<FreezbeeI>> GetFreezbeesFromIngredient(int idIngredient)
    {
        SqlCommand cmd = new SqlCommand("get_ingredient_modeles");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_ingredient", idIngredient);
        cmd.Parameters["@id_ingredient"].Direction = ParameterDirection.Input;

        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);
        
        List<FreezbeeI> toReturn = new List<FreezbeeI>();

        while (result.Read())
        {
            FreezbeeI i = new FreezbeeI()
            {
                Id = (int)result["id"],
                Name = (string)result["nom"],
            };
              
            toReturn.Add(i);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    #endregion
    
    #region Add
    internal static async Task<int> AddIngredient(string nom, string description)
    {
        SqlCommand cmd = new SqlCommand("add_ingredient");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@nom", nom);
        cmd.Parameters["@nom"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@description", description);
        cmd.Parameters["@description"].Direction = ParameterDirection.Input;
        
        return await DbConnector.SendNonQueryRequest(cmd);
    }
    #endregion
    
    #region Delete
    internal static async Task<int> DeleteIngredient(int id)
    {
        if (id < 0)
            throw new ArgumentOutOfRangeException(nameof(id), id, "id should be >= 0");
        
        SqlCommand cmd = new SqlCommand("delete_ingredient");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters["@id"].Direction = ParameterDirection.Input;

        return await DbConnector.SendNonQueryRequest(cmd);
    }
    #endregion
    
    #region Update
    internal static async Task<int> UpdateModele(int id, string nom, string description)
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
        
        return await DbConnector.SendNonQueryRequest(cmd);
    }
    #endregion
}