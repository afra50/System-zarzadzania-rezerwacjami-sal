namespace RezerwacjeSal.Models
{
    public class UserDto
    {
        /// <summary>
        /// Imiê i nazwisko u¿ytkownika.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Adres e-mail u¿ytkownika.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Rola u¿ytkownika (administrator, klient).
        /// </summary>
        public string Role { get; set; } = string.Empty;
    }
}
