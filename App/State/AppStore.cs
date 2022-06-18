namespace App.State;

public static class AppStore
{
    public static string Username { get; set; } = "";
    public static string Password { get; set; } = "";
    public static string CurrentIp { get; set; } = "";

    public static string DomainName { get; set; } = "";

    public static FlagStore FlagStore { get; set; } = new(new string[] { });

    public static string Email { get; set; } = "";
}