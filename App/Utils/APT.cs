using App.State;
using App.Templates;
using CliWrap;
using CliWrap.Buffered;

namespace App.Utils;

public static class APT
{
    private static async Task UpdatePackages()
    {
        var password = Store.Password;

        var sudoAptUpdate = password | Cli.Wrap("sudo").WithArguments("apt update");
        await sudoAptUpdate.ExecuteBufferedAsync();
    }

    public static async Task InstallPackage(string package)
    {
        var password = Store.Password;

        await UpdatePackages();

        var aptInstallBuildEssential = password | Cli.Wrap("sudo")
            .WithArguments(new[] {"apt", "install", package, "-y"});
        await aptInstallBuildEssential.ExecuteBufferedAsync();
    }
}