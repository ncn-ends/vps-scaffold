using System.Drawing;
using System.Security.Cryptography;
using App;
using App.Operations;
using App.Utils;
using CliWrap;
using CliWrap.Buffered;
using CliWrap.EventStream;
using Pastel;
using static App.Utils.Executor;

// try
// {
//
//     var dockerResults = await Cli.Wrap("git")
//         .WithArguments("--version")
//         .ExecuteBufferedAsync();
//
//     Console.WriteLine(dockerResults.StandardError == ""
//         ? dockerResults.StandardOutput
//         : dockerResults.StandardError);
// }
// catch (Exception e)
// {
//     Console.WriteLine(e.ToString());
// }

// .WithWorkingDirectory
//      -- the directory to execute the command / shell script in

// Cli.Wrap(@"~/scripts/something.sh");
//      -- execute a shell script file


// await WriteAllLines.ExampleAsync();

// var username = UserOperations.RequestUsernameByInput();
// await UserOperations.CreateUserAndAssignPermissions(username);

// await Execute(new[] {@"./test.sh", "qwe", "asd"});
//
// var answer = Console.ReadLine();
// Console.WriteLine(answer);


/* --- SETTING UP USER --- */
Console.Write("Write username: ".Pastel(Color.Gold));
var username = Console.ReadLine();

if (username is null) return;

Console.WriteLine($"Creating user: \"{username}\"...".Pastel(Color.Teal));
await Execute(new[] {"useradd", username});

Console.WriteLine("Assigning user roles...".Pastel(Color.Teal));
await Execute(new[] {"usermod", "-aG", "sudo", username});

Console.WriteLine("User created".Pastel(Color.Chartreuse));


/* --- SETTING UP FIREWALL --- */
Console.WriteLine("Setting up firewall...".Pastel(Color.Teal));
await Execute(new[] {"ufw", "allow", "OpenSSH"});
await Execute(new[] {"ufw", "--force", "enable"});

Console.WriteLine("Firewall enabled.".Pastel(Color.Chartreuse));