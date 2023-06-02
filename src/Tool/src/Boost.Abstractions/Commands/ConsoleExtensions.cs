using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;

namespace Boost.Commands;

public static class ConsoleExtensions
{
    public static void Write(this IConsole console, string value, ConsoleColor color)
    {
        ConsoleColor currentColor = console.ForegroundColor;

        console.ForegroundColor = color;
        console.Write(value);
        console.ForegroundColor = currentColor;
    }

    public static void Write(this IConsole console, string value, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        ConsoleColor currentForegroundColor = console.ForegroundColor;
        ConsoleColor currentBackgroundColor = console.BackgroundColor;

        console.ForegroundColor = foregroundColor;
        console.BackgroundColor = backgroundColor;
        console.Write(value);
        console.ForegroundColor = currentForegroundColor;
        console.BackgroundColor = currentBackgroundColor;
    }

    public static void WriteLine(this IConsole console, string value, ConsoleColor color)
    {
        ConsoleColor currentColor = console.ForegroundColor;

        console.ForegroundColor = color;
        console.WriteLine(value);
        console.ForegroundColor = currentColor;
    }

    public static void WriteIndent(this IConsole console, int indent = 2)
    {
        console.Write(new string(' ', indent));
    }

    public static void ClearLine(this IConsole console)
    {
        console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
    }

    public static int ChooseFromList(this IConsole console, IEnumerable<string> items)
    {
        int nr = 1;

        foreach (var item in items)
        {
            ConsoleColor color = ConsoleColor.Yellow;
            if (nr % 2 == 0)
            {
                color = ConsoleColor.Cyan;
            }

            console.WriteLine($"{nr}. {item}", color);

            nr++;
        }

        console.WriteLine();
        return GetInput(console, items.Count());
    }

    private static int GetInput(IConsole console, int length)
    {
        console.Write("Please choose: ");
        var input = Console.ReadLine();

        if (int.TryParse(input, out int nr))
        {
            if (nr < 1 || nr > length)
            {
                console.WriteLine(
                    $"Invalid input: {nr}, please choose a number between 1 and {length}", ConsoleColor.Red);

                return GetInput(console, length);
            }

            return nr - 1;
        }
        else
        {
            console.WriteLine(
                $"Invalid number: ", ConsoleColor.Red);
            return GetInput(console, length);
        }
    }
}
