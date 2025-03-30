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
        Response re = new Response();
        
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
        
        foreach (Freezbee f in result)
        {
            response.Body.Add(f.ToByteString());
        }

        return response;

    }

    internal static async Task<Response> GetFreezbeeById(IRequest req)
    {
        Freezbee? result;
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

        return new Response();
    }

    internal static async Task<Response> GetFreezbeeByName(Freezbee req)
    {
        List<Freezbee> result;
        int statusCode;
        string statusDescription;

        try
        {
            result = await FreezbeeModel.GetFreezbeeByName(req.NameModele);
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
        return new Response();
    }
    
    internal static async Task<Response> GetFreezbeeByGamme(Freezbee req)
    {
        List<Freezbee> result;
        int statusCode;
        string statusDescription;

        try
        {
            result = await FreezbeeModel.GetFreezbeeByGamme(req.GammeModele);
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
        return new Response();
    }
    
    internal static async Task<Response> GetFreezbeeTestById(Freezbee req)
    {
        List<TestFreezbee> result;
        int statusCode;
        string statusDescription;

        try
        {
            result = await FreezbeeModel.GetFreezbeeTestById(req.IdModele);
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
        return new Response();
    }
    
}