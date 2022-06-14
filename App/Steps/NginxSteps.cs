using System.Drawing;
using App.Static;
using App.Utils;
using CliWrap;
using CliWrap.Buffered;
using Pastel;
using static App.Utils.CLI;

namespace App.Steps;

// CopyFile(Files.SampleIndexHtml, /var/www/{domainName}/html/index.html)
// CopyFile(Files.ServerBlockSampleIndexHtml, /etc/nginx/sites-available/{domainName}
// sudo ln -s /etc/nginx/sites-available/{domainName} /etc/nginx/sites-enabled/
// EditLine(/etc/nginx/nginx.conf, "server_names_hash_bucket_size", "server_names_hash_bucket_size 64;")
// sudo nginx -t
// sudo systemctl restart nginx

public static class NginxSteps
{
    private static async Task InstallNginx()
    {
        Console.WriteLine("Installing Nginx...".Pastel(Color.Teal));
        await APT.InstallPackage("nginx");
    }

    private static void ConfirmBasicSetup()
    {
        var currentIp = Data.CurrentIp;

        Console.WriteLine("Verify that the server is accessible by navigating to it via your client machine's browser."
            .Pastel(Color.Gold));
        Console.WriteLine($"\tGo to: http://{currentIp}/".Pastel(Color.Gold));
        Console.WriteLine("You should be greeted by the default Nginx greeting page.");
        Speaker.SayPressAnyKey();
    }

    private static async Task SwitchToNewUser()
    {
        var password = Data.Password;
        var username = Data.Username;
        await (password | Cli.Wrap("su").WithArguments(username)).ExecuteBufferedAsync();
    }

    private static async Task SetUpNginxDirectories()
    {
        var currentIp = Data.CurrentIp;
        var domainName = currentIp;
        var password = Data.Password;

        await (password | Cli.Wrap($"sudo mkdir -p /var/www/{domainName}/html"))
            .ExecuteBufferedAsync();

        await (password | Cli.Wrap($"sudo chown -R $USER:$USER /var/www/{domainName}/html"))
            .ExecuteBufferedAsync();

        await (password | Cli.Wrap($"sudo chmod -R 755 /var/www/{domainName}"))
            .ExecuteBufferedAsync();
    }

    public static async Task PerformAll()
    {
        Console.WriteLine("Beginning Nginx configuration steps...".Pastel(Color.Teal));

        await InstallNginx();
        await FirewallSteps.PerformNginxSteps();
        ConfirmBasicSetup();

        Console.WriteLine("Configuring Nginx server block...".Pastel(Color.Teal));
        
        await SwitchToNewUser();
        await SetUpNginxDirectories();

    }
}