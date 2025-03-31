using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using MSLib.Proto;

namespace MS_Lib;

public class Router
{
    public readonly Dictionary<string, Func<IRequest, Task<Response>>> Routes = new Dictionary<string, Func<IRequest, Task<Response>>>();
    public readonly Dictionary<string, Type> Types = new Dictionary<string, Type>();
    
    public void AddRoutes(string functionName, Func<IRequest, Task<Response>> request)
    {
        if (string.IsNullOrEmpty(functionName))
        {
            throw new ArgumentNullException(nameof(functionName));
        }
        Routes.Add(functionName, request);
    }

    public void AddType(string name, Type type)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }
        Types.Add(name, type);
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
            Func<IRequest, Task<Response>> delegateInstance = (Func<IRequest, Task<Response>>)Delegate.CreateDelegate(typeof(Func<IRequest, Task<Response>>), method);
            Router.AddRoutes(method.Name, delegateInstance);
        }
    }

    internal async Task<byte[]> Call(Packet packet)
    {
        if (string.IsNullOrEmpty(packet.Fonction))
        {
            throw new ArgumentNullException(nameof(packet.Fonction));
        }
        if (Router == null)
        {
            throw new NullReferenceException(nameof(Router));
        }


        Type type = Router.Types.First(t => t.Key.Equals(packet.BodyType)).Value;
        Object instanceC = Activator.CreateInstance(type);

        PropertyInfo propertyInfo = type.GetProperty("Parser", BindingFlags.Public | BindingFlags.Static);
        Object instanceO = propertyInfo.GetValue(instanceC);

        List<MethodInfo> methodInfos = instanceO.GetType().GetMethods().Where(m => m.Name == "ParseFrom" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(ByteString)).ToList();

        MethodInfo methodInfo = methodInfos.First();
        
        Func<IRequest, Task<Response>> info = Router.Routes.First(r => r.Key.Equals(packet.Fonction)).Value;

        Response response = await info.Invoke((IRequest)methodInfo.Invoke(instanceO, new object[]{packet.Body}));

        return response.ToByteArray();
    }

}