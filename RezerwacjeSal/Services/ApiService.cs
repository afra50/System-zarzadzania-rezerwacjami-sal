using System;
using System.Net.Http;
using System.Configuration;

namespace RezerwacjeSal.Services
{
    public class ApiService
    {
        private static readonly Lazy<HttpClient> _httpClientInstance = new Lazy<HttpClient>(() => new HttpClient());

        public static HttpClient HttpClient => _httpClientInstance.Value;

        public static string BaseUrl => ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "http://95.215.232.175:5001/api"; 
    }
}
