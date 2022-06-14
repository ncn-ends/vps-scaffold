using App.Static;
using App.Utils;

namespace App.Steps;

public static class StepOrganizer
{
    public static async Task Begin()
    {
        Data.CurrentIp = await IpGrabber.GrabIpNoHttp();
        
        var (username, password) = await UserSteps.PerformAll();
        
        Data.Username = username;
        Data.Password = password;
        
        await FirewallSteps.PerformSshSteps();
        await SSHSteps.PerformAll();
        await NginxSteps.PerformAll();
        await NodeSteps.PerformAll();
    }
}