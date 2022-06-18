using System.Drawing;
using App.State;
using App.Terminal;
using App.Templates;
using App.Utils;
using CliWrap;
using CliWrap.Buffered;

namespace App.Steps;

public static class NginxSteps
{
    private static async Task InstallNginx()
    {
        ColorPrinter.Working("Installing Nginx...");
        await APT.InstallPackage("nginx");
    }

    private static void TellUserToConfirmBasicSetup()
    {
        var currentIp = AppStore.CurrentIp;

        ColorPrinter.CallToAction(
            "Verify that the server is accessible by navigating to it via your client machine's browser.");
        ColorPrinter.CallToAction($"\tGo to: http://{currentIp}/");
        ColorPrinter.CallToAction("You should be greeted by the default Nginx greeting page.");
        Prompter.PromptUserForAnyKey();
    }

    private static async Task SwitchToCreatedUser()
    {
        var username = AppStore.Username;
        await Cli.Wrap("su").WithArguments(username).ExecuteBufferedAsync();
    }

    private static async Task SetUpNginxDirectories()
    {
        var domainName = AppStore.DomainName;
        
        var password = AppStore.Password;
        var username = AppStore.Username;

        await (password | Cli.Wrap("sudo").WithArguments(new[]
            {
                "mkdir",
                "-p",
                $"/var/www/{domainName}/html"
            }))
            .ExecuteBufferedAsync();

        await (password | Cli.Wrap("sudo")
                .WithArguments(new[]
                {
                    "chown",
                    "-R",
                    $"{username}:{username}",
                    $"/var/www/{domainName}/html"
                }))
            .ExecuteBufferedAsync();

        await (password | Cli.Wrap("sudo").WithArguments(new[]
            {
                "chmod",
                "-R",
                "755",
                $"/var/www/{domainName}"
            }))
            .ExecuteBufferedAsync();

        await FileSystem.WriteFile(
            FileTemplates.DefaultHtmlFile(),
            $"/var/www/{domainName}/html",
            "index.html"
        );

        await FileSystem.WriteFile(
            FileTemplates.NginxServerBlock(domainName),
            "/etc/nginx/sites-available",
            domainName
        );

        await (password | Cli.Wrap("sudo"))
            .WithArguments($"ln -s -f /etc/nginx/sites-available/{domainName} /etc/nginx/sites-enabled/")
            .ExecuteBufferedAsync();
    }

    private static async Task VerifyNginx()
    {
        var password = AppStore.Password;

        await (password | Cli.Wrap("sudo").WithArguments(new[] {"nginx", "-t"}))
            .ExecuteBufferedAsync();

        await (password | Cli.Wrap("sudo").WithArguments("systemctl restart nginx"))
            .ExecuteBufferedAsync();
    }

    private static void TellUserToSetUpDns()
    {
        ColorPrinter.CallToAction("If you haven't done so yet, setup the domain to point to this IP.");
        Prompter.PromptUserForAnyKey();
    }
    
    private static void PromptForDomainName()
    {
        var domainName = Prompter.PromptUser("Enter domain name: ");
        ColorPrinter.Working($"Setting domain name to: {domainName}");
        if (domainName != null) AppStore.DomainName = domainName;
    }

    public static async Task PerformAll()
    {
        ColorPrinter.Working("Beginning Nginx configuration steps...");

        await InstallNginx();
        await Firewall.ConfigureHttpPorts();
        if (!AppStore.FlagStore.AsMinimal) TellUserToConfirmBasicSetup();

        ColorPrinter.Working("Configuring Nginx server block...");

        if (!AppStore.FlagStore.AsMinimal) TellUserToSetUpDns();
        
        if (!AppStore.FlagStore.AsNoDomain) PromptForDomainName();

        await SwitchToCreatedUser();
        await SetUpNginxDirectories();

        await FileSystem.EditLine(
            "/etc/nginx/nginx.conf",
            "server_names_hash_bucket_size",
            "server_names_hash_bucket_size 64;"
        );

        await VerifyNginx();

        ColorPrinter.WorkCompleted("Nginx set up complete.");
    }
}