using System;
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
                // ✅ Poprawiona struktura JSON (email zamiast e_mail)
                var requestBody = new
                {
                    room_id = reservation.RoomId,
                    email = reservation.UserEmail, // 🔵 Użycie poprawnej nazwy pola
                    start_datetime = reservation.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss"), // 🔵 MySQL/MariaDB oczekuje tego formatu
                    end_datetime = reservation.EndDateTime.ToString("yyyy-MM-dd HH:mm:ss") // 🔵 Ten format pasuje do `DATETIME`
                };

                string json = JsonSerializer.Serialize(requestBody);
                MessageBox.Show($"Wysyłane dane JSON:\n{json}");

                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // 🟢 Dodanie poprawnych nagłówków JSON
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
    }
}
