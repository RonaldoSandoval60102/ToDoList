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

var collectionSettings = new InsertOneOptions
{
    BypassDocumentValidation = false
};

app.MapPost("/api/tasks", async ([FromBody] TaskToDoList task) =>
{
    try
    {
        await collection.InsertOneAsync(task, collectionSettings);
        return Results.Ok(task);
    }
    catch (ToDoListException ex)
    {
        return Results.BadRequest(new { message = ex.GetBaseException().Message });
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
        var filter = Builders<TaskToDoList>.Filter.Eq("Id", id);
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

        var filter = Builders<TaskToDoList>.Filter.Eq("Id", id);
        var task = await collection.Find(filter).FirstOrDefaultAsync();

        if (task == null)
        {
            throw new ToDoListException(ExceptionToDoList.TaskNotFound);
        }

        await collection.DeleteOneAsync(filter);

        return Results.Ok(task);
    }
    catch (ToDoListException ex)
    {
        return Results.BadRequest(ex.GetBaseException().Message);
    }
});

app.MapPut("/api/tasks/{id}", async ([FromRoute] string id, [FromBody] TaskToDoList task) =>
{
    try
    {
        var filter = Builders<TaskToDoList>.Filter.Eq("Id", id);
        var taskToUpdate = await collection.Find(filter).FirstOrDefaultAsync();

        if (taskToUpdate == null)
        {
            throw new ToDoListException(ExceptionToDoList.TaskNotFound);
        }

        taskToUpdate.Title = task.Title;
        taskToUpdate.IsDone = task.IsDone;

        await collection.ReplaceOneAsync(filter, taskToUpdate);

        return Results.Ok(taskToUpdate);
    }
    catch (ToDoListException ex)
    {
        return Results.BadRequest(ex.GetBaseException().Message);
    }
});

app.MapPatch("/api/tasks/{id}", async ([FromRoute] string id, [FromBody] TaskToDoList task) =>
{
    try
    {
        var filter = Builders<TaskToDoList>.Filter.Eq("Id", id);
        var taskToUpdate = await collection.Find(filter).FirstOrDefaultAsync();

        if (taskToUpdate == null)
        {
            throw new ToDoListException(ExceptionToDoList.TaskNotFound);
        }

        taskToUpdate.IsDone = task.IsDone;

        await collection.ReplaceOneAsync(filter, taskToUpdate);

        return Results.Ok(taskToUpdate);
    }
    catch (ToDoListException ex)
    {
        return Results.BadRequest(ex.GetBaseException().Message);
    }
});

app.Run();
