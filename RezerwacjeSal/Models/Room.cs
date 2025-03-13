using System.Text.Json.Serialization;

public class Room
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    [JsonPropertyName("seats")]
    public int Seats { get; set; }

    [JsonPropertyName("latitude")]
    public decimal Latitude { get; set; } = 0.0m;

    [JsonPropertyName("longitude")]
    public decimal Longitude { get; set; } = 0.0m;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}
