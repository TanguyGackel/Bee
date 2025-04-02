using Google.Protobuf;
using MS_Test.Models;
using Microsoft.Data.SqlClient;
using MS_Lib;
using MSLib.Proto;
using MSTest.Proto;

namespace MS_Test.Controllers;

internal class TestControllers
{
    internal static async Task<Response> GetTests(IRequest req)
    {
        List<Test>? result = null;
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

        Response response = new Response()
        {
            StatusCode = statusCode,
            StatusDescription = statusDescription,
            BodyType = "Test"
        };

        if (result != null)
            foreach (Test t in result)
            {
                response.Body.Add(t.ToByteString());
            }
        else
        {
            response.BodyType = "Null";
        }

        return response;

    }

    internal static async Task<Response> GetTestById(IRequest req)
    {
        Test? result = null;
        int statusCode;
        string statusDescription;

        Test t = (Test)req;
        
        try
        {
            result = await TestModel.GetTestById(t.IdTest);
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

        Response response = new Response()
        {
            StatusCode = statusCode,
            StatusDescription = statusDescription,
            BodyType = "Test"
        };

        if (result != null)
            response.Body.Add(result.ToByteString());
        else
            response.BodyType = "Null";
        return response;
    }
    
    internal static async Task<Response> GetTestByName(IRequest req)
    {
        List<Test>? result = null;
        int statusCode;
        string statusDescription;

        Test t = (Test)req;

        try
        {
            result = await TestModel.GetTestByName(t.NameTest);
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

        Response response = new Response()
        {
            StatusCode = statusCode,
            StatusDescription = statusDescription,
            BodyType = "Test",
        };

        if (result != null)
            foreach (Test test in result)
            {
                response.Body.Add(test.ToByteString());
            }
        else
        {
            response.BodyType = "Null";
        }
        return response;
    }

    internal static async Task<Response> UpdateTest(IRequest req)
    {
        int statusCode;
        string statusDescription;

        Test t = (Test)req;

        try
        {
            await TestModel.UpdateTest(t.IdTest, t.Validate);
            statusCode = 200;
            statusDescription = "OK";
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

        Response response = new Response()
        {
            StatusCode = statusCode,
            StatusDescription = statusDescription,
            BodyType = "Null"
        };

        return response;

    }

    internal static async Task<Response> GetTestProcedeById(IRequest req)
    {
        List<ProcedeFabrication>? result = null;
        int statusCode;
        string statusDescription;

        Test t = (Test)req;
        
        try
        {
            result = await TestModel.GetTestProcedeById(t.IdTest);
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

        Response response = new Response()
        {
            StatusCode = statusCode,
            StatusDescription = statusDescription,
            BodyType = "ProcedeFabrication",
        };

        if (result != null)
            foreach (ProcedeFabrication pf in result)
            {
                response.Body.Add(pf.ToByteString());
            }
        else
        {
            response.BodyType = "Null";
        }
        return response;
    }

}