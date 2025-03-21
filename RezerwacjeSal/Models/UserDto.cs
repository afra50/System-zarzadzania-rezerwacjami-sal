namespace RezerwacjeSal.Models
{
    public class UserDto
    {
        /// <summary>
        /// Imię i nazwisko użytkownika.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Adres e-mail użytkownika.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Rola użytkownika (administrator, klient).
        /// </summary>
        public string Role { get; set; } = string.Empty;
    }
}
