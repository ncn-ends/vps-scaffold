using App.Static;
using CliWrap;
using CliWrap.Buffered;

namespace App.Utils;

public static class APT
{
    private static async Task UpdatePackages()
    {
        var password = Data.Password;
        var sudoAptUpdate = password | Cli.Wrap("sudo apt update");
        await sudoAptUpdate.ExecuteBufferedAsync();
    }

    public static async Task InstallPackage(string package)
    {
        var password = Data.Password;
        await UpdatePackages();
        var aptInstallBuildEssential = password | Cli.Wrap("sudo apt install")
            .WithArguments(new[] {package, "-y"});
        await aptInstallBuildEssential.ExecuteBufferedAsync();
    }
}