using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace Client
{
    public class FooServiceHttpProxy : IFooService
    {
        private readonly string _httpLocalhost;
        private readonly HttpClient _httpClient;

        public FooServiceHttpProxy(string httpLocalHost)
        {
            _httpLocalhost = httpLocalHost;
            _httpClient = new HttpClient();
        }

        public BarResult Get(int id)
        {
            var httpResponseMessage = _httpClient.GetAsync(new Uri(new Uri(_httpLocalhost),
                string.Format("Get?id={0}",id))).Result;
            var response = httpResponseMessage.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<BarResult>(response);
        }

        public IEnumerable<BarResult> Search(BarQuery query)
        {
            var httpResponseMessage = _httpClient.GetAsync(new Uri(new Uri(_httpLocalhost),
                string.Format("Search?name={0}", query.Name))).Result;
            var response = httpResponseMessage.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<IEnumerable<BarResult>>(response);
        }
    }
}