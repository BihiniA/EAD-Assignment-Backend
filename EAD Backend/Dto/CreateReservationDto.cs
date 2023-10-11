using EAD_Backend.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System;

namespace EAD_Backend.NewFolder
{
    [BsonIgnoreExtraElements]
    public class CreateReservationDto
    { 

        [JsonPropertyName("trainScheduleId")]
        public string TrainScheduleId { get; set; }

        [JsonPropertyName("nic")]
        public string nic { get; set; }

        [JsonPropertyName("reservationDate")]
        public string ReservationDate { get; set; }

        [JsonPropertyName("reserveCount")]
        public int ReserveCount { get; set; }

    }
}