using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using LibraryServer;
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

			Console.WriteLine ("Staring on port: 8081");

            while (true)
            {
                var context = httpListener.GetContext();
                var request = context.Request;
                var httpMethod = request.Url.LocalPath;
                var response = context.Response;

                Func<Object> action;
                NameValueCollection nameValueCollection;
                switch (httpMethod)
                {
                    case "/Get":
                        nameValueCollection = request.QueryString;
                        action = () => new FooService(new FooRepository()).Get(int.Parse(nameValueCollection["id"]));
                        break;
                    case "/Search":
                        nameValueCollection = request.QueryString;
                        var fooQuery = new FooService.FooQuery { Name = nameValueCollection["name"] };
                        action = () => new FooService(new FooRepository()).Search(fooQuery);
                        break;
                    default:
                        action = () => null;
                        break;
                }

                response.ContentType = "application/json";
                response.AppendHeader("Access-Control-Allow-Origin", "*");
                var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(action.Invoke()));
                response.OutputStream.Write(bytes, 0, bytes.Length);
                response.OutputStream.Close();
            }
        }

       
    }
}
