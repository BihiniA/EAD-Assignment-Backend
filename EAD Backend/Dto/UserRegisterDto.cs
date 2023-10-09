using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace EAD_Backend.Dto
{
    public class UserRegisterDto
    {
        [BsonId]
        [JsonPropertyName("nic")]
        public string nic { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; } 

        [JsonPropertyName("email")]
        public string email { get; set; } 

        [JsonPropertyName("role")]
        [BsonRepresentation(BsonType.String)]
        [BsonElement("role")]
        public UserRole Role { get; set; } 

        [JsonPropertyName("password")]
        public string password { get; set; } 
    }
}
