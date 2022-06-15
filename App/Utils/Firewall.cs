using System.Drawing;
using Pastel;
using static App.Utils.ShellController;

namespace App.Utils;

public static class Firewall
{
    
    public static async Task OpenNginxPorts()
    {
        Console.WriteLine("Configuring firewall for HTTP requests...".Pastel(Color.Teal));
        await Execute(new[] {"sudo", "ufw", "allow", @"Nginx HTTP"}, silently: true);

        Console.WriteLine("Firewall configuration complete.".Pastel(Color.Chartreuse));
    }
    
    public static async Task OpenSshPorts()
    {
        Console.WriteLine("Configuring firewall for SSH...".Pastel(Color.Teal));
        await Execute(new[] {"ufw", "allow", "OpenSSH"}, silently: true);
        await Execute(new[] {"ufw", "--force", "enable"}, silently: true);

        Console.WriteLine("Firewall configuration complete.".Pastel(Color.Chartreuse));
    }
}