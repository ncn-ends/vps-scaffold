using System.Runtime.InteropServices;
using App.State;
using App.Templates;
using CliWrap;
using CliWrap.EventStream;

namespace App.Utils;

public static class ShellController
{
    public static async Task SourceBashrc()
    {
        var username = Store.Username;
        
        var content = FileTemplates.SourceBashrcScript();
        var pathToFile = $"/home/{username}";
        var fileName = "source-bashrc.sh";

        await FileSystem.CreateScriptAndRun(content, pathToFile, fileName, cleanup: true, args: $"/home/{username}/.bashrc");
    }
    
    public static async Task Execute(string cmd, bool silently = false)
    {
        if (cmd is null) throw new Exception("Bad argument. Commands can't be null.");

        var parsedCmd = cmd.Split(" ");
        await Execute(parsedCmd, silently);
    }

    public static async Task Execute(string[] cmds, bool silently = false)
    {
        if (cmds is null) throw new Exception("Bad argument. Commands can't be null.");

        var results = Cli.Wrap(cmds[0]).WithArguments(cmds.Skip(1));

        await ProcessOutput(results, silently);
    }

    private static async Task ProcessOutput(Command results, bool silently)
    {
        await foreach (var cmdEvent in results.ListenAsync())
        {
            switch (cmdEvent)
            {
                case StandardOutputCommandEvent stdOut:
                    if (!silently) Console.WriteLine($"{stdOut.Text}");
                    break;
                case StandardErrorCommandEvent stdErr:
                    if (!silently) Console.WriteLine($"Error: {stdErr.Text}");
                    break;
                case ExitedCommandEvent exited:
                    // Console.Write(">>> ");
                    break;
            }

            if (silently)
            {
                Thread.Sleep(500);
            }
        }
    }
}