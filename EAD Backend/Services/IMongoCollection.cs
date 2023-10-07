using MongoDB.Driver;

namespace MongoDBExample.Services
{
    internal interface IMongoCollection
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
    }
}