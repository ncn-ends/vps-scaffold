// pm2 startup systemd

public static class FinalMessages
{
    public static async Task PerformAll(string password)
    {
        Console.WriteLine("Set up process complete!");
        Console.WriteLine("Here are some things you may want to do on your own: ");
        Console.WriteLine("\t-- `pm2 startup systemd`");
        Console.WriteLine("\t-- Add your NodeJS application to the server.");
        Console.WriteLine("\t-- Startup the NodeJS application with pm2");
    }
}