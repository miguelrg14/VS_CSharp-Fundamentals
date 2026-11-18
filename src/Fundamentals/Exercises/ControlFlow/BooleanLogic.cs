using ConsoleUtils;

namespace Fundamentals.Exercises.ControlFlow;

public sealed class BooleanLogic : IExercise
{
    private const int Threshold = 50;

    public string Title => "Boolean logic and conditionals";

    public string Description => $"Evaluates boolean expressions and checks how many of three numbers are above {Threshold}.";

    public void Run()
    {
        // && has higher precedence than ||, so this is: false || (true && true).
        bool precedence = 2 > 5 || 4 == 4 && 6 <= 7;
        Console.WriteLine($"2 > 5 || 4 == 4 && 6 <= 7  ->  {precedence}");

        int number = ConsoleInput.ReadInt("Type any whole number:");
        Console.WriteLine($"number == 10    -> {number == 10}");
        Console.WriteLine($"number != 5     -> {number != 5}");
        Console.WriteLine($"!(number == 5)  -> {!(number == 5)}  (same as the line above)");

        Console.WriteLine();
        int[] numbers =
        [
            ConsoleInput.ReadInt("First number:"),
            ConsoleInput.ReadInt("Second number:"),
            ConsoleInput.ReadInt("Third number:"),
        ];
        Console.WriteLine(DescribeAboveThreshold(numbers));
    }

    /// <summary>Counting first avoids writing one branch per combination of numbers.</summary>
    public static string DescribeAboveThreshold(IReadOnlyCollection<int> numbers)
    {
        int above = numbers.Count(n => n > Threshold);
        return above switch
        {
            0 => $"None of them is above {Threshold}.",
            1 => $"Only one of them is above {Threshold}.",
            _ when above == numbers.Count => $"All of them are above {Threshold}.",
            _ => $"{above} of them are above {Threshold}.",
        };
    }
}
