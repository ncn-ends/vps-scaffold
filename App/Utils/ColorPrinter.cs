using System.Drawing;
using Pastel;

namespace App.Utils;

public static class ColorPrinter
{
    public static void CallToAction(string msg)
    {
        Console.WriteLine(msg.Pastel(Color.Gold));
    }

    public static void Working(string msg)
    {
        Console.WriteLine(msg.Pastel(Color.Teal));
    }

    /* zug zug */
    public static void WorkCompleted(string msg)
    {
        Console.WriteLine(msg.Pastel(Color.Chartreuse));
    }

    public static void ImportantInfo(string msg)
    {
        Console.WriteLine(msg.Pastel(Color.Magenta));
    }

    public static void Error(string msg)
    {
        Console.WriteLine(msg.Pastel(Color.Crimson));
    }
}