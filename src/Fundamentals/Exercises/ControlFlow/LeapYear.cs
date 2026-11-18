using ConsoleUtils;

namespace Fundamentals.Exercises.ControlFlow;

public sealed class LeapYear : IExercise
{
    public string Title => "Leap year";

    public string Description => "Divisible by 4, except centuries, except centuries divisible by 400.";

    public void Run()
    {
        int year = ConsoleInput.ReadInt("Year:", 1, 9999);
        Console.WriteLine(IsLeapYear(year) ? $"{year} is a leap year." : $"{year} is not a leap year.");
    }

    public static bool IsLeapYear(int year) => (year % 4 == 0 && year % 100 != 0) || year % 400 == 0;
}
