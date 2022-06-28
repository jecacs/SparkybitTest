using MongoDB.Bson.Serialization.Attributes;

namespace SparkybitTest.Adapters.MongoDb.Models;

public class UserMongoModel
{
    public const string COLLECTION_NAME = "Users";

    [BsonElement("id"), BsonId]
    public Guid Id { get; set; }

    [BsonElement("name"), ]
    public string Name { get; set; }
    
    [BsonElement("createdAt"), ]
    public DateTime CreatedAt { get; set; }
}