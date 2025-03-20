using System.Text.Json.Serialization;

public class ReservationsByDay
{
    /// <summary>
    /// Nazwa dnia tygodnia (np. Poniedziałek, Wtorek).
    /// </summary>
    [JsonPropertyName("day_name")]
    public string DayName { get; set; }

    /// <summary>
    /// Liczba rezerwacji przypadających na dany dzień tygodnia.
    /// </summary>
    [JsonPropertyName("reservation_count")]
    public int ReservationCount { get; set; }
}
