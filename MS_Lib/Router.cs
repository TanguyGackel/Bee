using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using MSLib.Proto;

namespace MS_Lib;

public class Router
{
    public readonly Dictionary<string, Func<IRequest, Task<Response>>> Routes = new Dictionary<string, Func<IRequest, Task<Response>>>();

    public void AddRoutes(string functionName, Func<IRequest, Task<Response>> request)
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
            Router.AddRoutes(method.Name, (Func<IRequest, Task<Response>>)method.CreateDelegate(typeof(Func<IRequest, Task<Response>>), method));
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

        Type type = Type.GetType(packet.BodyType);
        Object instanceC = Activator.CreateInstance(type);

        FieldInfo fieldinfo = type.GetField("Parser");
        Object instanceO = fieldinfo.GetValue(instanceC);

        MethodInfo methodInfo = instanceO.GetType().GetMethod("ParseFrom");
        
        Func<IRequest, Task<Response>> info = Router.Routes.First(r => r.Key.Equals(packet.Fonction)).Value;

        Response response = await info.Invoke((IRequest)methodInfo.Invoke(instanceO, new object[]{packet.Body}));

        return response.ToByteArray();
    }

}