using MongoDB.Driver;

namespace Shared.Data.Services;

public interface IMongoDbFactory
{
    IMongoDatabase GetDatabase();
    IMongoClient GetClient();
}
