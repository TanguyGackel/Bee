using Azure;
using Google.Protobuf;
using Google.Protobuf.Collections;
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

        if (results != null)
            foreach (ProcedeFabrication pf in results)
            {
                response.Body.Add(pf.ToByteString());
            }

        return response;

    }

    internal static async Task<Response> GetTestsFromProcedeFabrication(IRequest r)
    {
        List<TestPF>? results = null;
        int statusCode;
        string statusDescription;
        ProcedeFabrication requete = (ProcedeFabrication)r;
            
        try
        {
            results = await ProcedeFabricationModel.GetTestsFromProcedeFabrication(requete.Id);
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
        catch (ArgumentOutOfRangeException)
        {
            statusCode = 422;
            statusDescription = "Unprocessable entity";
        }

        Response response = new Response()
        {
            StatusCode = statusCode,
            StatusDescription = statusDescription,
            BodyType = "ProcedeFabrication"
        };

        if (results != null)
            foreach (TestPF pf in results)
            {
                response.Body.Add(pf.ToByteString());
            }

        return response;

    }

    internal static async Task<Response> GetEtapeFromProcedeFabrication(IRequest r)
    {
        List<EtapePF>? results = null;
        int statusCode;
        string statusDescription;
        ProcedeFabrication requete = (ProcedeFabrication)r;

        try
        {
            results = await ProcedeFabricationModel.GetEtapeFromProcedeFabrication(requete.Id);
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

        if (results != null)
            foreach (EtapePF e in results)
            {
                response.Body.Add(e.ToByteString());
            }

        return response;
    }

    internal static async Task<Response> AddProcedeFabrication(IRequest r)
    {
        int statusCode;
        string statusDescription;
        ProcedeFabrication requete = (ProcedeFabrication)r;
        
        try
        {
            await ProcedeFabricationModel.AddProcedeFabrication(requete.Name, requete.Description, requete.Modele.Id);
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

    internal static async Task<Response> AddTestToProcedeFabrication(IRequest r)
    {
        int statusCode;
        string statusDescription;
        ProcedeFabrication requete = (ProcedeFabrication)r;

        try
        {
            await ProcedeFabricationModel.AddTestToProcedeFabrication(requete.Id, requete.Tests[0].Id);
            statusCode = 200;
            statusDescription = "OK";
        }
        catch (SqlException)
        {
            statusCode = 500;
            statusDescription = "Server internal erro" +
                                "r";
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

    internal static async Task<Response> AddEtapeToProcedeFabrication(IRequest r)
    {
        int statusCode;
        string statusDescription;
        ProcedeFabrication requete = (ProcedeFabrication)r;

        try
        {
            await ProcedeFabricationModel.AddEtapeToProcedeFabrication(requete.Id, requete.Etapes[0].Id);
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

    internal static async Task<Response> DeleteProcedeFabrication(IRequest r)
    {
        int statusCode;
        string statusDescription;
        ProcedeFabrication requete = (ProcedeFabrication)r;

        try
        {
            await ProcedeFabricationModel.DeleteProcedeFabrication(requete.Id);
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

    internal static async Task<Response> DeleteEtapeFromProcedeFabrication(IRequest r)
    {
        int statusCode;
        string statusDescription;
        ProcedeFabrication requete = (ProcedeFabrication)r;

        try
        {
            await ProcedeFabricationModel.DeleteEtapeFromProcedeFabrication(requete.Id, requete.Etapes[0].Id);
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

    internal static async Task<Response> UpdateProcedeFabrication(IRequest r)
    {
        int statusCode;
        string statusDescription;
        ProcedeFabrication requete = (ProcedeFabrication)r;

        try
        {
            await ProcedeFabricationModel.UpdateProcedeFabrication(requete.Id, requete.Name, requete.Description, requete.Modele.Id);

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