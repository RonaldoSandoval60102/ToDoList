using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebToDoList.Model
{
    public class TaskToDoList
    {
        [BsonId]
        public string Id { get; set; }
        public string Title { get; set; }

        public bool IsDone { get; set; }

        public TaskToDoList(string title)
        {
            Id = ObjectId.GenerateNewId().ToString();
            Title = title;
            IsDone = false;
        }
    }
}
