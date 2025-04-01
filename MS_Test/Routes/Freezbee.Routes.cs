using MS_Lib;
using MS_Test.Controllers;
using MSTest.Proto;

namespace MS_Test.Routes;
,
internal class FreezbeeRoutes : Route
{
    internal FreezbeeRoutes() : base(new Router())
    {
        Add(typeof(FreezbeeController));
        Router.Types.Add("Freezbee", typeof(Freezbee));
    }
}