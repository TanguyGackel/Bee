using System.Data;
using Google.Protobuf;
using Microsoft.Data.SqlClient;
using MS_Lib;
using MSRD;
using MSRD.Proto;

namespace MS_RD.Models;

internal static class ProcedeFabricationModel
{
    private static readonly DatabaseConnector DbConnector = DatabaseConnector.Instance;
    
    #region Get
    internal static async Task<List<ProcedeFabrication>> GetProcedeFabrications()
    {
        SqlCommand cmd = new SqlCommand("get_procedeFabrications");
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);

        List<ProcedeFabrication> toReturn = new List<ProcedeFabrication>();
        while (result.Read())
        {
            ProcedeFabrication pf = new ProcedeFabrication()
            {
                Id = (int)result["id"],
                Name = (string)result["nom"],
            };
            toReturn.Add(pf);

        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static async Task<ProcedeFabrication?> GetProcedeFabricationById(int idProcedeFabrication)
    {
        if (idProcedeFabrication < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idProcedeFabrication), (int)idProcedeFabrication, "idProcedeFabrication should be >= 0");
        }
        
        SqlCommand cmd = new SqlCommand("get_procedeFabrication_by_id");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_procedeFabrication", idProcedeFabrication);
        cmd.Parameters["@id_procedeFabrication"].Direction = ParameterDirection.Input;

        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);

        ProcedeFabrication? toReturn = null;

        while (result.Read())
        {
            ModelePF model = new ModelePF()
            {
                Id = (int)result["id_modele"],
                Name = (string)result["Modele.nom"]
            };
            
            toReturn = new ProcedeFabrication()
            {
                Name = (string)result["ProcedeFabrication.nom"],
                Description = (string)result["description"],
                Modele = model,
            };
        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static async Task<List<ProcedeFabrication>> GetProcedeFabricationByName(string nameProcedeFabrication)
    {
        SqlCommand cmd = new SqlCommand("get_procedeFabrication_by_name");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@nom", nameProcedeFabrication);
        cmd.Parameters["@nom"].Direction = ParameterDirection.Input;

        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);

        List<ProcedeFabrication> toReturn = new List<ProcedeFabrication>();

        while (result.Read())
        {
            ProcedeFabrication pf = new ProcedeFabrication()
            {
                Id = (int)result["id"],
                Name = (string)result["nom"]
            };
            toReturn.Add(pf);
        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static async Task<List<ProcedeFabrication>> GetProcedeFabricationFromModele(int idModele)
    {
        
        if (idModele < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idModele), idModele, "idProcedeFabrication should be >= 0");
        }
        
        SqlCommand cmd = new SqlCommand("get_procedeFabrication_modele");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@id_modele"].Direction = ParameterDirection.Input;

        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);
        
        List<ProcedeFabrication> toReturn = new List<ProcedeFabrication>();

        while (result.Read())
        {
            ProcedeFabrication pf = new ProcedeFabrication()
            {
                Id = (int)result["id"],
                Name = (string)result["nom"],
            };
            toReturn.Add(pf);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
    internal static async Task<List<TestPF>> GetTestsFromProcedeFabrication(int idProcedeFabrication)
    {
        if (idProcedeFabrication < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idProcedeFabrication), idProcedeFabrication, "idProcedeFabrication should be >= 0");
        }
        
        SqlCommand cmd = new SqlCommand("get_procedeFabrication_tests");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_procedeFabrication", idProcedeFabrication);
        cmd.Parameters["@id_procedeFabrication"].Direction = ParameterDirection.Input;

        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);
        
        List<TestPF> toReturn = new List<TestPF>();

        while (result.Read())
        {
            TestPF t = new TestPF()
            {
                Id = (int)result["id"],
                Name = (string)result["nom"],
                Validate = (bool)result["valide"],
                Type = (string)result["type"],
            };
            
            toReturn.Add(t);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
    internal static async Task<List<EtapePF>> GetEtapeFromProcedeFabrication(int idProcedeFabrication)
    {
        if (idProcedeFabrication < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idProcedeFabrication), idProcedeFabrication, "idProcedeFabrication should be >= 0");
        }
        
        SqlCommand cmd = new SqlCommand("get_procedeFabrication_etapes");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_procedeFabrication", idProcedeFabrication);
        cmd.Parameters["@id_procedeFabrication"].Direction = ParameterDirection.Input;

        SqlDataReader result = await DbConnector.SendQueryRequest(cmd);
        
        List<EtapePF> toReturn = new List<EtapePF>();

        while (result.Read())
        {
            EtapePF e = new EtapePF()
            {
                Id = (int)result["id"],
                Name = (string)result["nom"]
            };
            toReturn.Add(e);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    #endregion
    
    #region Add
    internal static async Task<int> AddProcedeFabrication(string nom, string description, int idModele)
    {
        if (idModele < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idModele), idModele, "idModele should be >= 0");
        }
        
        SqlCommand cmd = new SqlCommand("add_procedeFabrication");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@nom", nom);
        cmd.Parameters["@nom"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@description", description);
        cmd.Parameters["@description"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@pUHT"].Direction = ParameterDirection.Input;
        
        return await DbConnector.SendNonQueryRequest(cmd);
    }

    internal static async Task<int> AddTestToProcedeFabrication(int idProcedeFabrication, int idTest)
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

        return await DbConnector.SendNonQueryRequest(cmd);
    }
    
    internal static async Task<int> AddEtapeToProcedeFabrication(int idProcedeFabrication, int idEtape)
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

        return await DbConnector.SendNonQueryRequest(cmd);
    }
    #endregion
    
    #region Delete
    internal static async Task<int> DeleteProcedeFabrication(int id)
    {
        if (id < 0)
            throw new ArgumentOutOfRangeException(nameof(id), id, "id should be >= 0");
        
        SqlCommand cmd = new SqlCommand("delete_procedeFabrication");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters["@id"].Direction = ParameterDirection.Input;

        return await DbConnector.SendNonQueryRequest(cmd);
    }
    
    internal static async Task<int> DeleteTestFromProcedeFabrication(int idProcedeFabrication, int idTest)
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
        
        return await DbConnector.SendNonQueryRequest(cmd);
    }
    
    internal static async Task<int> DeleteEtapeFromProcedeFabrication(int idProcedeFabrication, int idEtape)
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

        return await DbConnector.SendNonQueryRequest(cmd);
    }
    #endregion
    
    #region Update
    internal static async Task<int> UpdateProcedeFabrication(int id, string nom, string description, int idModele)
    {
        if (id < 0)
            throw new ArgumentOutOfRangeException(nameof(id), id, "id should be >= 0");
        
        SqlCommand cmd = new SqlCommand("update_procedeFabrication");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters["@id"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@nom", nom);
        cmd.Parameters["@nom"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@description", description);
        cmd.Parameters["@description"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@id_modele", idModele);
        cmd.Parameters["@id_modele"].Direction = ParameterDirection.Input;
        
        return await DbConnector.SendNonQueryRequest(cmd);
    }
    #endregion
}