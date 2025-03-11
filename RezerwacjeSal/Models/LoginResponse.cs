namespace RezerwacjeSal.Models
{
    public class LoginResponse
    {
        public string Message { get; set; } = string.Empty;
        public UserDto? User { get; set; }
    }
}
