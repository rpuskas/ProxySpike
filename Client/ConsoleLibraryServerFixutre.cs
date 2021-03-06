using System.Linq;
using NUnit.Framework;

namespace Client
{
    [TestFixture]
    public class ConsoleLibraryServerFixutre
    {
        protected IFooService Service;

        [SetUp]
        public void Setup()
        {
            Service = new FooServiceHttpProxy("http://localhost:8081");
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