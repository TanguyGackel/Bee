using System.Data;
using System.Data.Common;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;

namespace MS_Test.Models;

public class Freezbee
{
    internal int id;
    internal string name;
    internal string description;
    internal int UHT_price;
    internal string gamme;
    
    internal static List<Freezbee> GetFreezbee()
    {
        SqlCommand cmd = new SqlCommand("get_modeles");
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader result = dbConnector.ExecuteNonQuery(cmd);

        List<Freezbee> toReturn = new List<Freezbee>();
        while (result.Read())
        {
            Freezbee f = new Freezbee()
            {
                id = (int)result["id"],
                name = (string)result["nom"],
                description = (string)result["description"],
                UHT_price = (int)result["pUHT"],
                gamme = (string)result["gamme"]
            };
            toReturn.Add(f);
        }
        cmd.Connection?.Close();
        return toReturn;
    }
    
}