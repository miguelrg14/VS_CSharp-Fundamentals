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

    /// <summary>Base case: 0. Recursive case: the number followed by the countdown from number - 1.</summary>
    public static IEnumerable<int> Countdown(int from)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(from);
        return from == 0 ? [0] : [from, .. Countdown(from - 1)];
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
