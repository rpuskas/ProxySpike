using System;
using Newtonsoft.Json;

namespace Client
{
    public static class Extensions
    {
    
        public static T MapTo<T>(this object value)
        {
            var serializeObject = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject<T>(serializeObject);
        }

        public static object MapTo(this object value, Type t)
        {
            var serializeObject = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject(serializeObject, t);
        }
    }
}