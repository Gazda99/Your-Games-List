using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IGDB.Model.Serialization {
public class UnixTimestampConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType.IsAssignableFrom(typeof(DateTimeOffset));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
        JsonSerializer serializer) {
        var defaultDateTime = default(DateTimeOffset);

        const long minTimestamp = -62135596800;
        const long maxTimestamp = 253402300799;

        if (reader.TokenType != JsonToken.Null) {
            if (reader.TokenType == JsonToken.Integer) {
                var rawValue = reader.Value.ToString();
                if (long.TryParse(rawValue, out long parsedUnixTimestamp)) {
                    if (parsedUnixTimestamp is <= maxTimestamp and >= minTimestamp) {
                        return DateTimeOffset.FromUnixTimeSeconds(parsedUnixTimestamp);
                    }
                }
            }
        }

        return defaultDateTime;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        var offset = value as DateTimeOffset?;

        if (offset.HasValue) {
            JToken.FromObject(offset.Value.ToUnixTimeSeconds()).WriteTo(writer);
        }
        else {
            JToken.FromObject(value).WriteTo(writer);
        }
    }
}
}