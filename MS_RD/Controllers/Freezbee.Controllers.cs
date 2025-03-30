using Google.Protobuf;
using Microsoft.Data.SqlClient;
using MS_Lib;
using MS_RD.Models;
using MSLib.Proto;
using MSTest.Proto;

namespace MS_RD.Controllers;

internal class FreezbeeController
{
    internal async Task<Response> GetFreezbee(IRequest r)
    {
        List<Freezbee>? results = null;
        int statusCode;
        string statusDescription;

        try
        {
            results = await FreezbeeModel.GetFreezbee();

            if (results.Count == 0)
            {
                statusCode = 204;
                statusDescription = "No Content";
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
            statusDescription = "Database TimeOut";
        }

        Response response = new Response()
        {
            StatusCode = statusCode,
            StatusDescription = statusDescription,
            BodyType = "Freezbee"
        };
        
        foreach (Freezbee f in results)
        {
            response.Body.Add(f.ToByteString());
        }
        return response;
    }
    
    internal async Task<Response> GetFreezbeeById(IRequest r)
    {
        Freezbee? results = null;
        int statusCode;
        string statusDescription;
        Freezbee requete = (Freezbee)r;

        try
        {
            results = await FreezbeeModel.GetFreezbeeById(requete.IdModele);

            if (results == null)
            {
                statusCode = 404;
                statusDescription = "Not Found";
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
            statusDescription = "Database TimeOut";
        }

        return new Response();
    }
    
    internal async Task<Response> GetFreezbeeByName(IRequest r)
    {
        List<Freezbee>? results = null;
        int statusCode;
        string statusDescription;
        Freezbee requete = (Freezbee)r;

        try
        {
            results = await FreezbeeModel.GetFreezbeeByName(requete.NameModele);

            if (results.Count == 0)
            {
                statusCode = 204;
                statusDescription = "No Content";
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
            statusDescription = "Database TimeOut";
        }

        return new Response();
    }
    
    internal async Task<Response> GetFreezbeeByGamme(IRequest r)
    {
        List<Freezbee>? results = null;
        int statusCode;
        string statusDescription;
        Freezbee requete = (Freezbee)r;
        
        try
        {
            results = await FreezbeeModel.GetFreezbeeByGamme(requete.GammeModele);

            if (results.Count == 0)
            {
                statusCode = 204;
                statusDescription = "No Content";
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
            statusDescription = "Database TimeOut";
        }

        return new Response();
    }
    
    internal async Task<Response> GetIngredientsFromFreezbee(IRequest r)
    {
        List<IngredientFreezbee>? results = null;
        int statusCode;
        string statusDescription;
        Freezbee requete = (Freezbee)r;

        try
        {
            results = await FreezbeeModel.GetIngredientsFromFreezbee(requete.IdModele);

            if (results.Count == 0)
            {
                statusCode = 204;
                statusDescription = "No Content";
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
            statusDescription = "Database TimeOut";
        }

        return new Response();
    }
    
    internal async Task<Response> GetCaracteristiquesFromFreezbee(IRequest r)
    {
        List<CaracteristiqueFreezbee>? results = null;
        int statusCode;
        string statusDescription;
        Freezbee requete = (Freezbee)r;

        try
        {
            results = await FreezbeeModel.GetCaracteristiquesFromFreezbee(requete.IdModele);

            if (results.Count == 0)
            {
                statusCode = 204;
                statusDescription = "No Content";
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
            statusDescription = "Database TimeOut";
        }

        return new Response();
    }
    
    internal async Task<Response> GetProcedeFabricationsFromFreezbee(IRequest r)
    {
        List<ProcedeFabricationFreezbee>? results = null;
        int statusCode;
        string statusDescription;
        Freezbee requete = (Freezbee)r;

        try
        {
            results = await FreezbeeModel.GetProcedeFabricationsFromFreezbee(requete.IdModele);

            if (results.Count == 0)
            {
                statusCode = 204;
                statusDescription = "No Content";
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
            statusDescription = "Database TimeOut";
        }

        return new Response();
    }
}