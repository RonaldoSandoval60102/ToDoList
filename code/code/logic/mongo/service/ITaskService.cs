using code.logic.model;
using code.logic.mongo.settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace code.logic.mongo.service
{
    public class ITaskService
    {
        private readonly IMongoCollection<ToDoListTask> _tasks;

        public ITaskService(IOptions<DatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            _tasks = database.GetCollection<ToDoListTask>(settings.Value.TaskCollectionName);
        }

        public List<ToDoListTask> Get() =>
            _tasks.Find(task => true).ToList();

        public ToDoListTask Get(string id) =>
            _tasks.Find<ToDoListTask>(task => task.Id == id).FirstOrDefault();

        public ToDoListTask Create(ToDoListTask task)
        {
            _tasks.InsertOne(task);
            return task;
        }

        public void Update(string id, ToDoListTask taskIn) =>
            _tasks.ReplaceOne(task => task.Id == id, taskIn);
        public void Remove(string id) =>
            _tasks.DeleteOne(task => task.Id == id);

        public void Patch(string id, ToDoListTask taskIn)
        {
            var filter = Builders<ToDoListTask>.Filter.Eq(s => s.Id, id);
            var update = Builders<ToDoListTask>.Update.Set(s => s.IsDone, taskIn.IsDone);
            _tasks.UpdateOne(filter, update);
        }
    }
}