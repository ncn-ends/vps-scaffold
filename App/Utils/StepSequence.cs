using App.Static;
using App.Steps;

namespace App.Utils;

public static class StepSequence
{
    public static async Task Begin()
    {
        Data.CurrentIp = await IpGrabber.GrabIpNoHttp();
        
        var (username, password) = await UserSteps.PerformAll();
        
        Data.Username = username;
        Data.Password = password;
        
        await SSHSteps.PerformAll();
        await NginxSteps.PerformAll();
        await NodeSteps.PerformAll();
    }
}