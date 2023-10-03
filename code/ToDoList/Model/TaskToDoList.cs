using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebToDoList.Model
{
    public class TaskToDoList
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [Required]
        public required string Title { get; set; }
    }
}
