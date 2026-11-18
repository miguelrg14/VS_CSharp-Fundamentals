using ConsoleUtils;

namespace Fundamentals.Exercises.StringsAndArrays;

public sealed class ArrayBasics : IExercise
{
    public string Title => "Array basics";

    public string Description => "Fills an array from the keyboard, then finds the maximum, sorts it and reverses it.";

    public void Run()
    {
        int length = ConsoleInput.ReadInt("How many numbers?", 1, 50);
        int[] numbers = new int[length];
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = ConsoleInput.ReadInt($"Value at index {i}:");
        }

        Print("Array", numbers);
        Console.WriteLine($"Maximum: {Max(numbers)}");
        Print("Descending", SortedDescending(numbers));
        Print("Reversed", Reversed(numbers));
        Print("Original (unchanged)", numbers);
    }

    /// <summary>
    /// A single pass. Sorting the array just to read its first item would also work,
    /// but it would silently reorder the caller's data.
    /// </summary>
    public static int Max(IReadOnlyList<int> numbers)
    {
        if (numbers.Count == 0)
        {
            throw new ArgumentException("The array is empty.", nameof(numbers));
        }

        int max = numbers[0];
        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] > max)
            {
                max = numbers[i];
            }
        }

        return max;
    }

    /// <summary>Returns a sorted copy; the input is not modified.</summary>
    public static int[] SortedDescending(IEnumerable<int> numbers)
    {
        int[] copy = [.. numbers];
        Array.Sort(copy, (a, b) => b.CompareTo(a));
        return copy;
    }

    public static int[] Reversed(IReadOnlyList<int> numbers)
    {
        int[] reversed = new int[numbers.Count];
        for (int i = 0; i < numbers.Count; i++)
        {
            reversed[i] = numbers[numbers.Count - 1 - i];
        }

        return reversed;
    }

    private static void Print(string label, IEnumerable<int> numbers) =>
        Console.WriteLine($"{label}: [{string.Join(", ", numbers)}]");
}
