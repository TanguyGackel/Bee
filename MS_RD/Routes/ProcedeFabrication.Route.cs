using MS_Lib;
using MS_RD.Models;
using MSRD.Proto;

namespace MS_RD.Routes;

internal class ProcedeFabricationRoute : Route
{
    internal ProcedeFabricationRoute() : base(new Router())
    {
        Add(typeof(ProcedeFabricationModel));
        Router.AddType("ProcedeFabrication", typeof(ProcedeFabrication));
    }
}