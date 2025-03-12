using System;
using System.Net.Http;

namespace RezerwacjeSal.Services
{
    public class ApiService
    {
        private static readonly string _baseUrl = $"{AppConfig.ApiBaseUrl}"; // Zmieñ na odpowiednie IP
        private static readonly Lazy<HttpClient> _httpClientInstance = new Lazy<HttpClient>(() => new HttpClient());

        public static HttpClient HttpClient => _httpClientInstance.Value;
        public static string BaseUrl => _baseUrl;
    }
}
