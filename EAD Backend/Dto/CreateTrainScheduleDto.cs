using System.Text.Json.Serialization;
using System;

namespace EAD_Backend.Dto
{
    public class CreateTrainScheduleDto
    {
        [JsonPropertyName("trainId")]
        public string TrainId { get; set; } = null!;

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
    }
}
