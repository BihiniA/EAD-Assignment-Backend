using EAD_Backend.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


public enum UserRole
{
    User,
    travelAgent,
    backOfficer
}

[BsonIgnoreExtraElements]
public class Users  //user class with relevent fields
{
    [BsonId]
    [JsonPropertyName("nic")]
    public string nic { get; set; }

    [JsonPropertyName("name")]
    public string name { get; set; } = null!;

    [JsonPropertyName("email")]
    public string email { get; set; } = null!;

    public UserRole Role { get; set; } 

    [JsonPropertyName("password")]
    public string password { get; set; } = null!;

    [JsonPropertyName("status")]
    [EnumDataType(typeof(StatusEnum))]
    public StatusEnum Status { get; set; } = StatusEnum.ACTIVE; // Default value set to "ACTIVE"
}



