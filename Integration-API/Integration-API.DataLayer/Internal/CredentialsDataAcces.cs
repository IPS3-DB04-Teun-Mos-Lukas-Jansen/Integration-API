using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Integration_API.DataLayer.Internal
{
    public class CredentialsDataAcces : ICredentialsDataAcces
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<BsonDocument> _credentialsCollection;

        public CredentialsDataAcces(string dbUrl)
        {
            _client = new MongoClient(dbUrl);
            _database = _client.GetDatabase("credentials");
            _credentialsCollection = _database.GetCollection<BsonDocument>("credentials-collection");
        }


        public async Task<BsonValue> GetCredentials(string userId, string integration)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("userId", userId);
            BsonDocument result = await _credentialsCollection.Find(filter).FirstOrDefaultAsync();
            return result["integrations"][integration];
        }

        public async Task SetCredentials(string userId, string integration, BsonDocument credentials)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("userId", userId);
            var options = new UpdateOptions { IsUpsert = true };
            await _credentialsCollection.UpdateOneAsync(filter, new BsonDocument("$set", new BsonDocument("integrations." + integration, credentials)), options);
        }



    }
}
