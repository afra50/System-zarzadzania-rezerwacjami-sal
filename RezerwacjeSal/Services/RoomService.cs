using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using RezerwacjeSal.Models;

namespace RezerwacjeSal.Services
{
    public class RoomService
    {
        private static readonly string _baseUrl = $"{AppConfig.ApiBaseUrl}/rooms";

        private static readonly HttpClient _httpClient = new HttpClient();

        // Pobieranie listy sal z API
        public static async Task<List<Room>> GetRoomsAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/all");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Room>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Room>();
                }
                else
                {
                    MessageBox.Show($"Błąd pobierania sal: {response.StatusCode}");
                    return new List<Room>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd połączenia z API: {ex.Message}");
                return new List<Room>();
            }
        }
    }
}
