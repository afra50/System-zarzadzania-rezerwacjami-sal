public static class SessionManager
{
    public static string? UserName { get; set; }
    public static string? UserEmail { get; set; }
    public static string? UserRole { get; set; }

    public static bool IsAdmin => UserRole == "admin";
}
