/// <summary>
/// Statyczna klasa odpowiedzialna za przechowywanie danych sesji użytkownika.
/// </summary>
public static class SessionManager
{
    /// <summary>
    /// Przechowuje nazwę użytkownika w sesji.
    /// </summary>
    public static string? UserName { get; set; }

    /// <summary>
    /// Przechowuje adres email użytkownika w sesji.
    /// </summary>
    public static string? UserEmail { get; set; }

    /// <summary>
    /// Przechowuje rolę użytkownika w sesji (np. admin, klient).
    /// </summary>
    public static string? UserRole { get; set; }

    /// <summary>
    /// Określa, czy użytkownik ma rolę administratora.
    /// </summary>
    public static bool IsAdmin => UserRole == "admin";
}
