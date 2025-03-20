using System;
using System.Text.Json.Serialization;

namespace RezerwacjeSal.Models
{
    public class Reservation
    {
        /// <summary>
        /// Identyfikator rezerwacji.
        /// </summary>
        [JsonPropertyName("id_reservation")]
        public int IdReservation { get; set; }

        /// <summary>
        /// Identyfikator sali, która została zarezerwowana.
        /// </summary>
        [JsonPropertyName("room_id")]
        public int RoomId { get; set; }

        /// <summary>
        /// Adres email użytkownika, który dokonał rezerwacji.
        /// </summary>
        [JsonPropertyName("user_email")]
        public string UserEmail { get; set; } = string.Empty;

        /// <summary>
        /// Data i godzina rozpoczęcia rezerwacji.
        /// </summary>
        [JsonPropertyName("start_datetime")]
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Data i godzina zakończenia rezerwacji.
        /// </summary>
        [JsonPropertyName("end_datetime")]
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Status rezerwacji (np. 'potwierdzona', 'oczekująca', 'anulowana').
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }
}
