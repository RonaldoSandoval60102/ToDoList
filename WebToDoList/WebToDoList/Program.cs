using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using DotNetEnv;
using WebToDoList.Mongo;
using WebToDoList.Model;
using WebToDoList.Exceptions;
using WebToDoList.enums;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors(builder =>
{
    builder
        .WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

Env.Load();

var mongoConnection = new ConnectMongo(Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING"));
var collection = mongoConnection.GetCollection("ToDoList", "Tasks");

var collectionSettings = new MongoCollectionSettings
{
    AssignIdOnInsert = false 
};

app.MapPost("/api/tasks", async ([FromBody] TaskToDoList task) =>
{
    try
    {
        await collection.InsertOneAsync(task);
        return Results.Ok();
    }
    catch (ToDoListException)
    {
        return Results.BadRequest(new ToDoListException(ExceptionToDoList.BadRequest).GetBaseException().Message);
    }
});

app.MapGet("/api/tasks", async () =>
{
    var tasks = await collection.Find(new BsonDocument()).ToListAsync();
    return Results.Ok(tasks);
});

app.MapGet("/api/tasks/{id}", async ([FromRoute] string id) =>
{
    try
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            throw new ToDoListException(ExceptionToDoList.TaskNotFound);
        }

        var filter = Builders<TaskToDoList>.Filter.Eq("_id", objectId);
        var task = await collection.Find(filter).FirstOrDefaultAsync();

        if (task == null)
        {
            throw new ToDoListException(ExceptionToDoList.TaskNotFound);
        }

        return Results.Ok(task);
    }
    catch (ToDoListException ex)
    {
        return Results.BadRequest(ex.GetBaseException().Message);
    }
});



app.MapDelete("/api/tasks/{id}", async ([FromRoute] string id) =>
{
    try
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            throw new ToDoListException(ExceptionToDoList.TaskNotFound);
        }

        var filter = Builders<TaskToDoList>.Filter.Eq("_id", objectId);
        var result = await collection.DeleteOneAsync(filter);

        if (result.DeletedCount == 0)
        {
            throw new ToDoListException(ExceptionToDoList.NotDeleted);
        }

        return Results.Ok();
    }
    catch (ToDoListException ex)
    {
        return Results.BadRequest(ex.GetBaseException().Message);
    }
});

app.Run();
