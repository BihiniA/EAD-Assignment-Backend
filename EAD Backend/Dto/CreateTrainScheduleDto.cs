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
        public DateTime StartTime { get; set; }

        [JsonPropertyName("endTime")]
        public DateTime EndTime { get; set; }
    }
}
