using Google.Protobuf;
using Microsoft.Data.SqlClient;
using MS_RD.Models;
using MSRD.Proto;

namespace MS_RD.Controllers;

internal class FreezbeeController
{
    internal void GetFreezbee(Freezbee req)
    {
        List<FreezbeeModel>? results;
        int statusCode;
        string statusDescription;

        try
        {
            results = FreezbeeModel.GetFreezbee();

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
        
        
    }
}