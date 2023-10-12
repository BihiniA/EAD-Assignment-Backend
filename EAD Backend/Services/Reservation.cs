using MongoDBExample.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System;
using EAD_Backend.Models;
using EAD_Backend.NewFolder;
using EAD_Backend.Dto;
using System.Diagnostics;
using System.Text.Json;
using System.Xml.Linq;

public class ReservationService
{
    private readonly IMongoCollection<Reservation> _reservationCollection;
    private readonly TrainScheduleService _trainScheduleService;


    public ReservationService(IOptions<MongoDBSettings> mongoDBSettings, TrainScheduleService trainScheduleService)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _reservationCollection = database.GetCollection<Reservation>("reservations");
        _trainScheduleService = trainScheduleService;
    }

    public async Task<List<ReservationsWithSchedule>> GetAllAsync()
    {
        try
        {
            var reservations = await _reservationCollection.Find(_ => true).ToListAsync();

            var reservationsWithSchedule = new List<ReservationsWithSchedule>();

            foreach (var reservation in reservations)
            {
                var trainSchedule = await _trainScheduleService.GetByIdAsync(reservation.TrainScheduleid);

                if (trainSchedule != null)
                {
                    var reservationWithSchedule = new ReservationsWithSchedule
                    {
                        _id = reservation._id,
                        TrainScheduleid = reservation.TrainScheduleid,
                        nic = reservation.nic,
                        CreatedAt = reservation.CreatedAt,
                        UpdatedAt = reservation.UpdatedAt,
                        ReservationDate = reservation.ReservationDate,
                        ReserveCount = reservation.ReserveCount,
                        Departure = trainSchedule.Departure,
                        Destination = trainSchedule.Destination,
                        Status = reservation.Status
                    };

                    reservationsWithSchedule.Add(reservationWithSchedule);
                }
            }

            return reservationsWithSchedule;
        }
        catch (Exception)
        {
            // Handle or log the exception here
            throw;
        }
    }




    public async Task<ReservationsWithSchedule> GetByIdAsync(string id)
    {
        try
        {
            var reservation = await _reservationCollection.Find(r => r._id == id).FirstAsync();
            var reservationsWithSchedule = new List<ReservationsWithSchedule>();

            var trainSchedule = await _trainScheduleService.GetByIdAsync(reservation.TrainScheduleid);

            if (trainSchedule != null)
            {
                var reservationWithSchedule = new ReservationsWithSchedule
                {
                    _id = reservation._id,
                    TrainScheduleid = reservation.TrainScheduleid,
                    nic = reservation.nic,
                    CreatedAt = reservation.CreatedAt,
                    UpdatedAt = reservation.UpdatedAt,
                    ReservationDate = reservation.ReservationDate,
                    ReserveCount = reservation.ReserveCount,
                    Departure = trainSchedule.Departure,
                    Destination = trainSchedule.Destination,
                    Status = reservation.Status
                };

                reservationsWithSchedule.Add(reservationWithSchedule);
            }

            return reservationsWithSchedule[0];
        }
        catch (Exception)
        {
            // Handle or log the exception here
            throw;
        }
    }


    public async Task<List<Reservation>> GetByUserIdAsync(string userId)
    {
        try
        {
            var reservations = await _reservationCollection.FindAsync(r => r.nic == userId);
            return await reservations.ToListAsync();
        }
        catch (Exception)
        {
            // Handle or log the exception here
            throw;
        }
    }

    public async Task<CreateReservationDto> CreateAsync(CreateReservationDto reservation)
    {
        try
        {
            if ( reservation.ReserveCount > 4) {
                throw new InvalidOperationException("one user can only reserve 0-4 seats");
            }

            Reservation res = new Reservation
            {
                nic = reservation.nic,
                ReservationDate = reservation.ReservationDate.ToString(),
                CreatedAt = DateTime.Now.ToString(),
                UpdatedAt = DateTime.UtcNow.ToString(),
                Status = StatusEnum.ACTIVE,
                ReserveCount = reservation.ReserveCount,
                TrainScheduleid = reservation.TrainScheduleId
            };
            

            string id = reservation.TrainScheduleId;

            var trainSchedule = await _trainScheduleService.GetByIdAsync(id);

            if (trainSchedule.AvailableSeatCount < reservation.ReserveCount)
            {
                throw new InvalidOperationException("Requested seat count is not available");
            }

            var updatedSeatCount = trainSchedule.AvailableSeatCount - reservation.ReserveCount;

            UpdateTrainScheduleSeatCountDto updateTrainScheduleSeatCountDto = new UpdateTrainScheduleSeatCountDto
            {
                seatCount = updatedSeatCount
            };

            // Wait until the UpdateTrainScheduleTicketCount method completes before returning the reservation object.
            await _trainScheduleService.UpdateTrainScheduleSeatCount(id, updateTrainScheduleSeatCountDto);

            await _reservationCollection.InsertOneAsync(res);
            return reservation;
        }
        catch (Exception)
        {
            // Handle or log the exception here
            throw;
        }
    }

    public async Task<Reservation> UpdateAsync(string id, Reservation updatedReservation)
    {
        try
        {
            var filter = Builders<Reservation>.Filter.Eq(r => r._id, id);
            var update = Builders<Reservation>.Update
                .Set(r => r.TrainScheduleid, updatedReservation.TrainScheduleid)
                .Set(r => r.nic, updatedReservation.nic)
                .Set(r => r.CreatedAt, updatedReservation.CreatedAt)
                .Set(r => r.UpdatedAt, updatedReservation.UpdatedAt)
                .Set(r => r.ReservationDate, updatedReservation.ReservationDate)
                .Set(r => r.ReserveCount, updatedReservation.ReserveCount)
                .Set(r => r.Status, updatedReservation.Status);

            await _reservationCollection.UpdateOneAsync(filter, update);
            return updatedReservation;
        }
        catch (Exception)
        {
            // Handle or log the exception here
            throw;
        }
    }


    public async Task<bool> DeleteById(string id)
    {
        try
        {
            var result = await _reservationCollection.DeleteOneAsync(t => t._id == id);
            return result.DeletedCount > 0;
        }
        catch (Exception)
        {
            // Handle or log the exception here
            throw;
        }
    }

    //public async Task<List<Reservation>> SearchAsync(ReservationSearchModel searchModel)
    //{
    //    try
    //    {
    //        var filterBuilder = Builders<Reservation>.Filter;
    //        var filter = filterBuilder.Empty; // Initialize an empty filter

    //        if (!string.IsNullOrEmpty(searchModel.TrainScheduleid))
    //        {
    //            filter &= filterBuilder.Eq(r => r.TrainScheduleid, searchModel.TrainScheduleid);
    //        }

    //        if (!string.IsNullOrEmpty(searchModel.nic))
    //        {
    //            filter &= filterBuilder.Eq(r => r.nic, searchModel.nic);
    //        }

    //        if (searchModel.ReservationDate.HasValue)
    //        {
    //            filter &= filterBuilder.Eq(r => r.ReservationDate, searchModel.ReservationDate);
    //        }

    //        if (searchModel.ReserveCount.HasValue)
    //        {
    //            filter &= filterBuilder.Eq(r => r.ReserveCount, searchModel.ReserveCount);
    //        }

    //        if (searchModel.Status.HasValue)
    //        {
    //            filter &= filterBuilder.Eq(r => r.Status, searchModel.Status);
    //        }

    //        var reservations = await _reservationCollection.FindAsync(filter);
    //        return await reservations.ToListAsync();
    //    }
    //    catch (Exception)
    //    {
    //        // Handle or log the exception here
    //        throw;
    //    }
    //}

}
