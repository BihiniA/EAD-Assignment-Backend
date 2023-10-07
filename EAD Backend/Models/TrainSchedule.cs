using EAD_Backend.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


[BsonIgnoreExtraElements]
public class TrainSchedule  //Train class 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("_id")]
    public string _id { get; set; }

    [JsonPropertyName("train_id")]
    public string TrainId { get; set; } = null!;

    [JsonPropertyName("departure")]
    public string Departure { get; set; }

    [JsonPropertyName("destination")]
    public string Destination { get; set; }

    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; set; }

    [JsonPropertyName("endTime")]
    public DateTime EndTime { get; set; }

    [JsonPropertyName("status")]
    [BsonRepresentation(BsonType.String)]
    [BsonElement("status")]
    [EnumDataType(typeof(StatusEnum))]
    public StatusEnum Status { get; set; } = StatusEnum.ACTIVE; // Default value set to "ACTIVE"

    //[JsonPropertyName("train")]
    //[BsonIgnore]
    //public Train Train { get; set; } // Property to store the associated Train

    // Add a constructor to initialize Train property as null
    // public TrainSchedule()
    //{
        //Train = null;
    //}
}