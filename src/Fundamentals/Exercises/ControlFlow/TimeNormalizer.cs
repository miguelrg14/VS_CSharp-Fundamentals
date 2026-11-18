using ConsoleUtils;

namespace Fundamentals.Exercises.ControlFlow;

public sealed class TimeNormalizer : IExercise
{
    public string Title => "Normalize a duration";

    public string Description => "Turns any amount of days, hours, minutes and seconds into a normalized duration (e.g. 90 min -> 1 h 30 min).";

    public void Run()
    {
        int days = ConsoleInput.ReadInt("Days:", 0);
        int hours = ConsoleInput.ReadInt("Hours:", 0);
        int minutes = ConsoleInput.ReadInt("Minutes:", 0);
        int seconds = ConsoleInput.ReadInt("Seconds:", 0);

        (long d, int h, int m, int s) = Normalize(days, hours, minutes, seconds);
        Console.WriteLine($"{d} days, {h} hours, {m} minutes, {s} seconds");
    }

    /// <summary>
    /// Converts everything to seconds and splits it back with division and remainder.
    /// A long is used so large inputs cannot overflow.
    /// </summary>
    public static (long Days, int Hours, int Minutes, int Seconds) Normalize(long days, long hours, long minutes, long seconds)
    {
        long total = (((days * 24) + hours) * 60 + minutes) * 60 + seconds;
        return (total / 86_400, (int)(total % 86_400 / 3_600), (int)(total % 3_600 / 60), (int)(total % 60));
    }
}
