using EAD_Backend.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;





[BsonIgnoreExtraElements]
public class Train  //Train class 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("_id")]
    public string _id { get; set; }



    [JsonPropertyName("status")]
    public StatusEnum Status { get; set; } // Default value set to "ACTIVE"



    [JsonPropertyName("scheduleId")]
    public List<string> ScheduleId { get; set; } // Change the data type to List<string> and use nullable List<string>

    
    [JsonPropertyName("trainName")]
    public string TrainName { get; set; }



    [JsonPropertyName("seatCount")] // New property "seatCount"
    public int SeatCount { get; set; }


    // Add a constructor to initialize the ScheduleId property as null
    public Train()
    {
        ScheduleId = null;
    }



}