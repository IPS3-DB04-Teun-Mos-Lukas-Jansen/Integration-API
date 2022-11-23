using MongoDB.Bson;

namespace Integration_API.DataLayer.Internal
{
    public interface ICredentialsDataAcces
    {
        Task<BsonValue> GetCredentials(string userId, string integration);
        Task<BsonValue> GetAllCredentials(string userId);
        Task SetCredentials(string userId, string integration, BsonDocument credentials);
        Task<int> RemoveIntegrationCredentials(string userId, string integration);
    }
}