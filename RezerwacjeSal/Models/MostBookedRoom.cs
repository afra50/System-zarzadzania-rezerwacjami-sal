using System.Text.Json.Serialization;

public class MostBookedRoom
{
    /// <summary>
    /// Identyfikator sali.
    /// </summary>
    [JsonPropertyName("room_id")]
    public int RoomId { get; set; }

    /// <summary>
    /// Nazwa sali.
    /// </summary>
    [JsonPropertyName("room_name")]
    public string RoomName { get; set; }

    /// <summary>
    /// Liczba rezerwacji dla danej sali.
    /// </summary>
    [JsonPropertyName("booking_count")]
    public int BookingCount { get; set; }
}
