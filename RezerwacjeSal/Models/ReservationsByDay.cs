using System.Text.Json.Serialization;

public class ReservationsByDay
{
    [JsonPropertyName("day_of_week")]
    public int DayOfWeek { get; set; }

    [JsonPropertyName("reservation_count")]
    public int ReservationCount { get; set; }
}
