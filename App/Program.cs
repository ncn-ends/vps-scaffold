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

// await Execute(new[] {"useradd", username});

Console.Write("Write temporary password: ".Pastel(Color.Gold));
var pw = Console.ReadLine();
if (pw is null) return;

Console.WriteLine($"Creating user: \"{username}\"...".Pastel(Color.Teal));


var passwdCmd = $"{pw}\n{pw}\n\n\n\n\nY" | Cli.Wrap("adduser")
    .WithArguments(username);
var result = await passwdCmd.ExecuteBufferedAsync();

Console.WriteLine(result.StandardError, result.StandardOutput);
Console.WriteLine("Assigning user roles...".Pastel(Color.Teal));
await Execute(new[] {"usermod", "-aG", "sudo", username});

Console.WriteLine("User created".Pastel(Color.Chartreuse));


/* --- SETTING UP FIREWALL --- */
Console.WriteLine("Setting up firewall...".Pastel(Color.Teal));
await Execute(new[] {"ufw", "allow", "OpenSSH"});
await Execute(new[] {"ufw", "--force", "enable"});

Console.WriteLine("Firewall enabled.".Pastel(Color.Chartreuse));

/* --- SETTING UP SSH KEYS --- */
// var username = "bobby";
// var pw = "password123";
var currentIp = IpGrabber.GrabIp();
Console.WriteLine(
    "\nAdd ssh keys remotely by using the ssh-copy-id command from your client machine. You will be prompted to use the temporary password.".Pastel(Color.Gold));
Console.WriteLine($"\tCommand: ssh-copy-id {username}@{currentIp}".Pastel(Color.Teal));
Console.WriteLine($"\tUsername: {username}".Pastel(Color.Teal));
Console.WriteLine($"\tPassword: {pw}".Pastel(Color.Teal));
Console.WriteLine("Once you're finished, press any key to continue...".Pastel(Color.Gold));
Console.ReadKey();
