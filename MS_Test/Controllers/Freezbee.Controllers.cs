using Google.Protobuf;
using Microsoft.Data.SqlClient;
using MS_Test.Models;
using MSTest.Proto;

namespace MS_Test.Controllers;

internal class FreezbeeController
{

    internal void GetFreezbee()
    {
        List<FreezbeeModel> result;
        int statusCode;
        string statusDescription;

        try
        {
            result = FreezbeeModel.GetFreezbee();
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

    internal void GetFreezbeeById(Freezbee req)
    {
        FreezbeeModel? result;
        int statusCode;
        string statusDescription;

        try
        {
            result = FreezbeeModel.GetFreezbeeById(req.IdModele);
            if (string.IsNullOrEmpty(result.name))
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

    internal void GetFreezbeeByName(Freezbee req)
    {
        List<FreezbeeModel> result;
        int statusCode;
        string statusDescription;

        try
        {
            result = FreezbeeModel.GetFreezbeeByName(req.NameModele);
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
    
    internal void GetFreezbeeByGamme(Freezbee req)
    {
        List<FreezbeeModel> result;
        int statusCode;
        string statusDescription;

        try
        {
            result = FreezbeeModel.GetFreezbeeByGamme(req.GammeModele);
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
    
    internal void GetFreezbeeTestById(Freezbee req)
    {
        List<TestModel> result;
        int statusCode;
        string statusDescription;

        try
        {
            result = FreezbeeModel.GetFreezbeeTestById(req.IdModele);
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