using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client
{
    public class AllRequiredConverter<T> : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var targetProperties = objectType.GetProperties();
            var sourceProperties = jObject.Properties();

            //TODO: Add attribute for overriding
            //TODO: Recursively down in properties (why is this not provided in application?)
            var unmappedProperties = targetProperties.Select(t => t.Name).Except(sourceProperties.Select(s => s.Name)).ToArray();
            if (unmappedProperties.Any())
            {
                throw new ApplicationException(
                    String.Format("Target type [{1}] contains unmatched properties [{0}]", string.Join(" : ", unmappedProperties), objectType.Name));
            }

            var target = Activator.CreateInstance(objectType);
            serializer.Populate(jObject.CreateReader(), target);
            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) { throw new NotImplementedException(); }
        public override bool CanConvert(Type objectType) { return typeof(T).IsAssignableFrom(objectType); }
        public override bool CanRead { get { return true; } }
        public override bool CanWrite { get { return false; }}
    }
}