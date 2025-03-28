using MS_Lib;
using MS_RD.Controllers;

namespace MS_RD.Routes;

public class FreezbeeRoutes : Route
{
    internal FreezbeeRoutes() : base(new Router())
    {
        Add(typeof(FreezbeeController));
    }
}