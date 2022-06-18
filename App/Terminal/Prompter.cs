namespace App.Terminal;

public class Prompter
{
    public static void PromptUserForAnyKey()
    {
        ColorPrinter.CallToAction("\t(Once you're finished, press any key to continue...)");
        Console.ReadKey();
    }
    
    public static string? PromptUserAsHidden(string prompt)
    {
        ColorPrinter.CallToAction(prompt);
        string? password = null;

        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
                break;
            password += key.KeyChar;
        }

        return password;
    }

    public static string? PromptUser(string prompt)
    {
        ColorPrinter.CallToAction(prompt);
        var promptAnswer = Console.ReadLine();

        return promptAnswer;
    }
}