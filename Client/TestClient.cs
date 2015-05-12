using System;
using System.Net.Http;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Client
{
    [TestFixture]
    public class TestClient
    {
        [Test]
        public void ShouldGetDataFromUnreferencedLibraryServer()
        {
            var barResult = new LibraryClient().Get();
            Assert.AreEqual(barResult.Name,"FooResult");
            Assert.AreEqual(barResult.Age,1);
        }

        [Test]
        public void ShouldGetDataFromUnreferencedConsoleServer()
        {
            var barResult = new ConsoleClient().Get();
            Assert.AreEqual(barResult.Name, "FooResult");
            Assert.AreEqual(barResult.Age, 1);
        }
    }

    public interface IProxy
    {
        BarResult Get();
    }

    public class LibraryClient : IProxy
    {
        private const string Path = @"c:\users\rpuskas\documents\visual studio 2013\Projects\ProxySpike\LibraryServer\bin\Debug\LibraryServer.dll";

        public BarResult Get()
        {
            var result = Result("LibraryServer.Foo", "Get", null);
            var serializeObject = JsonConvert.SerializeObject(result);
            return JsonConvert.DeserializeObject<BarResult>(serializeObject);
        }

        private static object Result(string libraryserverFoo, string name, object[] objects)
        {
            var assembly = Assembly.LoadFile(Path);
            var type = assembly.GetType(libraryserverFoo);
            var instance = Activator.CreateInstance(type);
            return type.InvokeMember(name, BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, instance, objects);
        }
    }

    public class ConsoleClient : IProxy
    {
        public BarResult Get()
        {
            var httpClient = new HttpClient();
            var httpResponseMessage = httpClient.GetAsync("http://localhost:8081").Result;
            var response = httpResponseMessage.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<BarResult>(response);
        }
    }

    public class BarResult
    {
        public object Name { get; set; }
        public int Age { get; set; }
    }
}
