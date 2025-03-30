using MS_Test.Models;
using Microsoft.Data.SqlClient;
using MSTest.Proto;

namespace MS_Test.Controllers;

internal class Test_Controllers
{
    internal async void GetTests()
    {
        List<Test> result;
        int statusCode;
        string statusDescription;

        try
        {
            result = await TestModel.GetTests();
            if (result.Count == 0)
            {
                statusCode = 204;
                statusDescription = "No content";
            }
            else
            {
                statusCode = 200;
                statusDescription = "OK";
            }
        }
        catch (SqlException)
        {
            statusCode = 500;
            statusDescription = "Internal Server Error";
        }
        catch (TimeoutException)
        {
            statusCode = 504;
            statusDescription = "Database timeout";
        }
    }

    internal async void GetTestById(Test req)
    {
        Test? result;
        int statusCode;
        string statusDescription;

        try
        {
            result = await TestModel.GetTestById(req.IdTest);
            if (string.IsNullOrEmpty(result.NameTest))
            {
                statusCode = 404;
                statusDescription = "Not found";
            }
            else
            {
                statusCode = 200;
                statusDescription = "OK";
            }
        }
        catch (SqlException)
        {
            statusCode = 500;
            statusDescription = "Internal server error";
        }
        catch (TimeoutException)
        {
            statusCode = 504;
            statusDescription = "Database timeout";
        }
        catch (ArgumentOutOfRangeException)
        {
            statusCode = 422;
            statusDescription = "Unprocessable entity";
        }
        
    }
    
    internal async void GetFreezbeeByName(Test req)
    {
        List<Test> result;
        int statusCode;
        string statusDescription;

        try
        {
            result = await TestModel.GetTestByName(req.NameTest);
            if (result.Count == 0)
            {
                statusCode = 404;
                statusDescription = "Not found";
            }
            else
            {
                statusCode = 200;
                statusDescription = "OK";
            }
        }
        catch (SqlException)
        {
            statusCode = 500;
            statusDescription = "Internal server error";
        }
        catch (TimeoutException)
        {
            statusCode = 504;
            statusDescription = "Database timeout";
        }
    }

    internal void UpdateTest(Test req)
    {
        int statusCode;
        string statusDescription;

        try
        {
            TestModel.UpdateTest(req.IdTest, req.Validate);
        }
        catch (SqlException)
        {
            statusCode = 500;
            statusDescription = "Server internal error";
        }
        catch (TimeoutException)
        {
            statusCode = 504;
            statusDescription = "Database timeout";
        }
        catch (ArgumentOutOfRangeException)
        {
            statusCode = 422;
            statusDescription = "Unprocessable entity";
        }
        
    }

    internal async void GetTestProcedeById(Test req)
    {
        List<ProcedeFabrication> result;
        int statusCode;
        string statusDescription;

        try
        {
            result = await TestModel.GetTestProcedeById(req.IdTest);
            if (result.Count == 0)
            {
                statusCode = 404;
                statusDescription = "Not found";
            }
            else
            {
                statusCode = 200;
                statusDescription = "OK";
            }
        }
        catch (SqlException)
        {
            statusCode = 500;
            statusDescription = "Internal server error";
        }
        catch (TimeoutException)
        {
            statusCode = 504;
            statusDescription = "Database timeout";
        }
        catch (ArgumentOutOfRangeException)
        {
            statusCode = 422;
            statusDescription = "Unprocessable entity";
        }
    }

}