using Azure;
using Google.Protobuf;
using Microsoft.Data.SqlClient;
using MS_Lib;
using MS_RD.Models;
using MSRD.Proto;
using Response = MSLib.Proto.Response;

namespace MS_RD.Controllers;

internal class FreezbeeController
{
    internal static async Task<Response> GetFreezbee(IRequest r)
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

        if (results != null)
            foreach (Freezbee f in results)
            {
                response.Body.Add(f.ToByteString());
            }
        else
        {
            response.BodyType = "Null";
        }

        return response;
    }
    
    internal static async Task<Response> GetFreezbeeById(IRequest r)
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

        Response response = new Response()
        {
            StatusCode = statusCode,
            StatusDescription = statusDescription,
            BodyType = "Freezbee",
        };

        if (results != null)
            response.Body.Add(results.ToByteString());
        else
            response.BodyType = "Null";
        
        return response;
    }
    
    internal static async Task<Response> GetFreezbeeByName(IRequest r)
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

        Response response = new Response()
        {
            StatusCode = statusCode,
            StatusDescription = statusDescription,
            BodyType = "Freezbee",
        };

        if (results != null)
            foreach (Freezbee f in results)
            {
                response.Body.Add(f.ToByteString());
            }
        else
        {
            response.BodyType = "Null";
        }
        return response;
    }
    
    internal static async Task<Response> GetFreezbeeByGamme(IRequest r)
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

        Response response = new Response()
        {
            StatusCode = statusCode,
            StatusDescription = statusDescription,
            BodyType = "Freezbee",
        };

        if (results != null)
            foreach (Freezbee f in results)
            {
                response.Body.Add(f.ToByteString());
            }
        else
        {
            response.BodyType = "Null";
        }
        return response;
    }
    
    internal static async Task<Response> GetIngredientsFromFreezbee(IRequest r)
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

        Response response = new Response()
        {
            StatusCode = statusCode,
            StatusDescription = statusDescription,
            BodyType = "IngredientFreezbee",
        };

        if (results != null)
            foreach (IngredientFreezbee ingredient in results)
            {
                response.Body.Add(ingredient.ToByteString());
            }
        else
        {
            response.BodyType = "Null";
        }
        return response;
    }
    
    internal static async Task<Response> GetCaracteristiquesFromFreezbee(IRequest r)
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

        Response response = new Response()
        {
            StatusCode = statusCode,
            StatusDescription = statusDescription,
            BodyType = "CaracteristiqueFreezbee",
        };

        if (results != null)
            foreach (CaracteristiqueFreezbee cf in results)
            {
                response.Body.Add(cf.ToByteString());
            }
        else
        {
            response.BodyType = "Null";
        }
        return response;
    }
    
    internal static async Task<Response> GetProcedeFabricationsFromFreezbee(IRequest r)
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

        Response response = new Response()
        {
            StatusCode = statusCode,
            StatusDescription = statusDescription,
            BodyType = "ProcedeFabricationFreezbee",
        };

        if (results != null)
            foreach (ProcedeFabricationFreezbee pf in results)
            {
                response.Body.Add(pf.ToByteString());
            }
        else
        {
            response.BodyType = "Null";
        }
        return response;
    }

    internal static async Task<Response> AddFreezbee(IRequest r)
    {
        int statusCode;
        string statusDescription;

        Freezbee f = (Freezbee)r;

        try
        {
            await FreezbeeModel.AddFreezbee(f.NameModele, f.Description, f.PUHT, f.GammeModele);
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
            BodyType = "Null",
        };

        return response;
    }

    internal static async Task<Response> AddIngredientToFreezbee(IRequest r)
    {
        int statusCode;
        string statusDescription;

        Freezbee f = (Freezbee)r;
       
        try
        {
            await FreezbeeModel.AddIngredientToFreezbee(f.IdModele, f.Ingredients[0].Id, f.Ingredients[0].Grammage);
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
            BodyType = "Null",
        };

        return response;
    }

    internal static async Task<Response> AddCaracteristiqueToFreezbee(IRequest r)
    {
        int statusCode;
        string statusDescription;

        Freezbee f = (Freezbee)r;
       
        try
        {
            await FreezbeeModel.AddCaracteristiqueToFreezbee(f.IdModele, f.Caracteristiques[0].Id);
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
            BodyType = "Null",
        };

        return response;
    }
    
    
}