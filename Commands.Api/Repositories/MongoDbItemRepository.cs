using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Commands.Api.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Commands.Api.Repositories
{
    public class MongoDbItemRepository : IItemRepository
    {
        private const string databaseName = "commands";
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> itemsCollection;

        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;
        
        public MongoDbItemRepository(IMongoClient mongoClient) 
        {
            IMongoDatabase  database = mongoClient.GetDatabase(databaseName);
            itemsCollection = database.GetCollection<Item>(collectionName);           
        }

        public async Task CreateItemAsync(Item item)
        {
            await itemsCollection.InsertOneAsync(item);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id , id);
            await itemsCollection.DeleteOneAsync(filter);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id , id);
            return await itemsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<Item> GetItemByNameAsync(string name)
        {
            var filter = filterBuilder.Eq(item => item.Name, name);
            return await itemsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await itemsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            var filter = filterBuilder.Eq(exsistingItem => exsistingItem.Id ,item.Id);
            await itemsCollection.ReplaceOneAsync(filter, item);

        }
    }
}