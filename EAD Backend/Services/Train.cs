using MongoDBExample.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using EAD_Backend.Models;

public class TrainService
{
    private readonly IMongoCollection<Train> _trainCollection;

    public TrainService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _trainCollection = database.GetCollection<Train>("trains");
    }

    public async Task<List<Train>> GetAsync()
    {
        try
        {
            return await _trainCollection.Find(_ => true).ToListAsync();
        }
        catch (Exception)
        {
            // Handle the exception, log it, or throw a custom exception if needed.
            throw;
        }
    }

    public async Task<Train> CreateAsync(Train train)
    {
        try
        {
            // Initialize ScheduleId as an empty list if it's null
            if (train.ScheduleId == null)
            {
                train.ScheduleId = new List<string>();
            }

            // You can add validation or business logic here before inserting into the database
            await _trainCollection.InsertOneAsync(train);
            return train;
        }
        catch (Exception)
        {
            // Handle the exception, log it, or throw a custom exception if needed.
            throw;
        }
    }

    public async Task<Train> GetByIdAsync(string id)
    {
        try
        {
            return await _trainCollection.Find(train => train._id == id).FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            // Handle the exception, log it, or throw a custom exception if needed.
            throw;
        }
    }

    public async Task UpdateTrain(string id, Train train)
    {
        try
        {
            // You can add validation or business logic here before updating the train
            await _trainCollection.ReplaceOneAsync(t => t._id == id, train);
        }
        catch (Exception)
        {
            // Handle the exception, log it, or throw a custom exception if needed.
            throw;
        }
    }

    public async Task UpdateStatus(string id, StatusEnum status)
    {
        try
        {
            // You can add validation or business logic here before updating the status
            await _trainCollection.UpdateOneAsync(t => t._id == id, Builders<Train>.Update.Set(t => t.Status, status));
        }
        catch (Exception)
        {
            // Handle the exception, log it, or throw a custom exception if needed.
            throw;
        }
    }


    public void ConfigureServices(IServiceCollection services)
    {
        // ... other service registrations ...

        // Register the TrainService
        services.AddSingleton<TrainService>();

        // ... other service registrations ...
    }

    public async Task<Train> GetByName(string name)
    {
        try
        {
            return await _trainCollection.Find(train => train.TrainName == name).FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            // Handle the exception, log it, or throw a custom exception if needed.
            throw;
        }
    }

    public async Task<bool> DeleteAsync(string id)
    {
        try
        {
            var result = await _trainCollection.DeleteOneAsync(t => t._id == id);
            return result.DeletedCount > 0;
        }
        catch (Exception)
        {
            // Handle or log the exception here
            throw;
        }
    }


}
