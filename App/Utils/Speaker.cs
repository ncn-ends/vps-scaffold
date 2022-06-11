using System.Drawing;
using Pastel;

namespace App.Utils;

public static class Speaker
{
    public static void SayPressAnyKey()
    {
        Console.WriteLine("\tOnce you're finished, press any key to continue...".Pastel(Color.Gold));
        Console.ReadKey();
    }
}