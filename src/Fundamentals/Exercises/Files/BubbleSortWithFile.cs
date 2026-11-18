using System.Globalization;
using ConsoleUtils;
using DataStructures;

namespace Fundamentals.Exercises.Files;

public sealed class BubbleSortWithFile : IExercise
{
    public const string FileName = "numbers.txt";

    private static readonly char[] Separators = ['/', ' ', ','];

    public string Title => "Bubble sort with file I/O";

    public string Description => $"Reads numbers (from the keyboard or from {FileName}), sorts them with bubble sort and saves the result.";

    public void Run()
    {
        List<int> numbers = File.Exists(FileName) && ConsoleInput.ReadYesNo($"Load the numbers saved in {FileName}?")
            ? ParseNumbers(File.ReadLines(FileName).FirstOrDefault() ?? string.Empty)
            : ReadFromKeyboard();

        if (numbers.Count == 0)
        {
            Console.WriteLine("There is nothing to sort.");
            return;
        }

        Console.WriteLine($"Before: {Format(numbers)}");
        Sorting.BubbleSort(numbers);
        Console.WriteLine($"After:  {Format(numbers)}");

        File.WriteAllLines(FileName, [Format(numbers)]);
        Console.WriteLine($"Sorted numbers saved to {Path.GetFullPath(FileName)}.");
    }

    /// <summary>Parses a line such as "4 / 1 / 3". Anything that is not a number is ignored.</summary>
    public static List<int> ParseNumbers(string line) =>
    [
        .. line.Split(Separators, StringSplitOptions.RemoveEmptyEntries)
            .Select(part => int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out int n) ? n : (int?)null)
            .OfType<int>(),
    ];

    public static string Format(IEnumerable<int> numbers) => string.Join(" / ", numbers);

    private static List<int> ReadFromKeyboard()
    {
        int count = ConsoleInput.ReadInt("How many numbers?", 1, 100);
        return [.. Enumerable.Range(1, count).Select(i => ConsoleInput.ReadInt($"Number {i}:"))];
    }
}
