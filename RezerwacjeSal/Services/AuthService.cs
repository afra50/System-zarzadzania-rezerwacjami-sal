using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace RezerwacjeSal.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://192.168.0.3:5001/api/auth"; // Zmień na IP serwera

        public AuthService()
        {
            _httpClient = new HttpClient();
        }

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


        public async Task<bool> LoginUser(string email, string password)
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

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Logowanie zakończone sukcesem!");
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
    }
}
