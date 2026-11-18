using ConsoleUtils;

namespace Fundamentals.Exercises.StringsAndArrays;

public sealed class RandomArrayStats : IExercise
{
    public string Title => "Random array statistics";

    public string Description => "Fills arrays with random and consecutive numbers and computes their sum and average.";

    public void Run()
    {
        int length = ConsoleInput.ReadInt("Number of elements:", 1, 1000);
        int min = ConsoleInput.ReadInt("Minimum value:");
        int max = ConsoleInput.ReadInt("Maximum value:", min);

        int[] randomNumbers = RandomArray(length, min, max, Random.Shared);
        Console.WriteLine($"Random array: [{string.Join(", ", randomNumbers)}]");
        Console.WriteLine($"Sum: {Sum(randomNumbers)}, average: {Average(randomNumbers):0.##}");

        int[] oneToHundred = Consecutive(1, 100);
        Console.WriteLine($"Numbers 1-100 -> sum: {Sum(oneToHundred)}, average: {Average(oneToHundred):0.##}");
    }

    /// <summary>The Random instance is passed in, so tests can use a fixed seed.</summary>
    public static int[] RandomArray(int length, int min, int max, Random random)
    {
        int[] numbers = new int[length];
        for (int i = 0; i < length; i++)
        {
            numbers[i] = random.Next(min, max + 1); // Upper bound is exclusive.
        }

        return numbers;
    }

    public static int[] Consecutive(int from, int to)
    {
        int[] numbers = new int[to - from + 1];
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = from + i;
        }

        return numbers;
    }

    /// <summary>Uses a long so that large arrays cannot overflow the total.</summary>
    public static long Sum(IEnumerable<int> numbers)
    {
        long total = 0;
        foreach (int number in numbers)
        {
            total += number;
        }

        return total;
    }

    /// <summary>
    /// Returns a double: with integer division the average of 1 and 2 would be 1.
    /// </summary>
    public static double Average(IReadOnlyCollection<int> numbers) =>
        numbers.Count == 0 ? throw new ArgumentException("The array is empty.", nameof(numbers)) : (double)Sum(numbers) / numbers.Count;
}
