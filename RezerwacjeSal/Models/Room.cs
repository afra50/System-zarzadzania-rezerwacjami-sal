using System.Text.Json.Serialization;

public class Room
{
    /// <summary>
    /// Identyfikator sali.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Nazwa sali.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Adres sali.
    /// </summary>
    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Liczba miejsc w sali.
    /// </summary>
    [JsonPropertyName("seats")]
    public int Seats { get; set; }

    /// <summary>
    /// Szerokość geograficzna sali.
    /// </summary>
    [JsonPropertyName("latitude")]
    public decimal Latitude { get; set; } = 0.0m;

    /// <summary>
    /// Długość geograficzna sali.
    /// </summary>
    [JsonPropertyName("longitude")]
    public decimal Longitude { get; set; } = 0.0m;

    /// <summary>
    /// Opis sali.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}
