using EAD_Backend.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


[BsonIgnoreExtraElements]
public class Reservation  //Reservation class 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("_id")]
    public string _id { get; set; }

    [JsonPropertyName("trainScheduleid")]
    public string TrainScheduleid { get; set; }

    [JsonPropertyName("nic")]
    public string nic { get; set; }

    [JsonPropertyName("createdAt")]
    public BsonTimestamp CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public BsonTimestamp UpdatedAt { get; set; }

    [JsonPropertyName("reservationDate")]
    public DateTime ReservationDate { get; set; }

    [JsonPropertyName("reserveCount")]
    [BsonRepresentation(BsonType.Int32)]
    [BsonElement("reserveCount")]
    [Range(0, 4, ErrorMessage = "The 'reserveCount' field must be between 0 and 4.")]
    public int ReserveCount { get; set; } = 0; // Default value set to 0

    [JsonPropertyName("status")]
    [BsonRepresentation(BsonType.String)]
    [BsonElement("status")]
    [EnumDataType(typeof(StatusEnum))]
    public StatusEnum Status { get; set; } = StatusEnum.ACTIVE; // Default value set to "ACTIVE"

}