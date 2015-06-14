using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace Client
{
    public abstract class ClassProxy 
    {
        protected readonly object ClassInstance;
        private const BindingFlags BindingFlags = 
            System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;

        protected ClassProxy(string className, IUnityContainer container, Assembly assembly)
        {
            //Assumes Bootstrapper type
            container.Resolve(assembly.GetType("LibraryServer.Bootstrapper"));
            ClassInstance = container.Resolve(assembly.GetType(className));
        }

        protected object Invoke(string methodName, object[] objects, bool convertParams = true)
        {
            var method = ClassInstance.GetType().GetMethod(methodName, BindingFlags);
            return method.Invoke(ClassInstance, convertParams ? ConvertParameters(method, objects) : objects);
        }

        //DANGER: Requires ordered matching, unsure if [method.GetParameters] has deterministic ordering 
        private static object[] ConvertParameters(MethodInfo method, IEnumerable<object> paramsToConvert)
        {
            var parameterTypes = method.GetParameters();
            return paramsToConvert.Select((t, i) => t.MapTo(parameterTypes[i].ParameterType)).ToArray();
        }
    }
}