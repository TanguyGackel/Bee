using System.Data;
using Microsoft.Data.SqlClient;
using MS_Lib;
using MSTest.Proto;

namespace MS_Test.Models;

internal class TestModel
{
    
    private static readonly DatabaseConnector dbConnector = DatabaseConnector.Instance;

    internal static async Task<List<Test>> GetTests()
    {
        SqlCommand cmd = new SqlCommand("get_tests");
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader result = await dbConnector.SendQueryRequest(cmd);

        List<Test> toReturn = new List<Test>();
        while (result.Read())
        {
            Test f = new Test()
            {
                IdTest = (int)result["id"],
                NameTest = (string)result["nom"],
                Type = (string)result["type"],
                Validate = (bool)result["valide"]
            };
            toReturn.Add(f);

        }
        cmd.Connection?.Close();
        return toReturn;
    }

    internal static async Task<Test?> GetTestById(int idTest)
    {
        if (idTest < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idTest), idTest, "Value should be >= 0" );
        }
        
        SqlCommand cmd = new SqlCommand("get_tests_by_id");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_test", idTest);
        cmd.Parameters["@id_test"].Direction = ParameterDirection.Input;
        
        SqlDataReader result = await dbConnector.SendQueryRequest(cmd);

        Test? toReturn = null;

        while (result.Read())
        {
            toReturn = new Test()
            {
                NameTest = (string)result["nom"],
                Description = (string)result["description"],
                Type = (string)result["type"],
                Validate = (bool)result["valide"]
            };
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
    internal static async Task<List<Test>> GetTestByName(string nameTest)
    {
        SqlCommand cmd = new SqlCommand("get_tests_by_name");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@nom", nameTest);
        cmd.Parameters["@nom"].Direction = ParameterDirection.Input;
        
        SqlDataReader result = await dbConnector.SendQueryRequest(cmd);

        List<Test> toReturn = new List<Test>();

        while (result.Read())
        {
            Test t = new Test()
            {
                IdTest = (int)result["id"],
                NameTest = (string)result["nom"],
                Type = (string)result["type"],
                Validate = (bool)result["valide"]
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

    internal static async Task<List<ProcedeFabrication>> GetTestProcedeById(int idTest)
    {
        if (idTest < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idTest), idTest, "Value should be >= 0" );
        }
        
        SqlCommand cmd = new SqlCommand("get_procede_test");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@id_test", idTest);
        cmd.Parameters["@id_test"].Direction = ParameterDirection.Input;
        
        SqlDataReader result = await dbConnector.SendQueryRequest(cmd);

        List<ProcedeFabrication> toReturn = new List<ProcedeFabrication>();

        while (result.Read())
        {
            ProcedeFabrication p = new ProcedeFabrication()
            {
                Id = (int)result["id"],
                Name = (string)result["nom"],
                Description = (string)result["description"]
 
            };
            toReturn.Add(p);    
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
}