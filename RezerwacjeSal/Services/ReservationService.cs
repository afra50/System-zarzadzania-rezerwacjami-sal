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

        public async Task<List<Reservation>> GetReservationsAsync(int? roomId = null)
        {
            try
            {
                string url = $"{_baseUrl}/list"; // Endpoint backendowy, który zwraca rezerwacje

                // Jeżeli roomId jest podane, dodajemy parametr do zapytania
                if (roomId.HasValue)
                {
                    url += $"?room_id={roomId.Value}";
                }

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<List<Reservation>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    MessageBox.Show($"❌ Błąd pobierania rezerwacji: {response.StatusCode}\n{responseString}");
                    return new List<Reservation>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"⚠️ Błąd pobierania rezerwacji: {ex.Message}");
                return new List<Reservation>();
            }
        }


        public async Task<List<ReservationAvailability>> GetOccupiedTimesAsync(int roomId)
        {
            try
            {
                string url = $"{_baseUrl}/occupied/{roomId}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                string responseString = await response.Content.ReadAsStringAsync();

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

        public async Task<List<Reservation>> GetUserReservationsAsync(string userEmail)
        {
            try
            {
                string url = $"{_baseUrl}/user/{userEmail}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<List<Reservation>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    MessageBox.Show($"❌ Błąd pobierania rezerwacji: {response.StatusCode}\n{responseString}");
                    return new List<Reservation>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"⚠️ Błąd pobierania rezerwacji: {ex.Message}");
                return new List<Reservation>();
            }
        }

        public async Task<bool> CancelReservationAsync(int reservationId)
        {
            try
            {
                // URL endpointu, który obsługuje anulowanie rezerwacji
                string url = $"{_baseUrl}/cancel/{reservationId}";

                // Zmieniamy metodę na DELETE
                HttpResponseMessage response = await _httpClient.DeleteAsync(url); // Zmiana na DeleteAsync
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show($"❌ Błąd anulowania rezerwacji: {response.StatusCode}\n{responseString}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"⚠️ Błąd anulowania rezerwacji: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ConfirmReservationAsync(int reservationId)
        {
            try
            {
                string url = $"{_baseUrl}/confirm/{reservationId}"; // Endpoint do akceptowania rezerwacji
                HttpResponseMessage response = await _httpClient.PutAsync(url, null); // PUT bez body
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show($"❌ Błąd akceptacji rezerwacji: {response.StatusCode}\n{responseString}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"⚠️ Błąd akceptacji rezerwacji: {ex.Message}");
                return false;
            }
        }



    }
}
