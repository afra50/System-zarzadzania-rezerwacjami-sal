using System.Text.Json.Serialization;

public class ReservationsByMonth
{
    [JsonPropertyName("month")]
    public int Month { get; set; }

    [JsonPropertyName("reservation_count")]
    public int ReservationCount { get; set; }
}
