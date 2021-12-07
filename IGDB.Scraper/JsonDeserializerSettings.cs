using System.Collections.Generic;
using IGDB.Model.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IGDB.Scraper; 

public static class JsonDeserializerSettings {
    public static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings {
        Converters = new List<JsonConverter> {
            new IdentityConverter(),
            new UnixTimestampConverter()
        },
        ContractResolver = new DefaultContractResolver {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };
}