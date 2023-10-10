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

    [JsonPropertyName("trainId")]
    public string TrainId { get; set; } 

    [JsonPropertyName("departure")]
    public string Departure { get; set; }

    [JsonPropertyName("destination")]
    public string Destination { get; set; }

    [JsonPropertyName("startTime")]
    public string StartTime { get; set; }

    [JsonPropertyName("endTime")]
    public string EndTime { get; set; }

    [JsonPropertyName("scheduleDate")]
    public DateTime ScheduleDate { get; set; }

    [JsonPropertyName("availableSeatCount")]
    public int AvailableSeatCount { get; set; }

    [JsonPropertyName("status")]
    [BsonRepresentation(BsonType.String)]
    [BsonElement("status")]
    [EnumDataType(typeof(StatusEnum))]
    public StatusEnum Status { get; set; } = StatusEnum.ACTIVE; // Default value set to "ACTIVE"
}