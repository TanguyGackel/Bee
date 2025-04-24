using MS_Lib;
using MS_RD.Controllers;
using MS_RD.Models;
using MSRD.Proto;

namespace MS_RD.Routes;

internal class ProcedeFabricationRoute : Route
{
    internal ProcedeFabricationRoute() : base(new Router())
    {
        Add(typeof(ProcedeFabricationControllers));
        Router.AddType("ProcedeFabrication", typeof(ProcedeFabrication));
    }
}