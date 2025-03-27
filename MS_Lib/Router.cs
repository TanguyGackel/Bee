using System.Reflection;
using MSLib.Proto;

namespace MS_Lib;

public class Router
{
    public readonly Dictionary<string, Action<IRequest>> Routes = new Dictionary<string, Action<IRequest>>();

    public void AddRoutes(string functionName, Action<IRequest> request)
    {
        if (string.IsNullOrEmpty(functionName))
        {
            throw new ArgumentNullException(nameof(functionName));
        }
        Routes.Add(functionName, request);
    }
}

public abstract class Route
{
    protected Router Router;

    protected Route(Router router)
    {
        Router = router;
    }

    protected void Add(Type controller)
    {
        MethodInfo[] methods = controller.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (MethodInfo method in methods)
        {
            Router.AddRoutes(method.Name, (Action<IRequest>)method.CreateDelegate(typeof(Action<IRequest>), method));
        }
    }

    internal void Call(Packet packet)
    {
        // if (string.IsNullOrEmpty(functionName))
        // {
        //     throw new ArgumentNullException(nameof(functionName));
        // }
        // if (Router == null)
        // {
        //     throw new NullReferenceException(nameof(functionName));
        // }
        //
        // Tuple<Type, Action<IRequest>> info = Router.Routes.First(r => r.Key.Equals(functionName)).Value;

    }

}