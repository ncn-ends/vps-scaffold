using System.Drawing;
using Pastel;
using static App.Utils.Executor;

namespace App.Steps;

public static class FirewallSteps
{
    public static async Task PerformAll()
    {
        Console.WriteLine("Setting up firewall...".Pastel(Color.Teal));
        await Execute(new[] {"ufw", "allow", "OpenSSH"}, silently: true);
        await Execute(new[] {"ufw", "--force", "enable"}, silently: true);

        Console.WriteLine("Firewall enabled.".Pastel(Color.Chartreuse));
    }
}