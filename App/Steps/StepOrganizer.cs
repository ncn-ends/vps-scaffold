namespace App.Steps;

public static class StepOrganizer
{
    public static async Task Begin()
    {
        var (username, password) = await UserSteps.PerformAll();
        await FirewallSteps.PerformAll();
        await SSHSteps.PerformAll(username, password);
        await NodeSteps.PerformAll(password);
    }
}