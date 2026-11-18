using ConsoleUtils;

namespace Fundamentals.Exercises.Basics;

public sealed class VariablesAndOutput : IExercise
{
    public string Title => "Variables and output";

    public string Description => "Declares variables of the basic types, changes their values and prints them.";

    public void Run()
    {
        // Declaration and initialization in one line.
        int number = 10;
        float pi = 3.141592f; // Literals with decimals are double by default; the 'f' suffix makes it a float.
        string message = "Hello world";
        bool isLearning = true;

        Console.WriteLine($"int    number     = {number}");
        Console.WriteLine($"float  pi         = {pi}");
        Console.WriteLine($"string message    = \"{message}\"");
        Console.WriteLine($"bool   isLearning = {isLearning}");

        // A variable can change its value, but never its type.
        message = "Hello everyone";
        Console.WriteLine($"After reassigning: message = \"{message}\"");

        string name = ConsoleInput.ReadString("What is your name?");
        Console.WriteLine($"Nice to meet you, {name}! Your name has {name.Length} characters.");
    }
}
