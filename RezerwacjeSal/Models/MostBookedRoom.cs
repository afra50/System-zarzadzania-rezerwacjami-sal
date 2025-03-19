using System.Text.Json.Serialization;

public class MostBookedRoom
{
    [JsonPropertyName("room_id")]
    public int RoomId { get; set; }

    [JsonPropertyName("room_name")]
    public string RoomName { get; set; }

    [JsonPropertyName("booking_count")]
    public int BookingCount { get; set; }
}
