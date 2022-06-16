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
}