using App.Utils;
using CliWrap;
using CliWrap.Buffered;

namespace App.Operations;

public class UserOperations
{
    public static string? RequestUsernameByInput()
    {
        Console.WriteLine("Enter username. (This username will be used to login to the server):\n");
        var username = Console.ReadLine();

        if (username is null)
        {
            Console.WriteLine("Username cannot be empty.");
            return RequestUsernameByInput();
        }

        return username;
    }

    public static async Task CreateUser(string username, string password)
    {
        try
        {
            Console.WriteLine($"Creating user: \"{username}\"...");

            // var addUserResults = await Cli.Wrap("adduser")
            //     .WithArguments(username)
            //     .ExecuteBufferedAsync();
            // Console.WriteLine(addUserResults.StandardOutput);
            await Executor.Execute(new[] {"useradd", username});
            Console.WriteLine("user created");
            
            var asd = Console.ReadLine();
            // Console.WriteLine("done waiting");
            // Console.ReadLine();

            // Console.WriteLine("Typing in password...");
            // Console.WriteLine(password);
            // Thread.Sleep(500);
            //
            // Console.WriteLine("Typing in password...");
            // Console.WriteLine(password);
            // Thread.Sleep(500);
            //
            //
            // if (addUserResults.StandardError != "")
            //     throw new Exception(addUserResults.StandardError);

            // Console.WriteLine("User created.");
            // Console.WriteLine("Adding user to sudo group...");
            //
            // var userModResults = await Cli.Wrap("usermod")
            //     .WithArguments(new[] {"-aG", "sudo", username})
            //     .ExecuteBufferedAsync();
            //
            // if (userModResults.StandardError != "")
            //     throw new Exception(userModResults.StandardError);
            //
            // Console.WriteLine("User added to sudo group.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}