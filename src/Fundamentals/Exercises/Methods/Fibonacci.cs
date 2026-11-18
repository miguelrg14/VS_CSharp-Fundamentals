using ConsoleUtils;

namespace Fundamentals.Exercises.Methods;

public sealed class Fibonacci : IExercise
{
    /// <summary>F(92) is the largest Fibonacci number that fits in a long.</summary>
    public const int MaxTerm = 92;

    public string Title => "Fibonacci sequence";

    public string Description => "Compares the naive recursive definition with an O(n) loop.";

    public void Run()
    {
        int count = ConsoleInput.ReadInt($"How many terms (1-{MaxTerm + 1})?", 1, MaxTerm + 1);
        Console.WriteLine(string.Join(", ", Sequence(count)));

        int n = ConsoleInput.ReadInt("Compute F(n) recursively for n (0-35):", 0, 35);
        Console.WriteLine($"F({n}) = {Recursive(n)}  (each call makes two more, so this is O(2^n))");
    }

    /// <summary>F(0) = 0, F(1) = 1, F(n) = F(n - 1) + F(n - 2).</summary>
    public static long Recursive(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        return n < 2 ? n : Recursive(n - 1) + Recursive(n - 2);
    }

    /// <summary>The first <paramref name="count"/> terms, keeping only the last two values.</summary>
    public static IEnumerable<long> Sequence(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(count, MaxTerm + 1);

        long current = 0;
        long next = 1;
        for (int i = 0; i < count; i++)
        {
            yield return current;
            (current, next) = (next, current + next);
        }
    }
}
