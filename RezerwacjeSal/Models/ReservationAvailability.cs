using System;
using System.Text.Json.Serialization;

public class ReservationAvailability
{
    /// <summary>
    /// Data i godzina rozpoczęcia dostępności rezerwacji.
    /// </summary>
    [JsonPropertyName("start_datetime")]
    public DateTime StartDateTime { get; set; }

    /// <summary>
    /// Data i godzina zakończenia dostępności rezerwacji.
    /// </summary>
    [JsonPropertyName("end_datetime")]
    public DateTime EndDateTime { get; set; }

    /// <summary>
    /// Lokalne godzinne ustawienie rozpoczęcia dostępności.
    /// </summary>
    public DateTime StartDateTimeLocal => StartDateTime.ToLocalTime();

    /// <summary>
    /// Lokalne godzinne ustawienie zakończenia dostępności.
    /// </summary>
    public DateTime EndDateTimeLocal => EndDateTime.ToLocalTime();
}
