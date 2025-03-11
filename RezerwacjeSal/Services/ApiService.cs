using System;
using System.Net.Http;
using System.Threading.Tasks;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://192.168.1.100:5001"; // Zmie� na IP publiczne - 95.215.232.175:5001

    public ApiService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> GetTestMessage()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/");
            response.EnsureSuccessStatusCode(); // Sprawdza, czy API zwr�ci�o 200 OK
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"B��d pobierania wiadomo�ci z API: {ex.Message}");
            return "B��d API";
        }
    }
}
