using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:8081/");
            httpListener.Start();

            while (true)
            {
                var context = httpListener.GetContext();
                var response = context.Response;
                response.ContentType = "application/json";
                response.AppendHeader("Access-Control-Allow-Origin", "*");
                var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Foo.FooResult));
                response.OutputStream.Write(bytes, 0, bytes.Length);
                response.OutputStream.Close();
            }
        }
    }

    public class Foo
    {
        public static FooResult FooResult = new FooResult("FooResult", 1);
    }

    public class FooResult
    {
        public FooResult(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; set; }
        public int Age { get; set; }
    }
}
