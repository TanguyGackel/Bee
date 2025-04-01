using Google.Protobuf;
using Microsoft.Data.SqlClient;
using MS_Lib;
using MS_Test.Models;
using MSLib.Proto;
using MSTest.Proto;

namespace MS_Test.Controllers;

internal class FreezbeeController
{

    internal static async Task<Response> GetFreezbee(IRequest r)
    {
        List<Freezbee>? result = null;
        
        int statusCode;
        string statusDescription;

        try
        {
            result = await FreezbeeModel.GetFreezbee();
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
            BodyType = "Freezbee"
        };

        if (result != null)
            foreach (Freezbee f in result)
            {
                response.Body.Add(f.ToByteString());
            }
        else
        {
            response.BodyType = "Null";
        }

        return response;

    }

    internal static async Task<Response> GetFreezbeeById(IRequest req)
    {
        Freezbee? result = null;
        int statusCode;
        string statusDescription;

        Freezbee f = (Freezbee)req;

        try
        {
            result = await FreezbeeModel.GetFreezbeeById(f.IdModele);
            if (string.IsNullOrEmpty(result.NameModele))
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
            BodyType = "Freezbee"
        };
        
        response.Body.Add(result.ToByteString());
        return response;
        
    }

    internal static async Task<Response> GetFreezbeeByName(IRequest req)
    {
        List<Freezbee>? result = null;
        int statusCode;
        string statusDescription;

        Freezbee f = (Freezbee)req;
        
        try
        {
            result = await FreezbeeModel.GetFreezbeeByName(f.NameModele);
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
            BodyType = "Freezbee"
        };

        if (result != null)
            foreach (Freezbee freezbee in result)
            {
                response.Body.Add(freezbee.ToByteString());
            }
        else
        {
            response.BodyType = "Null";
        }
        return response;
    }
    
    internal static async Task<Response> GetFreezbeeByGamme(IRequest req)
    {
        List<Freezbee>? result = null;
        int statusCode;
        string statusDescription;
        Freezbee f = (Freezbee)req;

        try
        {
            result = await FreezbeeModel.GetFreezbeeByGamme(f.GammeModele);
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
            BodyType = "Freezbee"
        };

        if (result != null)
            foreach (Freezbee freezbee in result)
            {
                response.Body.Add(freezbee.ToByteString());
            }
        else
        {
            response.BodyType = "Null";
        }
        return response;
    }
    
    internal static async Task<Response> GetFreezbeeTestById(IRequest req)
    {
        List<TestFreezbee>? result = null;
        int statusCode;
        string statusDescription;
        Freezbee f = (Freezbee)req;

        try
        {
            result = await FreezbeeModel.GetFreezbeeTestById(f.IdModele);
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
            BodyType = "TestFreezbee"
        };

        if (result != null)
            foreach (TestFreezbee testF in result)
            {
                response.Body.Add(testF.ToByteString());
            }
        else
        {
            response.BodyType = "Null";
        }
        return response;
    }
    
}