using App.State;
using App.Terminal;
using App.Templates;
using App.Steps;

namespace App.Utils;

public static class StepSequence
{
    public static async Task Begin(FlagStore flagStore)
    {
        try
        {
            Store.CurrentIp = await IpGrabber.GrabIpNoHttp();

            var (username, password) = await UserSteps.PerformAll();

            Store.Username = username;
            Store.Password = password;

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