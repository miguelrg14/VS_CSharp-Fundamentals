using ConsoleUtils;

namespace Fundamentals.Exercises.ControlFlow;

public sealed class ReverseTwoDigits : IExercise
{
    public string Title => "Reverse a two-digit number";

    public string Description => "Splits a number into its digits with / and % and swaps them.";

    public void Run()
    {
        int number = ConsoleInput.ReadInt("Two-digit number:", 10, 99);
        Console.WriteLine($"Reversed: {Reverse(number)}");
    }

    /// <summary>Returns a string so that 10 becomes "01" instead of losing the leading zero.</summary>
    public static string Reverse(int number)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(number, 10);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(number, 99);
        return $"{number % 10}{number / 10}";
    }
}
