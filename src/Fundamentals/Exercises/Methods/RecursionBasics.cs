using ConsoleUtils;

namespace Fundamentals.Exercises.Methods;

public sealed class RecursionBasics : IExercise
{
    /// <summary>21! no longer fits in a long.</summary>
    public const int MaxFactorialInput = 20;

    public string Title => "Recursion: countdown and factorial";

    public string Description => "Every recursive method needs a base case that stops it.";

    public void Run()
    {
        int start = ConsoleInput.ReadInt("Count down from:", 0, 1000);
        Console.WriteLine(string.Join(", ", Countdown(start)));

        int n = ConsoleInput.ReadInt($"Factorial of (0-{MaxFactorialInput}):", 0, MaxFactorialInput);
        Console.WriteLine($"{n}! = {FactorialRecursive(n)} (recursive)");
        Console.WriteLine($"{n}! = {FactorialIterative(n)} (iterative)");
    }

    public static IReadOnlyList<int> Countdown(int from)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(from);
        var numbers = new List<int>(from + 1);
        AddCountdown(from, numbers);
        return numbers;
    }

    /// <summary>
    /// Base case is 0! = 1. Stopping at 1 instead would make 0! recurse until the stack overflows.
    /// </summary>
    public static long FactorialRecursive(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(n, MaxFactorialInput);
        return n == 0 ? 1 : n * FactorialRecursive(n - 1);
    }

    /// <summary>
    /// Base case: 0 is added and the recursion stops. Recursive case: add the number, then count down from number - 1.
    /// The list is passed along so every call appends to the same one instead of copying it.
    /// </summary>
    private static void AddCountdown(int number, List<int> numbers)
    {
        numbers.Add(number);
        if (number > 0)
        {
            AddCountdown(number - 1, numbers);
        }
    }

    public static long FactorialIterative(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(n, MaxFactorialInput);

        long result = 1;
        for (int i = 2; i <= n; i++)
        {
            result *= i;
        }

        return result;
    }
}
