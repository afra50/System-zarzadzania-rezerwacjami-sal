using System.Text.Json.Serialization;

public class ReservationsByMonth
{
    [JsonPropertyName("month_name")]
    public string MonthName { get; set; }

    [JsonPropertyName("reservation_count")]
    public int ReservationCount { get; set; }
}
