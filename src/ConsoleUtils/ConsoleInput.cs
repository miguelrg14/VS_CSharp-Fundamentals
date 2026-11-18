using System.Globalization;

namespace ConsoleUtils;

/// <summary>
/// Validated console input. Every method keeps asking until the user types a valid value,
/// so callers never have to deal with <see cref="FormatException"/> or out-of-range values.
/// </summary>
/// <remarks>
/// Reads from <see cref="Console.In"/> and writes to <see cref="Console.Out"/>, so tests can
/// redirect both with <see cref="Console.SetIn"/> and <see cref="Console.SetOut"/>.
/// </remarks>
public static class ConsoleInput
{
    /// <summary>Reads an integer in the inclusive range [<paramref name="min"/>, <paramref name="max"/>].</summary>
    public static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
    {
        while (true)
        {
            string text = ReadLine(prompt);
            if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value)
                && value >= min && value <= max)
            {
                return value;
            }

            Console.WriteLine(RangeMessage("a whole number", min, max));
        }
    }

    /// <summary>Reads a decimal number. Both "7.5" and "7,5" are accepted.</summary>
    public static double ReadDouble(string prompt, double min = double.MinValue, double max = double.MaxValue)
    {
        while (true)
        {
            string text = ReadLine(prompt).Replace(',', '.');
            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double value)
                && value >= min && value <= max)
            {
                return value;
            }

            Console.WriteLine(min == double.MinValue && max == double.MaxValue
                ? "Please enter a number."
                : $"Please enter a number between {min.ToString(CultureInfo.InvariantCulture)} and {max.ToString(CultureInfo.InvariantCulture)}.");
        }
    }

    /// <summary>Reads a line of text. Empty input is rejected unless <paramref name="allowEmpty"/> is true.</summary>
    public static string ReadString(string prompt, bool allowEmpty = false)
    {
        while (true)
        {
            string text = ReadLine(prompt).Trim();
            if (allowEmpty || text.Length > 0)
            {
                return text;
            }

            Console.WriteLine("Please type something.");
        }
    }

    /// <summary>Asks a yes/no question. Accepts y/yes/n/no in any case.</summary>
    public static bool ReadYesNo(string prompt)
    {
        while (true)
        {
            switch (ReadLine($"{prompt} (y/n)").Trim().ToLowerInvariant())
            {
                case "y" or "yes":
                    return true;
                case "n" or "no":
                    return false;
                default:
                    Console.WriteLine("Please answer 'y' or 'n'.");
                    break;
            }
        }
    }

    /// <summary>Waits for the user to press Enter.</summary>
    public static void Pause(string message = "Press Enter to continue...")
    {
        Console.WriteLine(message);
        Console.In.ReadLine();
    }

    private static string ReadLine(string prompt)
    {
        Console.Write($"{prompt} ");
        // A closed input stream (end of a redirected file) would otherwise loop forever.
        return Console.In.ReadLine() ?? throw new EndOfStreamException("No more console input.");
    }

    private static string RangeMessage(string what, int min, int max) => (min, max) switch
    {
        (int.MinValue, int.MaxValue) => $"Please enter {what}.",
        (_, int.MaxValue) => $"Please enter {what} greater than or equal to {min}.",
        (int.MinValue, _) => $"Please enter {what} less than or equal to {max}.",
        _ => $"Please enter {what} between {min} and {max}.",
    };
}
