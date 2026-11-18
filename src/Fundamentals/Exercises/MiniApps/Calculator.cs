using ConsoleUtils;
using Fundamentals.Exercises.Methods;

namespace Fundamentals.Exercises.MiniApps;

public sealed class Calculator : IExercise
{
    public enum Operation
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Power,
    }

    private static readonly string[] OperationNames = ["Add", "Subtract", "Multiply", "Divide", "Power"];

    public string Title => "Calculator with memory";

    public string Description => "Basic operations on two numbers. The result can be kept as the first number of the next operation.";

    public void Run()
    {
        double? memory = null;
        while (true)
        {
            double left = memory ?? ConsoleInput.ReadDouble("First number:");
            if (memory is not null)
            {
                Console.WriteLine($"First number (from memory): {left}");
            }

            if (ConsoleMenu.Show("Operation", OperationNames, "Quit") is not int choice)
            {
                return;
            }

            var operation = (Operation)choice;
            double right = operation == Operation.Power
                ? ConsoleInput.ReadInt("Exponent:", -64, 64)
                : ConsoleInput.ReadDouble("Second number:");

            if (!TryCalculate(left, right, operation, out double result, out string? error))
            {
                Console.WriteLine(error);
                memory = null;
                continue;
            }

            Console.WriteLine($"Result: {result}");
            memory = ConsoleInput.ReadYesNo("Keep the result for the next operation?") ? result : null;
        }
    }

    public static bool TryCalculate(double left, double right, Operation operation, out double result, out string? error)
    {
        error = null;
        result = 0;
        switch (operation)
        {
            case Operation.Add:
                result = left + right;
                return true;
            case Operation.Subtract:
                result = left - right;
                return true;
            case Operation.Multiply:
                result = left * right;
                return true;
            case Operation.Divide when right == 0:
                error = "Cannot divide by zero.";
                return false;
            case Operation.Divide:
                result = left / right;
                return true;
            case Operation.Power when left == 0 && right < 0:
                error = "0 cannot be raised to a negative power.";
                return false;
            case Operation.Power:
                result = SquareAndPower.Power(left, (int)right);
                return true;
            default:
                throw new ArgumentOutOfRangeException(nameof(operation), operation, "Unknown operation.");
        }
    }
}
