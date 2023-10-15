using EAD_Backend.config;
using EAD_Backend.Models;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace EAD_Backend.Dto
{
    public class UpdateUserDto
    {
        [JsonPropertyName("name")]
        public string? name { get; set; }

        [JsonPropertyName("email")]
        public string? email { get; set; }

        [JsonPropertyName("Role")]
        public UserRole? Role { get; set; }

        [JsonPropertyName("password")]
        public string? password { get; set; }

        [JsonPropertyName("status")]
        [EnumDataType(typeof(StatusEnum))]
        public StatusEnum? Status { get; set; }
    }
}
