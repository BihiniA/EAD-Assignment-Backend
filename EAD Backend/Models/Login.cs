using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


[BsonIgnoreExtraElements]
public class Login  //user login class
{

    [JsonPropertyName("email")]
    public string email { get; set; } 

    [JsonPropertyName("password")]
    public string password { get; set; } 


}