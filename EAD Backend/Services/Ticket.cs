using MongoDBExample.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

public class TicketService
{
    private readonly IMongoCollection<Ticket> _ticketCollection;

    public TicketService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _ticketCollection = database.GetCollection<Ticket>("tickets");
    }

    public async Task<List<Ticket>> GetAllAsync()
    {
        try
        {
            var tickets = await _ticketCollection.FindAsync(_ => true);
            return await tickets.ToListAsync();
        }
        catch (Exception)
        {
            // Handle or log the exception here
            throw;
        }
    }

    public async Task<List<Ticket>> GetAllByUserIdAsync(string userId)
    {
        try
        {
            var tickets = await _ticketCollection.FindAsync(t => t.ReservationId == userId);
            return await tickets.ToListAsync();
        }
        catch (Exception)
        {
            // Handle or log the exception here
            throw;
        }
    }

    public async Task<Ticket> CreateAsync(Ticket ticket)
    {
        try
        {
            await _ticketCollection.InsertOneAsync(ticket);
            return ticket;
        }
        catch (Exception)
        {
            // Handle or log the exception here
            throw;
        }
    }

    public async Task<Ticket> UpdateAsync(Ticket ticket)
    {
        try
        {
            var filter = Builders<Ticket>.Filter.Eq(t => t._id, ticket._id);
            var update = Builders<Ticket>.Update
                .Set(t => t.ReservationId, ticket.ReservationId)
                .Set(t => t.Status, ticket.Status);

            var options = new FindOneAndUpdateOptions<Ticket>
            {
                ReturnDocument = ReturnDocument.After
            };

            return await _ticketCollection.FindOneAndUpdateAsync(filter, update, options);
        }
        catch (Exception)
        {
            // Handle or log the exception here
            throw;
        }
    }
}
