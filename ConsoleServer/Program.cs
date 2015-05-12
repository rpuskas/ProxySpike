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
                var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new FooService().Get()));
                response.OutputStream.Write(bytes, 0, bytes.Length);
                response.OutputStream.Close();
            }
        }
    }
}
