using System.Text.Json.Serialization;

namespace EAD_Backend.Dto
{
    public class UpdateTrainScheduleSeatCountDto
    {
        [JsonPropertyName("seatCount")]
        public int seatCount { get; set; }
    }
}
