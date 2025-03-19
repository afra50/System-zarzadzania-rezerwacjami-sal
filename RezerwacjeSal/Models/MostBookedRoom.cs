using System.Text.Json.Serialization;

public class MostBookedRoom
{
    [JsonPropertyName("room_id")]
    public int RoomId { get; set; }

    [JsonPropertyName("booking_count")]
    public int BookingCount { get; set; }
}
