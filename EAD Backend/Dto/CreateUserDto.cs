using EAD_Backend.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EAD_Backend.Dto
{
    public class CreateUserDto
    {
        [BsonId]
        [JsonPropertyName("nic")]
        public string nic { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; } = null!;

        [JsonPropertyName("email")]
        public string email { get; set; } = null!;

        [JsonPropertyName("password")]
        public string password { get; set; } = null!;
    }
}
