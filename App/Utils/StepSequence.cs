using App.State;
using App.Terminal;
using App.Templates;
using App.Steps;

namespace App.Utils;

public static class StepSequence
{
    public static async Task Begin()
    {
        try
        {
            var ip = await IpGrabber.GrabIpNoHttp();
            AppStore.CurrentIp = ip;
            AppStore.DomainName = ip;

            var (username, password) = await UserSteps.PerformAll();

            AppStore.Username = username;
            AppStore.Password = password;

            await SSHSteps.PerformAll();

            await NginxSteps.PerformAll();
            // await NodeSteps.PerformAll();

            if (AppStore.FlagStore.AsHttps) await SSLSteps.PerformAll();
        }
        catch (Exception e)
        {
            ColorPrinter.Error(e.ToString());
        }
    }
}