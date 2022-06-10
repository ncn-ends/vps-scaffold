using CliWrap;
using CliWrap.EventStream;

namespace App.Utils;

public static class Executor
{
    public static async Task Execute(string cmd)
    {
        var results = Cli.Wrap(cmd);

        await ProcessOutput(results);
    }

    public static async Task Execute(string[] cmds)
    {
        if (cmds is null) throw new Exception("Bad argument. Commands can't be null.");
        
        var results = Cli.Wrap(cmds[0]).WithArguments(cmds.Skip(1));
        
        await ProcessOutput(results);
    }

    private static async Task ProcessOutput(Command results)
    {
        await foreach (var cmdEvent in results.ListenAsync())
        {
            switch (cmdEvent)
            {
                case StandardOutputCommandEvent stdOut:
                    Console.WriteLine($"{stdOut.Text}");
                    if (stdOut.Text.Contains("Proceed") && stdOut.Text.Contains("?"))
                    {
                        var asd = Console.ReadLine();
                        Thread.Sleep(2000);
                    }
                    break;
                case StandardErrorCommandEvent stdErr:
                    Console.WriteLine($"Error: {stdErr.Text}");
                    break;
                case ExitedCommandEvent exited:
                    // Console.Write(">>> ");
                    break;
            }
            Thread.Sleep(500);
        }
    }
}