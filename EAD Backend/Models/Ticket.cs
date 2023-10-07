using EAD_Backend.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


[BsonIgnoreExtraElements]
public class Ticket  //Ticket class 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("_id")]
    public string _id { get; set; }

    [JsonPropertyName("reservation_id")]
    public string ReservationId { get; set; } = null!;

    [JsonPropertyName("status")]
    [BsonRepresentation(BsonType.String)]
    [BsonElement("status")]
    [EnumDataType(typeof(StatusEnum))]
    public StatusEnum Status { get; set; } = StatusEnum.ACTIVE; // Default value set to "ACTIVE"

}