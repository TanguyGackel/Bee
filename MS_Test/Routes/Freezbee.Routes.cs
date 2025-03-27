using MS_Lib;
using MS_Test.Controllers;

namespace MS_Test.Routes;

internal class FreezbeeRoutes : Route
{
    internal FreezbeeRoutes() : base(new Router())
    {
        Add(typeof(FreezbeeController));
    }
}