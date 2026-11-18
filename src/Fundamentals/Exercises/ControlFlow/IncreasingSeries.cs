using ConsoleUtils;

namespace Fundamentals.Exercises.ControlFlow;

public sealed class IncreasingSeries : IExercise
{
    public string Title => "Increasing series";

    public string Description => "Type a strictly increasing series of numbers (a negative number ends it). Reports the biggest gap between two consecutive numbers.";

    public void Run()
    {
        var numbers = new List<int>();
        while (true)
        {
            int number = ConsoleInput.ReadInt("Number:");
            if (number < 0)
            {
                break;
            }

            if (numbers.Count > 0 && number <= numbers[^1])
            {
                Console.WriteLine($"Invalid series: {number} is not greater than {numbers[^1]}.");
                return;
            }

            numbers.Add(number);
        }

        Console.WriteLine(numbers.Count < 2
            ? "At least two numbers are needed to measure a gap."
            : $"The biggest gap is {MaxGap(numbers)}.");
    }

    /// <summary>Largest difference between consecutive items of an increasing series.</summary>
    public static int MaxGap(IReadOnlyList<int> series)
    {
        if (series.Count < 2)
        {
            throw new ArgumentException("The series needs at least two numbers.", nameof(series));
        }

        int maxGap = 0;
        for (int i = 1; i < series.Count; i++)
        {
            if (series[i] <= series[i - 1])
            {
                throw new ArgumentException("The series must be strictly increasing.", nameof(series));
            }

            maxGap = Math.Max(maxGap, series[i] - series[i - 1]);
        }

        return maxGap;
    }
}
