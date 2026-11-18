using System.Globalization;

namespace Fundamentals.Exercises.Basics;

public sealed class CastingAndConversion : IExercise
{
    public string Title => "Casting and conversion";

    public string Description => "Implicit vs explicit casts, and converting text to numbers safely.";

    public void Run()
    {
        // Implicit casts are allowed when no information can be lost.
        int smallNumber = 15;
        float asFloat = smallNumber;
        double asDouble = asFloat;
        Console.WriteLine($"int {smallNumber} -> float {asFloat} -> double {asDouble} (implicit)");

        // Explicit casts are required when information may be lost: the decimals are truncated.
        double precise = 10.99;
        int truncated = (int)precise;
        float lessPrecise = (float)precise;
        Console.WriteLine($"(int){precise} = {truncated}, (float){precise} = {lessPrecise} (explicit)");

        // Mixing types: the result takes the "widest" type of the expression.
        var intPlusFloat = 1 + 2.5f;
        var floatPlusDouble = 2.5f + 1.0;
        Console.WriteLine($"int + float    -> {intPlusFloat.GetType().Name}");
        Console.WriteLine($"float + double -> {floatPlusDouble.GetType().Name}");
        Console.WriteLine($"\"Total: \" + 3 -> {("Total: " + 3).GetType().Name} \"Total: 3\"");

        // A string cannot be cast to a number: it has to be parsed.
        string text = "42";
        int parsed = int.Parse(text, CultureInfo.InvariantCulture);
        Console.WriteLine($"int.Parse(\"{text}\") = {parsed}");

        // TryParse is the safe option for user input: it reports failure instead of throwing.
        foreach (string candidate in new[] { "123", "12.5", "abc" })
        {
            bool ok = int.TryParse(candidate, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value);
            Console.WriteLine($"int.TryParse(\"{candidate}\") -> {(ok ? $"success, {value}" : "failed")}");
        }
    }
}
