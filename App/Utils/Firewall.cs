using App.State;
using App.Terminal;
using static App.Utils.ShellController;

namespace App.Utils;

public static class Firewall
{
    public static async Task ConfigureHttpPorts()
    {
        var toOpenBoth = AppStore.FlagStore.AsHttp && AppStore.FlagStore.AsHttps;
        if (toOpenBoth) await OpenBothHttpPorts();
        else if (AppStore.FlagStore.AsHttp) await OpenHttpOnlyPorts();
        else if (AppStore.FlagStore.AsHttps) await OpenHttpsOnlyPorts();
    }
    
    private static async Task OpenHttpOnlyPorts()
    {
        ColorPrinter.Working("Configuring firewall for only HTTP requests...");
        
        await Execute(new[] {"sudo", "ufw", "allow", @"Nginx HTTP"}, silently: true);

        ColorPrinter.WorkCompleted("Firewall configuration complete.");
    }
    
    private static async Task OpenHttpsOnlyPorts()
    {
        ColorPrinter.Working("Configuring firewall for only HTTPS requests...");
        
        await Execute(new[] {"sudo", "ufw", "allow", @"Nginx HTTP"}, silently: true);

        ColorPrinter.WorkCompleted("Firewall configuration complete.");
    }
    
    private static async Task OpenBothHttpPorts()
    {
        ColorPrinter.Working("Configuring firewall for HTTP and HTTPS requests...");
        
        await Execute(new[] {"sudo", "ufw", "allow", @"Nginx Full"}, silently: true);

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