using System.Drawing;
using App.Static;
using App.Utils;
using CliWrap;
using CliWrap.Buffered;
using Pastel;
using static App.Utils.CLI;

namespace App.Steps;

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

    private static async Task SwitchToCreatedUser()
    {
        var password = Data.Password;
        var username = Data.Username;
        await (password | Cli.Wrap("su").WithArguments(username)).ExecuteBufferedAsync();
    }

    private static async Task SetUpNginxDirectories()
    {
        var domainName = Data.CurrentIp;
        var password = Data.Password;

        await (password | Cli.Wrap($"sudo mkdir -p /var/www/{domainName}/html"))
            .ExecuteBufferedAsync();

        await (password | Cli.Wrap($"sudo chown -R $USER:$USER /var/www/{domainName}/html"))
            .ExecuteBufferedAsync();

        await (password | Cli.Wrap($"sudo chmod -R 755 /var/www/{domainName}"))
            .ExecuteBufferedAsync();

        await FileSystem.WriteFile(
            StaticFileText.DefaultHtmlFile(),
            $"/var/www/{domainName}/html",
            "index.html"
        );

        await FileSystem.WriteFile(
            StaticFileText.NginxServerBlock(domainName),
            "/etc/nginx/sites-available",
            domainName
        );

        await (password | Cli.Wrap($"sudo ln -s /etc/nginx/sites-available/{domainName} /etc/nginx/sites-enabled/"))
            .ExecuteBufferedAsync();
    }

    private static async Task VerifyNginx()
    {
        var password = Data.Password;

        await (password | Cli.Wrap("sudo nginx -t"))
            .ExecuteBufferedAsync();

        await (password | Cli.Wrap("sudo systemctl restart nginx"))
            .ExecuteBufferedAsync();
    }

    public static async Task PerformAll()
    {
        Console.WriteLine("Beginning Nginx configuration steps...".Pastel(Color.Teal));

        await InstallNginx();
        await FirewallSteps.PerformNginxSteps();
        ConfirmBasicSetup();

        Console.WriteLine("Configuring Nginx server block...".Pastel(Color.Teal));

        await SwitchToCreatedUser();
        await SetUpNginxDirectories();
        
        await FileSystem.EditLine(
            "/etc/nginx/nginx.conf",
            "server_names_hash_bucket_size",
            "\tserver_names_hash_bucket_size 64;"
        );

        await VerifyNginx();

        Console.WriteLine("Nginx set up complete.".Pastel(Color.Chartreuse));
    }
}