using System.Data;
using Microsoft.Data.SqlClient;
using MS_Lib;

namespace MS_Test.Models;

internal class TestModel
{
    internal TestModel()
    {
        name = "";
        description = "";
        type = "";
    }

    private static readonly DatabaseConnector dbConnector = DatabaseConnector.Instance;
    
    internal int id;
    internal string name;
    internal string description;
    internal string type;
    internal bool valide;

    internal static List<TestModel> GetTests()
    {
        SqlCommand cmd = new SqlCommand("get_tests");
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader result = dbConnector.SendQueryRequest(cmd);

        List<TestModel> toReturn = new List<TestModel>();
        while (result.Read())
        {
            TestModel f = new TestModel()
            {
                id = (int)result["id"],
                name = (string)result["nom"],
                type = (string)result["type"],
                valide = (bool)result["valide"]
            };
            toReturn.Add(f);

        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static TestModel? GetTestById(int idTest)
    {
        if (idTest < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idTest), idTest, "Value should be >= 0" );
        }
        
        SqlCommand cmd = new SqlCommand("get_tests_by_id");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_test", idTest);
        cmd.Parameters["@id_test"].Direction = ParameterDirection.Input;
        
        SqlDataReader result = dbConnector.SendQueryRequest(cmd);

        TestModel? toReturn = null;

        while (result.Read())
        {
            toReturn = new TestModel()
            {
                name = (string)result["nom"],
                description = (string)result["description"],
                type = (string)result["type"],
                valide = (bool)result["valide"]
            };
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
    internal static List<TestModel> GetTestByName(string nameTest)
    {
        SqlCommand cmd = new SqlCommand("get_tests_by_name");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@nom", nameTest);
        cmd.Parameters["@nom"].Direction = ParameterDirection.Input;
        
        SqlDataReader result = dbConnector.SendQueryRequest(cmd);

        List<TestModel> toReturn = new List<TestModel>();

        while (result.Read())
        {
            TestModel t = new TestModel()
            {
                id = (int)result["id"],
                name = (string)result["nom"],
                type = (string)result["type"],
                valide = (bool)result["valide"]
            };
            toReturn.Add(t);
        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static void UpdateTest(int idTest, bool validate)
    {
        if (idTest < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idTest), idTest, "Value should be >= 0" );
        }
        
        SqlCommand cmd = new SqlCommand("update_test");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_test", idTest);
        cmd.Parameters["@id_test"].Direction = ParameterDirection.Input;
        cmd.Parameters.AddWithValue("@valide_field", validate);
        cmd.Parameters["@valide_field"].Direction = ParameterDirection.Input;
        
        dbConnector.SendNonQueryRequest(cmd);
    }

    internal static List<ProcedeFabricationModel> GetTestProcedeById(int idTest)
    {
        if (idTest < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idTest), idTest, "Value should be >= 0" );
        }
        
        SqlCommand cmd = new SqlCommand("get_procede_test");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_test", idTest);
        cmd.Parameters["@id_test"].Direction = ParameterDirection.Input;
        
        SqlDataReader result = dbConnector.SendQueryRequest(cmd);

        List<ProcedeFabricationModel> toReturn = new List<ProcedeFabricationModel>();

        while (result.Read())
        {
            ProcedeFabricationModel p = new ProcedeFabricationModel()
            {
                id = (int)result["id"],
                name = (string)result["nom"],
                description = (string)result["description"]
 
            };
            toReturn.Add(p);    
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
}