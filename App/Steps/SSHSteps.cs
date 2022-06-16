using App.Static;
using App.Utils;
using static App.Utils.ShellController;

namespace App.Steps;

public static class SSHSteps
{
    private static void TellUserSetUpSshFromClient()
    {
        var username = Data.Username;
        var password = Data.Password;
        var currentIp = Data.CurrentIp;
        ColorPrinter.CallToAction(
            "\nAdd ssh keys remotely by using the ssh-copy-id command from your client machine. You will be prompted to use the temporary password.");
        ColorPrinter.ImportantInfo($"\tCommand: ssh-copy-id {username}@{currentIp}");
        ColorPrinter.ImportantInfo($"\tPassword: {password}");
        Speaker.SayPressAnyKey();
    }

    private static void TellUserTrySshLogin()
    {
        ColorPrinter.CallToAction(
            "\nNow from your client machine, attempt to login to the remote server using your new user using SSH authentication.");
        Speaker.SayPressAnyKey();
    }

    private static async Task TurnOffPasswordAuthentication()
    {
        ColorPrinter.Working("Turning off password authentication.");
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
        ColorPrinter.CallToAction("\nSSH authentication completed and password authentication is disabled.");
        ColorPrinter.CallToAction("Try to log in again with ssh to confirm it's working.");
        Speaker.SayPressAnyKey();
    }


    public static async Task PerformAll()
    {
        ColorPrinter.Working("Setting up SSH Authentication...");

        await Firewall.OpenSshPorts();
        TellUserSetUpSshFromClient();
        TellUserTrySshLogin();
        await TurnOffPasswordAuthentication();
        await RestartSshService();
        TellUserTrySshLoginAgain();

        ColorPrinter.Working("SSH set up complete");
    }
}