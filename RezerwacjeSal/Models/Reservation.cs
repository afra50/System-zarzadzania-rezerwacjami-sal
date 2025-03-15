using System;
using System.Text.Json.Serialization;

namespace RezerwacjeSal.Models
{
    public class Reservation
    {
        [JsonPropertyName("room_id")]
        public int RoomId { get; set; }

        [JsonPropertyName("user_email")]
        public string UserEmail { get; set; } = string.Empty;

        [JsonPropertyName("start_datetime")]
        public DateTime StartDateTime { get; set; }

        [JsonPropertyName("end_datetime")]
        public DateTime EndDateTime { get; set; }
    }
}
