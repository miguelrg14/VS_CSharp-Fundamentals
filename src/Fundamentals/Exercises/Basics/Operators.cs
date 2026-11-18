namespace Fundamentals.Exercises.Basics;

public sealed class Operators : IExercise
{
    public string Title => "Arithmetic operators";

    public string Description => "Shows operator precedence, compound assignment and prefix vs postfix increments.";

    public void Run()
    {
        int a = 10;
        int b = 20;
        int c = -15;

        int result = a + b;
        Show("a + b", result);                 // 30
        result += 15;
        Show("result += 15", result);          // 45
        result++;
        Show("result++", result);              // 46
        result = a + b * c;
        Show("a + b * c", result);             // -290: multiplication goes first
        result = 5 % 3;
        Show("5 % 3", result);                 // 2: remainder of the division

        result = a + ++b;
        Show("a + ++b", result, $"b = {b}");   // 31: b is incremented BEFORE the sum
        result = a + b++;
        Show("a + b++", result, $"b = {b}");   // 31: b is incremented AFTER the sum
        result *= 10;
        Show("result *= 10", result);          // 310

        string hello = "Hello ";
        string world = "World!";
        Console.WriteLine($"{"\"Hello \" + \"World!\"",-22} = {hello + world}   (+ concatenates strings)");

        Console.WriteLine();
        Console.WriteLine("Given w = 9, x = 5, y = 2, z = -3:");
        int w = 9, x = 5, y = 2, z = -3;
        int total = (w + x * z) / ++y;
        Show("(w + x * z) / ++y", total);      // (9 - 15) / 3 = -2
        total += 8 * x;
        Show("total += 8 * x", total);         // 38
        total *= 2;
        Show("total *= 2", total);             // 76
        float half = total / 0.5f;
        Console.WriteLine($"{"total / 0.5f",-22} = {half}   (int / float gives a float)"); // 152
    }

    private static void Show(string expression, int value, string? note = null) =>
        Console.WriteLine($"{expression,-22} = {value}{(note is null ? "" : $"   ({note})")}");
}
