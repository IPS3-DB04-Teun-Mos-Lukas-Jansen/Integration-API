using MongoDB.Bson;

namespace Integration_API.DataLayer.Internal
{
    public interface ICredentialsDataAcces
    {
        Task<BsonValue> GetCredentials(string userId, string integration);
        Task SetCredentials(string userId, string integration, BsonDocument credentials);
    }
}