using ConsoleUtils;

namespace Fundamentals.Exercises.ControlFlow;

public sealed class MultiplicationTable : IExercise
{
    public string Title => "Multiplication table and repeated text";

    public string Description => "for loops: a multiplication table up to 20, then a text repeated N times.";

    public void Run()
    {
        int number = ConsoleInput.ReadInt("Number for the multiplication table:");
        for (int i = 1; i <= 20; i++)
        {
            Console.WriteLine($"{number} x {i,2} = {number * i}");
        }

        Console.WriteLine();
        int times = ConsoleInput.ReadInt("How many times should the text be repeated?", 0, 100);
        string text = ConsoleInput.ReadString("Text to repeat:");
        for (int i = 1; i <= times; i++)
        {
            Console.WriteLine($"{i,3}. {text}");
        }
    }
}
