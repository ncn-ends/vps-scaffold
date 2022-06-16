using System.Drawing;
using Pastel;

namespace App.Utils;

public static class Speaker
{
    public static void SayPressAnyKey()
    {
        ColorPrinter.CallToAction("\t(Once you're finished, press any key to continue...)");
        Console.ReadKey();
    }

    /* https://en.wikipedia.org/wiki/Box-drawing_character */
    public static void SayAsHeader(string msg, string subheader = "")
    {
        var horizontalline = new string('═', msg.Length + 4);
        var topline = '╔' + horizontalline + '╗';
        var bottomline = '╚' + horizontalline + '╝';
        var middleline = $"║  {msg}  ║";
        Console.WriteLine(topline.Pastel(Color.DeepSkyBlue));
        Console.WriteLine(middleline.Pastel(Color.DeepSkyBlue));
        Console.WriteLine(bottomline.Pastel(Color.DeepSkyBlue));
        Console.WriteLine(subheader.Pastel(Color.DeepSkyBlue));
    }
}