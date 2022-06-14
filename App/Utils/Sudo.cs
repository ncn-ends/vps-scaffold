using App.Static;
using CliWrap;
using CliWrap.Buffered;

namespace App.Utils;

public class Sudo
{
    public static async Task AptUpdate()
    {
        var password = Data.Password;
        var sudoAptUpdate = password | Cli.Wrap("sudo apt update");
        await sudoAptUpdate.ExecuteBufferedAsync();
    }
}