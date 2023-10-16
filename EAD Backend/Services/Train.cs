using MongoDBExample.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using EAD_Backend.Models;
using EAD_Backend.Services;
using System.Diagnostics;
using MongoDB.Bson;
using Microsoft.VisualBasic;

public class TrainService
{
    private readonly IMongoCollection<Train> _trainCollection;
    private readonly IMongoCollection<TrainSchedule> _trainScheduleCollection;
    private readonly IMongoCollection<Reservation> _reservationCollection;
    //private readonly TrainScheduleService _trainScheduleService;
    //private readonly ReservationService _reservationService;

    public TrainService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _trainCollection = database.GetCollection<Train>("trains");
        _trainScheduleCollection = database.GetCollection<TrainSchedule>("trainschedules");
        _reservationCollection = database.GetCollection<Reservation>("reservations");
        //_trainScheduleService = trainScheduleService;
        //_reservationService = reservationService;
    }

    public async Task<List<Train>> GetAsync()
    {
        try
        {
            return await _trainCollection.Find(_ => true).ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Train> CreateAsync(Train train)
    {
        try
        {
            if (train.ScheduleId == null)
            {
                train.ScheduleId = new List<string>();
            }

            await _trainCollection.InsertOneAsync(train);
            return train;
        }
        catch (Exception)
        {
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
            throw;
        }
    }

    public async Task UpdateTrain(string id, Train train)
    {
        try
        {
            await _trainCollection.ReplaceOneAsync(t => t._id == id, train);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task UpdateStatus(string id, StatusEnum status)
    {
        try
        {
            await _trainCollection.UpdateOneAsync(t => t._id == id, Builders<Train>.Update.Set(t => t.Status, status));
        }
        catch (Exception)
        {
            throw;
        }
    }


    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<TrainService>();

    }

    public async Task<Train> GetByName(string name)
    {
        try
        {
            return await _trainCollection.Find(train => train.TrainName == name).FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> DeleteAsync(string id)
    {
        try
        {
            var trainSchedules = await _trainScheduleCollection.FindAsync(_ => true);
            var trainScheduleList = new List<string>();
            var test = await trainSchedules.ToListAsync();
            Debug.WriteLine(test.ToArray());

            string[] reservationsIdArray = new string[test.Count];

            int count = 0;

            for (int i = 0; i < test.Count; i++)
            {
                if (test[i].TrainId == id)
                {
                    trainScheduleList.Add(test[i]._id);
                }
            }

            var reservations = await _reservationCollection.Find(_ => true).ToListAsync();

            int trainScheduleArrayCount = 0;
            int reservationsIdArrayCount = 0;

            for (int i = 0; i < reservations.Count; i++)
            {
                if (reservations[i].TrainScheduleid == trainScheduleList[trainScheduleArrayCount])
                {
                    reservationsIdArray[reservationsIdArrayCount] = reservations[i]._id;
                    trainScheduleArrayCount++;
                    reservationsIdArrayCount++;
                }
            }

            for (int i = 0; i < trainScheduleList.Count; i++)
            {
               await _trainScheduleCollection.DeleteOneAsync(t => t._id == trainScheduleList[i]);
            }

            for (int i = 0; i < reservationsIdArrayCount; i++)
            {
               await _reservationCollection.DeleteOneAsync(t => t._id == reservationsIdArray[i]);
            }

            var result = await _trainCollection.DeleteOneAsync(t => t._id == id);
            return result.DeletedCount > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
