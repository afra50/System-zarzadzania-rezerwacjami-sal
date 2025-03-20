using System.Text.Json.Serialization;

public class ReservationsByMonth
{
    /// <summary>
    /// Nazwa miesiąca (np. Styczeń, Luty).
    /// </summary>
    [JsonPropertyName("month_name")]
    public string MonthName { get; set; }

    /// <summary>
    /// Liczba rezerwacji przypadających na dany miesiąc.
    /// </summary>
    [JsonPropertyName("reservation_count")]
    public int ReservationCount { get; set; }
}
