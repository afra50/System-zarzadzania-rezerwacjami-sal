using System.Text.Json.Serialization;

public class ReservationsByDay
{
    [JsonPropertyName("day_name")]
    public string DayName { get; set; }

    [JsonPropertyName("reservation_count")]
    public int ReservationCount { get; set; }
}
