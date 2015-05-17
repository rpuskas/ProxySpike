using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Client
{
    public class LibraryClient : IProxy
    {
        private readonly string _assemblyPath;
        private readonly string _className;

        public LibraryClient(string assemblyPath, string className)
        {
            _assemblyPath = assemblyPath;
            _className = className;
        }

        public BarResult Get(int id)
        {
            return Result(_className, "Get", new object[] {id}).MapTo<BarResult>();
        }

        public IEnumerable<BarResult> Search(BarQuery query)
        {
            return Result(_className, "Search", new object[] { query }).MapTo<IEnumerable<BarResult>>();
        }

        private object Result(string libraryserverFoo, string methodName, object[] objects)
        {
            var assembly = Assembly.LoadFrom(Path.GetFullPath(_assemblyPath));
            var classInstance = Activator.CreateInstance(assembly.GetType(libraryserverFoo));
            var parameterTypes = assembly.GetType(libraryserverFoo).GetMethod(methodName).GetParameters();
            var convertedParameters = objects.Select((t, i) => t.MapTo(parameterTypes[i].ParameterType)).ToArray();

            const BindingFlags bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public;
            return assembly.GetType(libraryserverFoo).InvokeMember(methodName, bindingFlags, null, classInstance, convertedParameters );
        }
    }
}