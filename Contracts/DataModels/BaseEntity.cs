using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Tools;

namespace Contracts.DataModels;

public abstract class BaseEntity
{
    [BsonId]
    [JsonConverter(typeof(ObjectIdJsonConverter)), JsonPropertyName("_id"), BsonElement(nameof(Id))]
    public ObjectId Id { get; set; }
}