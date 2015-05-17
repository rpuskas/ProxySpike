using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace Client
{
    public class FooServiceClassProxy : ClassProxy, IFooService
    {
        public FooServiceClassProxy(string className, IUnityContainer container, Assembly assembly) : base(className, container, assembly) { }

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