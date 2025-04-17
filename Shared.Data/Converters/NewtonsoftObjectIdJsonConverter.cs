using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Shared.Data.Converters;

public class NewtonsoftObjectIdJsonConverter : JsonConverter<ObjectId>
{
    public override ObjectId ReadJson(JsonReader reader, Type objectType, ObjectId existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var value = JToken.Load(reader).ToString();
        return new ObjectId(value);
    }

    public override void WriteJson(JsonWriter writer, ObjectId value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }
}