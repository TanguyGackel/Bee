using Azure;
using Google.Protobuf;
using Microsoft.Data.SqlClient;
using MS_Lib;
using MS_RD.Models;
using MSRD.Proto;
using Response = MSLib.Proto.Response;

namespace MS_RD.Controllers;

internal class ProcedeFabricationControllers
{
    internal static async Task<Response> GetProcedeFabrications(IRequest r)
    {
        List<ProcedeFabrication>? results = null;
        int statusCode;
        string statusDescription;

        try
        {
            results = await ProcedeFabricationModel.GetProcedeFabrications();

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
            BodyType = "ProcedeFabrication"
        };
        
        foreach (ProcedeFabrication pf in results)
        {
            response.Body.Add(pf.ToByteString());
        }
        return response;
    }

    internal static async Task<Response> GetProcedeFabricationById(IRequest r)
    {
        ProcedeFabrication? result = new ProcedeFabrication();
        int statusCode;
        string statusDescription;

        ProcedeFabrication requete = (ProcedeFabrication)r;

        try
        {
            result = await ProcedeFabricationModel.GetProcedeFabricationById(requete.Id);
            if (result == null)
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
            BodyType = "ProcedeFabrication",
        };
        
        response.Body.Add(result.ToByteString());
        
        return response;
        
    }
    
    internal static async Task<Response> GetProcedeFabricationByName(IRequest r)
    {
        List<ProcedeFabrication>? results = null;
        int statusCode;
        string statusDescription;
        ProcedeFabrication requete = (ProcedeFabrication)r;

        try
        {
            results = await ProcedeFabricationModel.GetProcedeFabricationByName(requete.Name);

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
            BodyType = "ProcedeFabrication",
        };

        if (results != null)
            foreach (ProcedeFabrication f in results)
            {
                response.Body.Add(f.ToByteString());
            }

        return response;
    }

    internal static async Task<Response> GetProcedeFabricationFromModele(IRequest r)
    {
        List<ProcedeFabrication>? results = null;
        int statusCode;
        string statusDescription;
        ProcedeFabrication requete = (ProcedeFabrication)r;

        try
        {
            results = await ProcedeFabricationModel.GetProcedeFabricationFromModele(requete.Modele.Id);
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
            BodyType = "ProcedeFabrication",
        };

        if (results != null)
            foreach (ProcedeFabrication pf in results)
            {
                response.Body.Add(pf.ToByteString());
            }

        return response;

    }
    
}