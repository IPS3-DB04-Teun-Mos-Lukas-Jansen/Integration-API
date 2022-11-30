using MongoDB.Bson;
using MongoDB.Driver;

namespace Integration_API.DataLayer.Internal
{
    public class CredentialsDataAcces : ICredentialsDataAcces
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<BsonDocument> _credentialsCollection;

        public CredentialsDataAcces(MongoClient db)
        {
            _client = db;
            _database = _client.GetDatabase("credentials");
            _credentialsCollection = _database.GetCollection<BsonDocument>("credentials-collection");
        }


        public async Task<BsonValue> GetCredentials(string userId, string integration)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("userId", userId);
            BsonDocument result = await _credentialsCollection.Find(filter).FirstOrDefaultAsync();
            if (result == null)
            {
                return null;
            }
            else
            {
                return result["integrations"][integration];
            }
        }

        public async Task<BsonValue> GetAllCredentials(string userId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("userId", userId);
            BsonDocument result = await _credentialsCollection.Find(filter).FirstOrDefaultAsync();

            if (result != null)
            {
                return result["integrations"];
            }
            else
            {
                return null;
            }
        }

        public async Task<int> RemoveIntegrationCredentials(string userId, string integration)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("userId", userId);
            var rows = await _credentialsCollection.UpdateOneAsync(filter, new BsonDocument("$unset", new BsonDocument("integrations." + integration,1)));
            return (int)rows.ModifiedCount;
        }


        public async Task SetCredentials(string userId, string integration, BsonDocument credentials)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("userId", userId);
            var options = new UpdateOptions { IsUpsert = true };
            await _credentialsCollection.UpdateOneAsync(filter, new BsonDocument("$set", new BsonDocument("integrations." + integration, credentials)), options);
        }





    }
}
