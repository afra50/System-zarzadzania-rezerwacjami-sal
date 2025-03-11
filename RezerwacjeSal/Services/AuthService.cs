using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using RezerwacjeSal.Models;

namespace RezerwacjeSal.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://192.168.0.3:5001/api/auth";

        public AuthService()
        {
            _httpClient = new HttpClient();
        }

        // Rejestracja (pozostaje bez zmian, zwraca bool)
        public async Task<bool> RegisterUser(string name, string email, string password, string role)
        {
            var requestBody = new
            {
                name = name,
                email = email,
                password = password,
                role = role
            };

            try
            {
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/register", content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Rejestracja zakończona sukcesem!");
                    return true;
                }
                else
                {
                    MessageBox.Show($"Błąd: {responseString}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd połączenia: {ex.Message}");
                return false;
            }
        }

        // LOGOWANIE: zwracamy teraz obiekt UserDto? w zależności od JSON-a
        public async Task<UserDto?> LoginUser(string email, string password)
        {
            var requestBody = new
            {
                email = email,
                password = password
            };

            try
            {
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/login", content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    // Logowanie nieudane
                    return null;
                }

                // Tu mapujemy do LoginResponse,
                // bo serwer zwraca: { "message": "...", "user": { "name": "...", "email": "...", "role": "..." } }
                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseString);

                // Zwracamy samo pole user
                return loginResponse?.User;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}");
                return null;
            }
        }
    }
}
