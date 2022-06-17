using App.Terminal;
using App.Static;
using App.Steps;

namespace App.Utils;

public static class StepSequence
{
    public static async Task Begin()
    {
        try
        {
            AppStore.CurrentIp = await IpGrabber.GrabIpNoHttp();

            var (username, password) = await UserSteps.PerformAll();

            AppStore.Username = username;
            AppStore.Password = password;

            await SSHSteps.PerformAll();
            await NginxSteps.PerformAll();
            // await NodeSteps.PerformAll();
        }
        catch (Exception e)
        {
            ColorPrinter.Error(e.ToString());
        }
    }
}