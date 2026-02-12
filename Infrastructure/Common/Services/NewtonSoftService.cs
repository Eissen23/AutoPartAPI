using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Infrastructure.Common.Services;

public class NewtonSoftService : ISerializerService
{
    public T Deserialize<T>(string text)
    {
        var result = JsonConvert.DeserializeObject<T>(text);
        return result is null ? throw new InvalidOperationException($"Failed to deserialize to type {typeof(T).Name}") : result;
    }

    public string Serialize<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Converters = [
                new StringEnumConverter() { NamingStrategy = new CamelCaseNamingStrategy() }
            ]
        });
    }

    public string Serialize<T>(T obj, Type type)
    {
        return JsonConvert.SerializeObject(obj, type, new());
    }
}
