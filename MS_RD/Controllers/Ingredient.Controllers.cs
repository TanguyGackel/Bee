using System.Data;
using Google.Protobuf;
using Microsoft.Data.SqlClient;
using MS_Lib;
using MS_RD.Models;
using MSLib.Proto;
using MSRD.Proto;

namespace MS_RD.Controllers;

internal class IngredientControllers
{
    internal static async Task<Response> GetIngredients(IRequest r)
    {
        List<Ingredient>? results = null;
        int statusCode;
        string statusDescription;

        try
        {
            results = await IngredientModel.GetIngredients();

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
            BodyType = "Ingredient"
        };
        
        foreach (Ingredient f in results)
        {
            response.Body.Add(f.ToByteString());
        }
        return response;
    }
    internal static async Task<Response> GetIngredientById(IRequest r)
    {
        Ingredient? results = null;
        int statusCode;
        string statusDescription;

        Ingredient i = (Ingredient)r;

        try
        {
            results = await IngredientModel.GetIngredientById(i.Id);
            
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
            BodyType = "Ingredient"
        };
        

        response.Body.Add(results.ToByteString());
        
        return response;
    }
    internal static async Task<Response> GetIngredientByName(IRequest r)
    {
        List<Ingredient>? results = null;
        int statusCode;
        string statusDescription;

        Ingredient i = (Ingredient)r;

        try
        {
            results = await IngredientModel.GetIngredientByName(i.Name);

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
            BodyType = "Ingredient"
        };
        
        foreach (Ingredient f in results)
        {
            response.Body.Add(f.ToByteString());
        }
        return response;
    }

    internal static async Task<Response> GetFreezbeesFromIngredient(IRequest r)
    {
        List<FreezbeeI>? results = null;
        int statusCode;
        string statusDescription;

        Ingredient i = (Ingredient)r;

        try
        {
            results = await IngredientModel.GetFreezbeesFromIngredient(i.Id);

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
            BodyType = "Ingredient"
        };

        foreach (FreezbeeI f in results)
        {
            response.Body.Add(f.ToByteString());
        }

        return response;
    }
    internal static async Task<Response> AddIngredient(IRequest r)
    {
        int statusCode;
        string statusDescription;

        Ingredient f = (Ingredient)r;

        try
        {
            await IngredientModel.AddIngredient(f.Name, f.Description);
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
            BodyType = "null",
        };

        return response;
    }
    internal static async Task<Response> DeleteIngredient(IRequest r)
    {
        int statusCode;
        string statusDescription;

        Ingredient f = (Ingredient)r;

        try
        {
            await IngredientModel.DeleteIngredient(f.Id);
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
            BodyType = "null",
        };

        return response;
    }
    internal static async Task<Response> UpdateModele(IRequest r)
    {
        int statusCode;
        string statusDescription;

        FreezbeeI f = (FreezbeeI)r;

        try
        {
            await IngredientModel.UpdateModele(f.Id, f.Name, f.Description);
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
            BodyType = "null",
        };

        return response;
    }
}