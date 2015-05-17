using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using Flags = System.Reflection.BindingFlags;

namespace Client
{
    public abstract class LibraryProxy 
    {
        protected readonly object ClassInstance;
        private const BindingFlags BindingFlags = Flags.InvokeMethod | Flags.Instance | Flags.Public;

        protected LibraryProxy(string assemblyPath, string className)
        {
            var assembly = Assembly.LoadFrom(Path.GetFullPath(assemblyPath));
            var container = new UnityContainer();
            container.Resolve(assembly.GetType("LibraryServer.TypeRegistry"));
            ClassInstance = container.Resolve(assembly.GetType(className));
        }

        protected object Invoke(string methodName, object[] objects, bool convertParams = true)
        {
            var method = ClassInstance.GetType().GetMethod(methodName, BindingFlags);
            return method.Invoke(ClassInstance, convertParams ? ConvertParameters(method, objects) : objects);
        }

        //DANGER: Requires ordered matching, [method.GetParameters] has deterministic ordering? 
        private static object[] ConvertParameters(MethodInfo method, IEnumerable<object> paramsToConvert)
        {
            var parameterTypes = method.GetParameters();
            return paramsToConvert.Select((t, i) => t.MapTo(parameterTypes[i].ParameterType)).ToArray();
        }
    }

    public class LibraryFooServiceProxy : LibraryProxy, IFooServiceProxy
    {
        public LibraryFooServiceProxy(string assemblyPath, string className) : base(assemblyPath, className) {}

        public BarResult Get(int id)
        {
            return Invoke("Get", new object[] { id }).MapTo<BarResult>();
        }

        public IEnumerable<BarResult> Search(BarQuery query)
        {
            return Invoke("Search", new object[] { query }).MapTo<IEnumerable<BarResult>>();
        }
    }
}