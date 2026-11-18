using System.Text;
using ConsoleUtils;

namespace Fundamentals.Exercises.StringsAndArrays;

public sealed class StringToolkit : IExercise
{
    public string Title => "String toolkit";

    public string Description => "Iterates over characters, reverses text, replaces words and applies a Caesar cipher.";

    public void Run()
    {
        string text = ConsoleInput.ReadString("Type a sentence (try one with the word \"game\"):");

        Console.WriteLine($"Characters: {string.Join(' ', text.ToCharArray())}");
        Console.WriteLine($"Length:     {text.Length}");
        Console.WriteLine($"Reversed:   {Reverse(text)}");
        Console.WriteLine($"Replaced:   {text.Replace("game", "video game", StringComparison.OrdinalIgnoreCase)}");
        Console.WriteLine($"Caesar +1:  {CaesarShift(text, 1)}");
        Console.WriteLine($"Caesar -1:  {CaesarShift(CaesarShift(text, 1), -1)}  (decoding gives the original back)");
    }

    /// <summary>A StringBuilder avoids creating a new string on every iteration.</summary>
    public static string Reverse(string text)
    {
        var builder = new StringBuilder(text.Length);
        for (int i = text.Length - 1; i >= 0; i--)
        {
            builder.Append(text[i]);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Shifts English letters by <paramref name="shift"/> positions, wrapping around ('z' + 1 = 'a').
    /// Everything else (spaces, digits, punctuation) is left untouched.
    /// </summary>
    public static string CaesarShift(string text, int shift)
    {
        const int AlphabetLength = 26;
        int normalizedShift = ((shift % AlphabetLength) + AlphabetLength) % AlphabetLength;

        var builder = new StringBuilder(text.Length);
        foreach (char c in text)
        {
            char first = char.IsAsciiLetterLower(c) ? 'a' : char.IsAsciiLetterUpper(c) ? 'A' : '\0';
            builder.Append(first == '\0' ? c : (char)(first + ((c - first + normalizedShift) % AlphabetLength)));
        }

        return builder.ToString();
    }
}
