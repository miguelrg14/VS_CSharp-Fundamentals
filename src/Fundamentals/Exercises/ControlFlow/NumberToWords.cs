using ConsoleUtils;

namespace Fundamentals.Exercises.ControlFlow;

public sealed class NumberToWords : IExercise
{
    private static readonly string[] Units =
    [
        "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
        "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen",
    ];

    private static readonly string[] Tens = ["", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"];

    public string Title => "Number to words";

    public string Description => "Writes numbers from 0 to 99 in English words. Type -1 to finish.";

    public void Run()
    {
        while (true)
        {
            int number = ConsoleInput.ReadInt("Number (0-99, -1 to finish):", -1, 99);
            if (number == -1)
            {
                return;
            }

            Console.WriteLine(ToWords(number));
        }
    }

    /// <summary>Lookup tables instead of one if/else branch per number.</summary>
    public static string ToWords(int number)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(number);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(number, 99);

        if (number < Units.Length)
        {
            return Units[number];
        }

        int units = number % 10;
        return units == 0 ? Tens[number / 10] : $"{Tens[number / 10]}-{Units[units]}";
    }
}
