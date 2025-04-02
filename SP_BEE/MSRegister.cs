using System.Net;
using System.Net.Sockets;
using BEE;

namespace SP_BEE;

internal class MSRegister
{
    internal readonly List<MicroService> Register;
    
    private MSRegister()
    {
        Register = new List<MicroService>();
    }
    
    private static MSRegister? _instance;
    internal static MSRegister Instance => _instance ??= new MSRegister();

    internal void RegisterNewMicroService(ToRegister register)
    {
        Console.WriteLine("Registering a new micro service");
        
        Register.Add(new MicroService()
        {
            name = register.Name,
            type = register.Type,
            ip = IPAddress.Parse(register.Ip),
            port = register.Port,
            group = register.Group,
            id = -1
        });
    }

    internal void DeleteMicroService(string name)
    {
        Register.Remove(Register.First(m => m.name == name));
    }

    internal IEnumerable<MicroService> SearchMicroService(string type)
    {
        return Register.Where(w => w.type == type);
    }
}

internal class MicroService
{
    internal string name;
    internal string type;
    internal string group;
    internal IPAddress ip;
    internal int port;
    internal int id;
}