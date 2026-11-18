using ConsoleUtils;

namespace Fundamentals.Exercises.Methods;

public sealed class SquareAndPower : IExercise
{
    public string Title => "Square and power";

    public string Description => "Small methods with parameters and return values, including negative exponents.";

    public void Run()
    {
        int number = ConsoleInput.ReadInt("Number to square:", -46_340, 46_340);
        Console.WriteLine($"{number}^2 = {Square(number)}");

        int @base = ConsoleInput.ReadInt("Base:");
        int exponent = ConsoleInput.ReadInt("Exponent:", -64, 64);
        if (@base == 0 && exponent < 0)
        {
            Console.WriteLine("0 cannot be raised to a negative power (it would divide by zero).");
            return;
        }

        Console.WriteLine($"{@base}^{exponent} = {Power(@base, exponent)}");
    }

    public static int Square(int number) => number * number;

    /// <summary>
    /// Repeated multiplication. A negative exponent gives the inverse: 2^-3 = 1 / 2^3.
    /// The product must start at 1: starting at 0 would make every result 0.
    /// </summary>
    public static double Power(double @base, int exponent)
    {
        if (@base == 0 && exponent < 0)
        {
            throw new DivideByZeroException("0 cannot be raised to a negative power.");
        }

        double result = 1;
        for (int i = 0; i < Math.Abs(exponent); i++)
        {
            result *= @base;
        }

        return exponent < 0 ? 1 / result : result;
    }
}
