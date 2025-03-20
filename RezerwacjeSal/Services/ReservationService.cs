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
    /// <summary>
    /// Serwis odpowiedzialny za zarządzanie rezerwacjami w systemie. Obejmuje operacje takie jak tworzenie, pobieranie, anulowanie i potwierdzanie rezerwacji.
    /// </summary>
    public class ReservationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ReservationService()
        {
            _httpClient = new HttpClient();
            _baseUrl = System.Configuration.ConfigurationManager.AppSettings["ApiBaseUrl"] + "/reservations";
        }
        /// <summary>
        /// Tworzy nową rezerwację dla wybranego pokoju użytkownika.
        /// </summary>
        /// <param name="reservation">Obiekt zawierający dane rezerwacji</param>
        /// <returns>Wartość logiczną wskazującą, czy rezerwacja została pomyślnie utworzona</returns>

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

        /// <summary>
        /// Pobiera listę rezerwacji. Opcjonalnie umożliwia filtrowanie po identyfikatorze pokoju.
        /// </summary>
        /// <param name="roomId">Opcjonalny identyfikator pokoju do filtrowania rezerwacji</param>
        /// <returns>Lista rezerwacji</returns>
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

        /// <summary>
        /// Pobiera dostępne terminy rezerwacji dla konkretnego pokoju.
        /// </summary>
        /// <param name="roomId">Identyfikator pokoju</param>
        /// <returns>Lista dostępnych terminów rezerwacji</returns>
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

        /// <summary>
        /// Pobiera rezerwacje dokonane przez użytkownika na podstawie jego adresu email.
        /// </summary>
        /// <param name="userEmail">Adres email użytkownika</param>
        /// <returns>Lista rezerwacji użytkownika</returns>
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

        /// <summary>
        /// Anuluje wybraną rezerwację.
        /// </summary>
        /// <param name="reservationId">Identyfikator rezerwacji do anulowania</param>
        /// <returns>Wartość logiczną wskazującą, czy anulowanie zakończyło się powodzeniem</returns>
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

        /// <summary>
        /// Potwierdza wybraną rezerwację.
        /// </summary>
        /// <param name="reservationId">Identyfikator rezerwacji do potwierdzenia</param>
        /// <returns>Wartość logiczną wskazującą, czy rezerwacja została pomyślnie potwierdzona</returns>
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

        /// <summary>
        /// Pobiera statystyki rezerwacji według dni tygodnia.
        /// </summary>
        /// <returns>Lista rezerwacji z podziałem na dni tygodnia</returns>
        public async Task<List<ReservationsByDay>> GetReservationsByDayAsync()
        {
            string url = $"{_baseUrl}/reservations-by-day";
            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ReservationsByDay>>(json);
        }

        /// <summary>
        /// Pobiera statystyki rezerwacji według miesięcy.
        /// </summary>
        /// <returns>Lista rezerwacji z podziałem na miesiące</returns>
        public async Task<List<ReservationsByMonth>> GetReservationsByMonthAsync()
        {
            string url = $"{_baseUrl}/reservations-by-month";
            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ReservationsByMonth>>(json);
        }


    }
}
