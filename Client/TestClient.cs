using System.Linq;
using NUnit.Framework;

namespace Client
{
    [TestFixture]
    public class UnreferencedLibraryServerFixture
    {
        protected IFooServiceProxy Service;

        [SetUp]
        public void Setup()
        {
            Service = new LibraryFooServiceProxy(@"..\..\..\LibraryServer\bin\Debug\LibraryServer.dll", "LibraryServer.FooService");
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

    [TestFixture]
    public class ConsoleLibraryServerFixutre
    {
        protected IFooServiceProxy Service;

        [SetUp]
        public void Setup()
        {
            Service = new HttpProxy("http://localhost:8081");
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
