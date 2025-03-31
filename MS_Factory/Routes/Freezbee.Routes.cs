using MS_Lib;
using MS_Factory.Controllers;

namespace MS_Factory.Routes;

public class FreezbeeRoutes : Route
{
    internal FreezbeeRoutes() : base(new Router())
    {
        Add(typeof(FreezbeeController));
    }
}