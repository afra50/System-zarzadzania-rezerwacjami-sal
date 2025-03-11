using System;
using System.Net.Http;
using System.Threading.Tasks;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://192.168.0.3:5001"; // Zmie� na publiczne IP - 95.215.232.175:5001

    public ApiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<bool> IsApiAvailable()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/");
            return response.IsSuccessStatusCode; // Je�li kod 200 OK, API dzia�a
        }
        catch
        {
            return false; // Wszelkie b��dy oznaczaj� brak dost�pu
        }
    }
}
