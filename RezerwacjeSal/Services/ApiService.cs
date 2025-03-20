using System;
using System.Net.Http;
using System.Configuration;

namespace RezerwacjeSal.Services
{
    /// <summary>
    /// Serwis odpowiedzialny za udostêpnienie instancji HttpClient oraz ustawienia podstawowego URL do API.
    /// </summary>
    public class ApiService
    {
        private static readonly Lazy<HttpClient> _httpClientInstance = new Lazy<HttpClient>(() => new HttpClient());

        /// <summary>
        /// Statyczna instancja HttpClient do wykonywania zapytañ HTTP.
        /// </summary>
        public static HttpClient HttpClient => _httpClientInstance.Value;

        /// <summary>
        /// Pobiera podstawowy URL API z konfiguracji lub ustawia domyœlny URL.
        /// </summary>
        public static string BaseUrl => ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "http://95.215.232.175:5001/api"; 
    }
}
