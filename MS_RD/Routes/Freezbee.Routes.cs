using MS_Lib;
using MS_RD.Controllers;
using MSRD.Proto;

namespace MS_RD.Routes;

public class FreezbeeRoutes : Route
{
    internal FreezbeeRoutes() : base(new Router())
    {
        Add(typeof(FreezbeeController));
        Router.AddType("Freezbee", typeof(Freezbee));
    }
}