using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using RezerwacjeSal.Models;

namespace RezerwacjeSal.Services
{
    public class ReservationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ReservationService()
        {
            _httpClient = new HttpClient();
            _baseUrl = System.Configuration.ConfigurationManager.AppSettings["ApiBaseUrl"] + "/reservations";
        }

        public async Task<bool> CreateReservationAsync(Reservation reservation)
        {
            try
            {
                var requestBody = new
                {
                    room_id = reservation.RoomId,
                    email = reservation.UserEmail, 
                    start_datetime = reservation.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    end_datetime = reservation.EndDateTime.ToString("yyyy-MM-dd HH:mm:ss")
                };

                string json = JsonSerializer.Serialize(requestBody);

                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Dodanie poprawnych nagłówków JSON
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl, content);
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show($"❌ Błąd rezerwacji: {response.StatusCode}\n{responseString}");
                    return false;
                }
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"⚠️ Błąd HTTP: {httpEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"⚠️ Błąd połączenia: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ReservationAvailability>> GetOccupiedTimesAsync(int roomId)
        {
            try
            {
                string url = $"{_baseUrl}/occupied/{roomId}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                string responseString = await response.Content.ReadAsStringAsync();

                Debug.WriteLine($"🔵 Odpowiedź API: {responseString}"); // ✅ Debug JSON-a

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<List<ReservationAvailability>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    MessageBox.Show($"❌ Błąd pobierania zajętych terminów: {response.StatusCode}\n{responseString}");
                    return new List<ReservationAvailability>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"⚠️ Błąd pobierania terminów: {ex.Message}");
                return new List<ReservationAvailability>();
            }
        }

    }
}
