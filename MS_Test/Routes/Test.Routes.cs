using MS_Lib;
using MS_Test.Controllers;
using MSTest.Proto;

namespace MS_Test.Routes;

internal class TestRoutes : Route
{
    internal TestRoutes() : base(new Router())
    {
        Add(typeof(TestControllers));
        Router.Types.Add("Test", typeof(Test));
    }
}