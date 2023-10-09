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
    [BsonRepresentation(BsonType.String)]
    [BsonElement("status")]
    [RegularExpression("^(ACTIVE|ARCHIVE)$", ErrorMessage = "Valid values for 'status' are 'ACTIVE,' 'ARCHIVE,'")]
    public string Status { get; set; } = "ACTIVE"; // Default value set to "ACTIVE"



    [JsonPropertyName("scheduleId")]
    public List<string> ScheduleId { get; set; } // Change the data type to List<string> and use nullable List<string>



    [JsonPropertyName("seatCount")] // New property "seatCount"
    public int SeatCount { get; set; }


    // Add a constructor to initialize the ScheduleId property as null
    public Train()
    {
        ScheduleId = null;
    }



}