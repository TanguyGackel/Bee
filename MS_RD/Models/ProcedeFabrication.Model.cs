using System.Data;
using Microsoft.Data.SqlClient;
using MS_Lib;

namespace MS_RD.Models;

internal static class ProcedeFabricationModel
{
    private static readonly DatabaseConnector DbConnector = DatabaseConnector.Instance;
    
    #region Get
    internal static List<ProcedeFabricationModel> GetProcedeFabrications()
    {
        SqlCommand cmd = new SqlCommand("get_procedeFabrications");
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader result = DbConnector.SendQueryRequest(cmd);

        List<ProcedeFabricationModel> toReturn = new List<ProcedeFabricationModel>();
        while (result.Read())
        {
            ProcedeFabricationModel pf = new ProcedeFabricationModel()
            {
                Id = (int)result["id"],
                Nom = (string)result["nom"],
            };
            toReturn.Add(pf);

        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static ProcedeFabricationModel? GetProcedeFabricationById(int idProcedeFabrication)
    {
        if (idProcedeFabrication < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idProcedeFabrication), (int)idProcedeFabrication, "idProcedeFabrication should be >= 0");
        }
        
        SqlCommand cmd = new SqlCommand("get_procedeFabrication_by_id");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_procedeFabrication", idProcedeFabrication);
        cmd.Parameters["@id_procedeFabrication"].Direction = ParameterDirection.Input;

        SqlDataReader result = DbConnector.SendQueryRequest(cmd);

        ProcedeFabricationModel? toReturn = null;

        while (result.Read())
        {
            toReturn = new ProcedeFabricationModel()
            {
                Nom = (string)result["ProcedeFabrication.nom"],
                Description = (string)result["description"],
                IdModele = (int)result["id_modele"],
                NomModele = (string)result["Modele.nom"]
            };
        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static List<ProcedeFabricationModel> GetProcedeFabricationByName(string nameProcedeFabrication)
    {
        SqlCommand cmd = new SqlCommand("get_procedeFabrication_by_name");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@nom", nameProcedeFabrication);
        cmd.Parameters["@nom"].Direction = ParameterDirection.Input;

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

    internal static ProcedeFabricationModel GetProcedeFabricationFromModele(int idModele)
    {
        SqlCommand cmd = new SqlCommand("get_procedeFabrication_modele");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@id_modele"].Direction = ParameterDirection.Input;

        SqlDataReader result = DbConnector.SendQueryRequest(cmd);
        
        ProcedeFabricationModel toReturn = new ProcedeFabricationModel();

        while (result.Read())
        {
            toReturn.Id = (int)result["id"];
            toReturn.Nom = (string)result["nom"];
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
    internal static List<TestModel> GetTestsFromProcedeFabrication(int idProcedeFabrication)
    {
        SqlCommand cmd = new SqlCommand("get_procedeFabrication_tests");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_procedeFabrication", idProcedeFabrication);
        cmd.Parameters["@id_procedeFabrication"].Direction = ParameterDirection.Input;

        SqlDataReader result = DbConnector.SendQueryRequest(cmd);
        
        List<TestModel> toReturn = new List<TestModel>();

        while (result.Read())
        {
            TestModel t = new TestModel()
            {
                Id = (int)result["id"],
                Nom = (string)result["nom"],
                Valide = (bool)result["valide"]
            };
            Enum.TryParse((string)result["type"], out t.Type);

            toReturn.Add(t);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
    internal static List<EtapeModel> GetEtapeFromProcedeFabrication(int idProcedeFabrication)
    {
        SqlCommand cmd = new SqlCommand("get_procedeFabrication_etapes");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_procedeFabrication", idProcedeFabrication);
        cmd.Parameters["@id_procedeFabrication"].Direction = ParameterDirection.Input;

        SqlDataReader result = DbConnector.SendQueryRequest(cmd);
        
        List<EtapeModel> toReturn = new List<EtapeModel>();

        while (result.Read())
        {
            EtapeModel e = new EtapeModel()
            {
                Id = (int)result["id"],
                Nom = (string)result["nom"]
            };
            toReturn.Add(e);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    #endregion
    
    #region Add
    internal static void AddFreezbee(string nom, string description, int idModele)
    {
        SqlCommand cmd = new SqlCommand("add_procedeFabrication");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@nom", nom);
        cmd.Parameters["@nom"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@description", description);
        cmd.Parameters["@description"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@pUHT"].Direction = ParameterDirection.Input;
        
        DbConnector.SendNonQueryRequest(cmd);
    }

    internal static void AddIngredientToFreezbee(int idProcedeFabrication, int idTest)
    {
        if (idProcedeFabrication < 0)
            throw new ArgumentOutOfRangeException(nameof(idProcedeFabrication), idProcedeFabrication, "idProcedeFabrication should be >= 0");
        if (idTest < 0)
            throw new ArgumentOutOfRangeException(nameof(idTest), idTest, "idTest should be >= 0");
        
        SqlCommand cmd = new SqlCommand("add_test_to_procedeFabrication");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_procedeFabrication", idProcedeFabrication);
        cmd.Parameters["@id_procedeFabrication"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@id_test", idTest);
        cmd.Parameters["@id_test"].Direction = ParameterDirection.Input;

        DbConnector.SendNonQueryRequest(cmd);
    }
    
    internal static void AddEtapeToProcedeFabrication(int idProcedeFabrication, int idEtape)
    {
        if (idProcedeFabrication < 0)
            throw new ArgumentOutOfRangeException(nameof(idProcedeFabrication), idProcedeFabrication, "idProcedeFabrication should be >= 0");
        if (idEtape < 0)
            throw new ArgumentOutOfRangeException(nameof(idEtape), idEtape, "idEtape should be >= 0");

        SqlCommand cmd = new SqlCommand("add_etape_to_procedeFabrication");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_procedeFabrication", idProcedeFabrication);
        cmd.Parameters["@id_procedeFabrication"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@id_etape", idEtape);
        cmd.Parameters["@id_etape"].Direction = ParameterDirection.Input;

        DbConnector.SendNonQueryRequest(cmd);
    }
    #endregion
    
    #region Delete
    internal static void DeleteProcedeFabrication(int id)
    {
        if (id < 0)
            throw new ArgumentOutOfRangeException(nameof(id), id, "id should be >= 0");
        
        SqlCommand cmd = new SqlCommand("delete_procedeFabrication");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters["@id"].Direction = ParameterDirection.Input;

        DbConnector.SendNonQueryRequest(cmd);
    }
    
    internal static void DeleteTestFromProcedeFabrication(int idProcedeFabrication, int idTest)
    {
        if (idProcedeFabrication < 0)
            throw new ArgumentOutOfRangeException(nameof(idProcedeFabrication), idProcedeFabrication, "idModele should be >= 0");
        if (idTest < 0)
            throw new ArgumentOutOfRangeException(nameof(idTest), idTest, "idTest should be >= 0");

        SqlCommand cmd = new SqlCommand("delete_test_from_procedeFabrication");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_procedeFabrication", idProcedeFabrication);
        cmd.Parameters["@id_procedeFabrication"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@id_test", idTest);
        cmd.Parameters["@id_test"].Direction = ParameterDirection.Input;
        
        DbConnector.SendNonQueryRequest(cmd);
    }
    
    internal static void DeleteEtapeFromProcedeFabrication(int idProcedeFabrication, int idEtape)
    {
        if (idProcedeFabrication < 0)
            throw new ArgumentOutOfRangeException(nameof(idProcedeFabrication), idProcedeFabrication, "idProcedeFabrication should be >= 0");
        if (idEtape < 0)
            throw new ArgumentOutOfRangeException(nameof(idEtape), idEtape, "idEtape should be >= 0");

        SqlCommand cmd = new SqlCommand("delete_etape_from_procedeFabrication");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_procedeFabrication", idProcedeFabrication);
        cmd.Parameters["@id_procedeFabrication"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@id_etape", idEtape);
        cmd.Parameters["@id_etape"].Direction = ParameterDirection.Input;

        DbConnector.SendNonQueryRequest(cmd);
    }
    #endregion
    
    #region Update
    internal static void UpdateProcedeFabrication(int id, string nom, string description, int idModele)
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
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@id_modele"].Direction = ParameterDirection.Input;
        
        DbConnector.SendNonQueryRequest(cmd);
    }
    #endregion
}