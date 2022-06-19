using App.State;
using App.Terminal;
using App.Utils;
using CliWrap;
using CliWrap.Buffered;

namespace App.Steps;

public static class SSLSteps
{
    public static async Task PerformAll()
    {
        ColorPrinter.Working("Setting up SSL certificate...");
        
        var email = Prompter.PromptUser("Enter your email to be used to register for the SSL certificate:");
        if (email is null) throw new ArgumentException("E-Mail cannot be null.");
        AppStore.Email = email;
        
        await APT.InstallPackage("certbot");
        await APT.InstallPackage("python3-certbot-nginx");
        var autoHttpsRedirectAnswer = AppStore.FlagStore.AsHttp
            ? "1"
            : "2";
        await ($"{email}\nA\nN\n{autoHttpsRedirectAnswer}" | Cli.Wrap("sudo").WithArguments(new[]
            {
                "certbot",
                "--nginx",
                "-d",
                AppStore.DomainName
            }))
            .ExecuteBufferedAsync();
        // TODO: allow for multiple entered domains
        
        ColorPrinter.WorkCompleted("Finished setting up SSL certificate.");
    }
}