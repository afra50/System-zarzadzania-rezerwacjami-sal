using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using RezerwacjeSal.Models;

namespace RezerwacjeSal.Services
{
    public class RoomService
    {
        private static readonly HttpClient _httpClient = new HttpClient(); // Singleton dla oszczędności zasobów
        private readonly string _baseUrl;

        public RoomService()
        {
            string? baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException("Brak ApiBaseUrl w App.config!");
            }

            _baseUrl = $"{baseUrl}/rooms";
        }

        // Pobieranie listy sal z API
        public async Task<List<Room>> GetRoomsAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/all");
                response.EnsureSuccessStatusCode(); // Wyrzuci wyjątek w przypadku niepowodzenia

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Room>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Room>();
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show("Nie udało się połączyć z serwerem.");
                return new List<Room>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd aplikacji: {ex.Message}");
                return new List<Room>();
            }
        }

        // Metoda do aktualizacji danych sali
        public async Task<bool> UpdateRoomAsync(Room room)
        {

            try
            {

                // **Serializacja JSON**
                string json = JsonSerializer.Serialize(room, new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });

                HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync($"{_baseUrl}/update/{room.Id}", content);
                string responseString = await response.Content.ReadAsStringAsync();

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd aktualizacji: {ex.Message}");
                return false;
            }
        }

        public async Task<List<MostBookedRoom>> GetMostBookedRoomsAsync()
        {
            string url = $"{_baseUrl}/most-booked-rooms";
            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            var rooms = JsonSerializer.Deserialize<List<MostBookedRoom>>(json);

            // Pobranie pełnych nazw sal
            var allRooms = await GetRoomsAsync();
            foreach (var room in rooms)
            {
                room.RoomName = allRooms.FirstOrDefault(r => r.Id == room.RoomId)?.Name ?? "Nieznana sala";
            }

            return rooms;
        }

    }
}
