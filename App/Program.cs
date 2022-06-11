using System.Drawing;
using App.Utils;
using CliWrap;
using CliWrap.Buffered;
using Pastel;
using static App.Utils.Executor;


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

Console.WriteLine(result.StandardOutput);
Console.WriteLine("Assigning user roles...".Pastel(Color.Teal));
await Execute(new[] {"usermod", "-aG", "sudo", username}, silently: true);

Console.WriteLine("User created".Pastel(Color.Chartreuse));


/* --- SETTING UP FIREWALL --- */
Console.WriteLine("Setting up firewall...".Pastel(Color.Teal));
await Execute(new[] {"ufw", "allow", "OpenSSH"}, silently: true);
await Execute(new[] {"ufw", "--force", "enable"}, silently: true);

Console.WriteLine("Firewall enabled.".Pastel(Color.Chartreuse));

/* --- SETTING UP SSH KEYS --- */
// var username = "bobby";
// var pw = "password123";
var currentIp = await IpGrabber.GrabIpNoHttp();
Console.WriteLine(
    "\nAdd ssh keys remotely by using the ssh-copy-id command from your client machine. You will be prompted to use the temporary password."
        .Pastel(Color.Gold));
Console.WriteLine($"\tCommand: ssh-copy-id {username}@{currentIp}".Pastel(Color.Teal));
Console.WriteLine($"\tUsername: {username}".Pastel(Color.Teal));
Console.WriteLine($"\tPassword: {pw}".Pastel(Color.Teal));
Speaker.SayPressAnyKey();

Console.WriteLine("\nNow from your client machine, attempt to login to the remote server using your new user using ssh."
    .Pastel(Color.Gold));
Speaker.SayPressAnyKey();

Console.WriteLine("Turning off password authentication.".Pastel(Color.Teal));
var path = "/etc/ssh/sshd_config";
var sshdConfig = FileSystem.FetchFileContents(path);

var linesAfterEdit = FileSystem.EditLine(
    sshdConfig,
    "PasswordAuthentication yes",
    "PasswordAuthentication no");

await FileSystem.OverwriteFile(path, linesAfterEdit);

await Execute(new[] {"sudo", "systemctl", "restart", "ssh"});

Console.WriteLine("\nSSH authentication completed and password authentication is disabled.".Pastel(Color.Gold));
Console.WriteLine("Try to log in again with ssh to confirm it's working.".Pastel(Color.Gold));
Speaker.SayPressAnyKey();

Console.WriteLine("SSH set up complete".Pastel(Color.Chartreuse));