using ConsoleUtils;

namespace Fundamentals.Exercises.Methods;

public sealed class RefAndOutParameters : IExercise
{
    public string Title => "ref and out parameters";

    public string Description => "ref lets a method modify the caller's variable; out lets it return several values.";

    public void Run()
    {
        string sentence = ConsoleInput.ReadString("Type a sentence:");
        AppendOh(ref sentence);
        Console.WriteLine($"After AppendOh(ref sentence): \"{sentence}\"");

        double number = ConsoleInput.ReadDouble("Type a decimal number:");
        SplitNumber(number, out long whole, out double fraction);
        Console.WriteLine($"Whole part: {whole}, fractional part: {fraction:0.####}, has decimals: {fraction != 0}");

        var pair = new NumberPair();
        pair.Store(12, 32);
        pair.Read(out int first, out int second);
        Console.WriteLine($"Pair read with out parameters: ({first}, {second})");

        (int a, int b) = pair; // The same values through a deconstructor.
        Console.WriteLine($"Pair deconstructed: ({a}, {b})");
    }

    public static void AppendOh(ref string sentence) => sentence += ", oh";

    /// <summary>Splits 3.75 into 3 and 0.75 (and -3.75 into -3 and -0.75).</summary>
    public static void SplitNumber(double number, out long whole, out double fraction)
    {
        whole = (long)Math.Truncate(number);
        fraction = number - whole;
    }
}

/// <summary>Stores two numbers privately and gives them back through out parameters.</summary>
public sealed class NumberPair
{
    private int _first;
    private int _second;

    public void Store(int first, int second)
    {
        _first = first;
        _second = second;
    }

    public void Read(out int first, out int second)
    {
        first = _first;
        second = _second;
    }

    public void Deconstruct(out int first, out int second) => Read(out first, out second);

    public override string ToString() => $"({_first}, {_second})";
}
