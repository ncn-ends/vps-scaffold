using System.Drawing;
using App.Utils;
using Pastel;
using static App.Utils.Executor;

namespace App.Steps;

public static class SSHSteps
{
    public static async Task TellUserSetUpSSHFromClient(string username, string password)
    {
        var currentIp = await IpGrabber.GrabIpNoHttp();
        Console.WriteLine(
            "\nAdd ssh keys remotely by using the ssh-copy-id command from your client machine. You will be prompted to use the temporary password."
                .Pastel(Color.Gold));
        Console.WriteLine($"\tCommand: ssh-copy-id {username}@{currentIp}".Pastel(Color.Teal));
        Console.WriteLine($"\tUsername: {username}".Pastel(Color.Teal));
        Console.WriteLine($"\tPassword: {password}".Pastel(Color.Teal));
        Speaker.SayPressAnyKey();
    }

    public static void TellUserTrySSHLogin()
    {
        Console.WriteLine("\nNow from your client machine, attempt to login to the remote server using your new user using SSH authentication."
            .Pastel(Color.Gold));
        Speaker.SayPressAnyKey();
    }

    public static async Task TurnOffPasswordAuthentication()
    {
        Console.WriteLine("Turning off password authentication.".Pastel(Color.Teal));
        var path = "/etc/ssh/sshd_config";
        var sshdConfig = FileSystem.FetchFileContents(path);

        var linesAfterEdit = FileSystem.EditLine(
            sshdConfig,
            "PasswordAuthentication yes",
            "PasswordAuthentication no");

        await FileSystem.OverwriteFile(path, linesAfterEdit);
    }

    public static async Task RestartSshService()
    {
        await Execute(new[] {"sudo", "systemctl", "restart", "ssh"});
    }

    public static void TellUserTrySshLoginAgain()
    {
        Console.WriteLine("\nSSH authentication completed and password authentication is disabled.".Pastel(Color.Gold));
        Console.WriteLine("Try to log in again with ssh to confirm it's working.".Pastel(Color.Gold));
        Speaker.SayPressAnyKey();
    }
    
    
    public static async Task PerformAll(string username, string password)
    {
        await TellUserSetUpSSHFromClient(username, password);
        TellUserTrySSHLogin();
        await TurnOffPasswordAuthentication();
        await RestartSshService();
        TellUserTrySshLoginAgain();

        Console.WriteLine("SSH set up complete".Pastel(Color.Chartreuse));
    }
}