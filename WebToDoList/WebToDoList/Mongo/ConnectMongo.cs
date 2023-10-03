using MongoDB.Driver;
using System;
using WebToDoList.Model;

namespace WebToDoList.Mongo
{
    public class ConnectMongo
    {
        private IMongoClient _client;
        public ConnectMongo(string connectionString)
        {
            _client = new MongoClient(connectionString);
        }

        public IMongoCollection<TaskToDoList> GetCollection(string databaseName, string collectionName)
        {
            var database = _client.GetDatabase(databaseName);
            var collection = database.GetCollection<TaskToDoList>(collectionName);
            return collection;
        }
    }
}