using System;
using System.Configuration; // Umożliwia odczyt z App.config
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using RezerwacjeSal.Models;

namespace RezerwacjeSal.Services
{
    /// <summary>
    /// Serwis odpowiedzialny za autentykację użytkownika, w tym rejestrację i logowanie.
    /// </summary>
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        /// <summary>
        /// Inicjalizuje serwis autentykacji, ustawiając klienta HTTP i bazowy URL do API.
        /// </summary>
        public AuthService()
        {
            _httpClient = new HttpClient();
            _baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"] + "/auth"; // Pobieramy z App.config
        }

        /// <summary>
        /// Rejestruje użytkownika w systemie, wysyłając dane do API.
        /// </summary>
        /// <param name="name">Imię i nazwisko użytkownika</param>
        /// <param name="email">Adres email użytkownika</param>
        /// <param name="password">Hasło użytkownika</param>
        /// <param name="role">Rola użytkownika (np. klient lub administrator)</param>
        /// <returns>Wartość logiczna wskazująca sukces lub porażkę operacji</returns>
        public async Task<bool> RegisterUser(string name, string email, string password, string role)
        {
            var requestBody = new
            {
                name,
                email,
                password,
                role
            };

            try
            {
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync($"{_baseUrl}/register", content);
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Rejestracja zakończona sukcesem!");
                    return true;
                }
                else
                {
                    MessageBox.Show($"Błąd rejestracji: {responseString}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd połączenia z API: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Loguje użytkownika, wysyłając dane do API i zwraca obiekt użytkownika.
        /// </summary>
        /// <param name="email">Adres email użytkownika</param>
        /// <param name="password">Hasło użytkownika</param>
        /// <returns>Obiekt użytkownika lub null w przypadku nieudanego logowania</returns>
        public async Task<UserDto?> LoginUser(string email, string password)
        {
            var requestBody = new
            {
                email,
                password
            };

            try
            {
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync($"{_baseUrl}/login", content);
                string responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Błąd logowania: {responseString}");
                    return null;
                }

                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseString);
                return loginResponse?.User;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd połączenia z API: {ex.Message}");
                return null;
            }
        }
    }
}
