using MongoDBExample.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using EAD_Backend.Dto;
using EAD_Backend.Models;

public class TrainScheduleService
{
    private readonly IMongoCollection<TrainSchedule> _trainScheduleCollection;
    private readonly TrainService _trainService;

    public TrainScheduleService(IOptions<MongoDBSettings> mongoDBSettings, TrainService trainService)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _trainScheduleCollection = database.GetCollection<TrainSchedule>("trainschedules");
        _trainService = trainService;
    }

    public async Task<List<TrainSchedule>> GetAsync()
    {
        try
        {
            var trainSchedules = await _trainScheduleCollection.FindAsync(_ => true);
            return await trainSchedules.ToListAsync();
        }
        catch (Exception)
        {
            // Handle or log the exception here
            throw;
        }
    }

    public async Task<TrainSchedule> CreateAsync(CreateTrainScheduleDto createTrainScheduleDto)
    {
        try
        {
            string trainId = createTrainScheduleDto.TrainId.ToString();
            var trainObj = await _trainService.GetByIdAsync(trainId);

            TrainSchedule trainSchedule = new TrainSchedule();

            trainSchedule.StartTime = createTrainScheduleDto.StartTime;
            trainSchedule.EndTime = createTrainScheduleDto.EndTime;
            trainSchedule.Destination = createTrainScheduleDto.Destination;
            trainSchedule.Departure = createTrainScheduleDto.Departure;
            trainSchedule.Status = StatusEnum.ACTIVE;
            trainSchedule.AvailableSeatCount = trainObj.SeatCount;
            trainSchedule.TrainId = createTrainScheduleDto.TrainId;

            await _trainScheduleCollection.InsertOneAsync(trainSchedule);
            return trainSchedule;
        }
        catch (Exception)
        {
            // Handle or log the exception here
            throw;
        }
    }

    public async Task<TrainSchedule> GetByIdAsync(string id)
    {
        try
        {
            var trainSchedule = await _trainScheduleCollection.FindAsync(t => t._id == id);
            return await trainSchedule.FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            // Handle or log the exception here
            throw;
        }
    }

    public async Task UpdateTrainSchedule(string id, TrainSchedule updatedTrainSchedule)
    {
        try
        {
            var filter = Builders<TrainSchedule>.Filter.Eq(t => t._id, id);
            var update = Builders<TrainSchedule>.Update
                .Set(t => t.TrainId, updatedTrainSchedule.TrainId)
                .Set(t => t.Departure, updatedTrainSchedule.Departure)
                .Set(t => t.Destination, updatedTrainSchedule.Destination)
                .Set(t => t.StartTime, updatedTrainSchedule.StartTime)
                .Set(t => t.EndTime, updatedTrainSchedule.EndTime)
                .Set(t => t.Status, updatedTrainSchedule.Status);

            await _trainScheduleCollection.UpdateOneAsync(filter, update);
        }
        catch (Exception)
        {
            // Handle or log the exception here
            throw;
        }
    }

    public async Task<bool> DeleteAsync(string id)
    {
        try
        {
            var result = await _trainScheduleCollection.DeleteOneAsync(t => t._id == id);
            return result.DeletedCount > 0;
        }
        catch (Exception)
        {
            // Handle or log the exception here
            throw;
        }
    }

    public async Task<TrainSchedule> UpdateTrainScheduleSeatCount(string id, UpdateTrainScheduleSeatCountDto updateTrainScheduleSeatCountDto)
    {
        try
        {
            var filter = Builders<TrainSchedule>.Filter.Eq(t => t._id, id);
            var update = Builders<TrainSchedule>.Update.Set(t => t.AvailableSeatCount, updateTrainScheduleSeatCountDto.seatCount);

            var options = new FindOneAndUpdateOptions<TrainSchedule>
            {
                ReturnDocument = ReturnDocument.After,
            };

            var updatedTrainSchedule = await _trainScheduleCollection.FindOneAndUpdateAsync(filter, update);
            return updatedTrainSchedule;
        }
        catch (Exception )
        {
            throw ;
        }
    }
}
