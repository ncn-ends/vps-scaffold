using System.Drawing;
using App.Utils;
using Pastel;
using static App.Utils.Executor;

namespace App.Steps;

public static class SSHSteps
{
    private static async Task TellUserSetUpSSHFromClient(string username, string password)
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

    private static void TellUserTrySSHLogin()
    {
        Console.WriteLine(
            "\nNow from your client machine, attempt to login to the remote server using your new user using SSH authentication."
                .Pastel(Color.Gold));
        Speaker.SayPressAnyKey();
    }

    private static async Task TurnOffPasswordAuthentication()
    {
        Console.WriteLine("Turning off password authentication.".Pastel(Color.Teal));
        const string path = "/etc/ssh/sshd_config";
        const string lineBeforeEdit = "PasswordAuthentication yes";
        const string lineAfterEdit = "PasswordAuthentication no";

        await FileSystem.EditLine(
            path,
            lineBeforeEdit,
            lineAfterEdit);
    }

    private static async Task RestartSshService()
    {
        await Execute(new[] {"sudo", "systemctl", "restart", "ssh"});
    }

    private static void TellUserTrySshLoginAgain()
    {
        Console.WriteLine("\nSSH authentication completed and password authentication is disabled.".Pastel(Color.Gold));
        Console.WriteLine("Try to log in again with ssh to confirm it's working.".Pastel(Color.Gold));
        Speaker.SayPressAnyKey();
    }


    public static async Task PerformAll(string username, string password)
    {
        Console.WriteLine("Setting up SSH Authentication...".Pastel(Color.Teal));

        await TellUserSetUpSSHFromClient(username, password);
        TellUserTrySSHLogin();
        await TurnOffPasswordAuthentication();
        await RestartSshService();
        TellUserTrySshLoginAgain();

        Console.WriteLine("SSH set up complete".Pastel(Color.Chartreuse));
    }
}