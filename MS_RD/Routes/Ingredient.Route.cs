using MS_Lib;
using MS_RD.Controllers;
using MSRD.Proto;

namespace MS_RD.Routes;

internal class IngredientRoute : Route
{
    internal IngredientRoute() : base(new Router())
    {
        Add(typeof(IngredientControllers));
        Router.AddType("Ingredient", typeof(Ingredient));
        Router.AddType("Freezbee", typeof(Freezbee));
    }
}