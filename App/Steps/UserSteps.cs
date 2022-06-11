using System.Drawing;
using CliWrap;
using Pastel;
using static App.Utils.Executor;

namespace App.Steps;

public static class UserSteps
{
    private static string AskForUsername()
    {
        Console.Write("Write username: ".Pastel(Color.Gold));
        var username = Console.ReadLine();

        if (username is null)
        {
            Console.WriteLine("Invalid username. Try again.".Pastel(Color.Crimson));
            return AskForUsername();
        }

        return username;
    }

    private static string AskForPassword()
    {
        Console.Write("Write temporary password: ".Pastel(Color.Gold));
        var password = Console.ReadLine();
        
        if (password is null)
        {
            Console.WriteLine("Invalid password. Try again.".Pastel(Color.Crimson));
            return AskForPassword();
        }

        return password;
    }

    private static async Task CreateUser(string username)
    {
        Console.WriteLine($"Creating user: \"{username}\"...".Pastel(Color.Teal));
        await Execute(new[] {"useradd", username});
    }

    private static async Task SetTempPassword(string username, string password)
    {
        Console.WriteLine("Setting temp password for user...".Pastel(Color.Teal));
        var passwdCmd = $"{password}\n{password}\n\n\n\n\nY" | Cli.Wrap("adduser")
            .WithArguments(username);
        await passwdCmd.ExecuteAsync();
    }

    private static async Task AddUserToSudoRole(string username)
    {
        Console.WriteLine("Assigning user roles...".Pastel(Color.Teal));
        await Execute(new[] {"usermod", "-aG", "sudo", username}, silently: true);
    }

    public static async Task<(string username, string pw)> PerformAll()
    {
        var username = AskForUsername();
        var password = AskForPassword();
        
        await CreateUser(username);
        await SetTempPassword(username, password);
        await AddUserToSudoRole(username);

        Console.WriteLine("User created".Pastel(Color.Chartreuse));

        return (username, password);
    }
}