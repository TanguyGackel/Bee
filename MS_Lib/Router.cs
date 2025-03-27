namespace MS_Lib;

public class Router
{
    public readonly Dictionary<string, Tuple<Type,Action<IRequest>>> Routes = new Dictionary<string, Tuple<Type,Action<IRequest>>>();

    public void AddRoutes(string functionName, Tuple<Type,Action<IRequest>> request)
    {
        if (string.IsNullOrEmpty(functionName))
        {
            throw new ArgumentNullException(nameof(functionName));
        }
        Routes.Add(functionName, request);
    }
}

public abstract class Routes
{
    protected Router Router;

    protected Routes(Router router)
    {
        Router = router;
    }

    internal void Call(string functionName, string body)
    {
        if (string.IsNullOrEmpty(functionName))
        {
            throw new ArgumentNullException(nameof(functionName));
        }
        if (Router == null)
        {
            throw new NullReferenceException(nameof(functionName));
        }
        
        Tuple<Type, Action<IRequest>> info = Router.Routes.First(r => r.Key.Equals(functionName)).Value;

    }

}