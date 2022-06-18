using App.Terminal;
using App.Utils;
using CliWrap;
using CliWrap.Buffered;
using static App.Utils.ShellController;

namespace App.Steps;

public static class UserSteps
{
    private static string AskForUsername()
    {
        ColorPrinter.CallToAction("Write username: ");
        var username = Console.ReadLine();

        if (username is null)
        {
            ColorPrinter.Error("Invalid username. Try again.");
            return AskForUsername();
        }

        return username;
    }

    private static string AskForPassword()
    {
        // ColorPrinter.CallToAction("Write password for user: ");
        var password = Prompter.PromptUserAsHidden("Write password for user: ");

        if (password is null)
        {
            ColorPrinter.Error("Invalid password. Try again.");
            return AskForPassword();
        }

        return password;
    }

    private static async Task CreateUserWithPassword(string username, string password)
    {
        ColorPrinter.Working($"Creating user: \"{username}\"...");
        var passwdCmd = $"{password}\n{password}\n\n\n\n\nY" | Cli.Wrap("adduser")
            .WithArguments(username);
        await passwdCmd.ExecuteBufferedAsync();
    }

    private static async Task AddUserToSudoRole(string username)
    {
        ColorPrinter.Working("Assigning user roles...");
        await Execute(new[] {"usermod", "-aG", "sudo", username}, silently: true);
    }

    public static async Task<(string username, string pw)> PerformAll()
    {
        var username = AskForUsername();
        var password = AskForPassword();

        await CreateUserWithPassword(username, password);
        await AddUserToSudoRole(username);

        ColorPrinter.WorkCompleted("User created.");

        return (username, password);
    }
}