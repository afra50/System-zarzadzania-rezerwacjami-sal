using System;
using System.Text.Json.Serialization;

namespace RezerwacjeSal.Models
{
    public class Reservation
    {
        [JsonPropertyName("id_reservation")]
        public int IdReservation { get; set; }

        [JsonPropertyName("room_id")]
        public int RoomId { get; set; }

        [JsonPropertyName("user_email")]
        public string UserEmail { get; set; } = string.Empty;

        [JsonPropertyName("start_datetime")]
        public DateTime StartDateTime { get; set; }

        [JsonPropertyName("end_datetime")]
        public DateTime EndDateTime { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }
}
