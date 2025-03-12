using Newtonsoft.Json;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Seats { get; set; }

    [JsonProperty("latitude")]
    public string LatitudeString { get; set; } = "0.0";

    [JsonProperty("longitude")]
    public string LongitudeString { get; set; } = "0.0";

    public decimal Latitude => decimal.TryParse(LatitudeString, out var lat) ? lat : 0.0m;
    public decimal Longitude => decimal.TryParse(LongitudeString, out var lon) ? lon : 0.0m;

    public string Description { get; set; } = string.Empty;
}
