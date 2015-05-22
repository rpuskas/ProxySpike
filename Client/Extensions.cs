using System;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;
using Newtonsoft.Json;

namespace Client
{
    public static class Extensions
    {
    
        public static T MapTo<T>(this object value)
        {
            var serializeObject = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject<T>(serializeObject,new AllRequiredConverter<T>());
        }

        public static object MapTo(this object value, Type t)
        {
            var serializeObject = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject(serializeObject, t);
        }
    }
}