using System;
using System.Text.Json.Serialization;

public class ReservationAvailability
{
    [JsonPropertyName("start_datetime")]
    public DateTime StartDateTime { get; set; }

    [JsonPropertyName("end_datetime")]
    public DateTime EndDateTime { get; set; }

    public DateTime StartDateTimeLocal => StartDateTime.ToLocalTime();
    public DateTime EndDateTimeLocal => EndDateTime.ToLocalTime();
}
