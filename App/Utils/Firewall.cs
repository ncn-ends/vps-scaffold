using static App.Utils.ShellController;

namespace App.Utils;

public static class Firewall
{
    public static async Task OpenNginxPorts()
    {
        ColorPrinter.Working("Configuring firewall for HTTP requests...");
        
        await Execute(new[] {"sudo", "ufw", "allow", @"Nginx HTTP"}, silently: true);

        ColorPrinter.WorkCompleted("Firewall configuration complete.");
    }

    public static async Task OpenSshPorts()
    {
        ColorPrinter.Working("Configuring firewall for SSH...");
        
        await Execute(new[] {"ufw", "allow", "OpenSSH"}, silently: true);
        await Execute(new[] {"ufw", "--force", "enable"}, silently: true);

        ColorPrinter.WorkCompleted("Firewall configuration complete.");
    }
}