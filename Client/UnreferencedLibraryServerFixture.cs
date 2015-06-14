using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace Client
{
    [TestFixture]
    public class UnreferencedLibraryServerFixture
    {
        protected IFooService Service;

        [SetUp]
        public void Setup()
        {
            var assembly = Assembly.LoadFrom(Path.GetFullPath(@"../../../LibraryServer/bin/Debug/LibraryServer.dll"));
            Service = new FooServiceClassProxy("LibraryServer.FooService", new UnityContainer(), assembly);
        }

        [Test]
        public void Should_Invoke_With_Primitive_Type_Parameter()
        {
            Assert.AreEqual("One", Service.Get(1).Name);
            Assert.AreEqual("Two", Service.Get(2).Name);
        }

        [Test]
        public void Should_Invoke_With_Comple_Type_Parameters()
        {
            Assert.AreEqual("One", Service.Search(new BarQuery { Name = "One" }).Single().Name);
            Assert.AreEqual("Two", Service.Search(new BarQuery { Name = "Two" }).Single().Name);
            Assert.AreEqual(2, Service.Search(new BarQuery { Name = "Twins" }).Count());
        }
    }
}
