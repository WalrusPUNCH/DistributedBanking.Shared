using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Data.Converters;

namespace Shared.Data.Entities;

public class BaseEntity
{
    [BsonId]
    [JsonConverter(typeof(ObjectIdJsonConverter)), Newtonsoft.Json.JsonConverter(typeof(NewtonsoftObjectIdJsonConverter)), JsonPropertyName("_id")]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
}