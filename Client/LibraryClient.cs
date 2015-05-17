using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace Client
{
    public class LibraryClient : IProxy
    {
        private readonly string _className;
        private readonly Assembly _assembly;
        private readonly UnityContainer _container;

        public LibraryClient(string assemblyPath, string className)
        {
            _className = className;
            _assembly = Assembly.LoadFrom(Path.GetFullPath(assemblyPath));
            _container = new UnityContainer();
            Invoke("LibraryServer.TypeRegistry", "Configure", new object[] { _container }, convertParams: false);
        }

        public BarResult Get(int id)
        {
            return Invoke(_className, "Get", new object[] { id }).MapTo<BarResult>();
        }

        public IEnumerable<BarResult> Search(BarQuery query)
        {
            return Invoke(_className, "Search", new object[] { query }).MapTo<IEnumerable<BarResult>>();
        }

        private object Invoke(string className, string methodName, object[] objects, bool convertParams = true)
        {
            const BindingFlags bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public;
            
            var classInstance = _container.Resolve(_assembly.GetType(className));
            var method = classInstance.GetType().GetMethod(methodName, bindingFlags);

            return method.Invoke(classInstance, convertParams ? ConvertParameters(method, objects) : objects);
        }

        //DANGER: Requires ordered matching, [method.GetParameters] has deterministic ordering? 
        private static object[] ConvertParameters(MethodInfo method, IEnumerable<object> paramsToConvert)
        {
            var parameterTypes = method.GetParameters();
            return paramsToConvert.Select((t, i) => t.MapTo(parameterTypes[i].ParameterType)).ToArray();
        }
    }
}