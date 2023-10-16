using EAD_Backend.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System;

namespace EAD_Backend.Dto
{
    public class TrainScheduleUpdate
    {
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

    }
}
