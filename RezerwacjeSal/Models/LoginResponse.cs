namespace RezerwacjeSal.Models
{
    /// <summary>
    /// Odpowiedź logowania zawierająca komunikat i informacje o użytkowniku.
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Komunikat zwrócony po próbie logowania (np. błędne dane, sukces).
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Obiekt zawierający dane użytkownika po udanym logowaniu.
        /// </summary>
        public UserDto? User { get; set; }
    }
}
