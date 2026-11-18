using System.Text;
using ConsoleUtils;

namespace Fundamentals.Exercises.ControlFlow;

public sealed class HollowRectangle : IExercise
{
    public string Title => "Hollow rectangle";

    public string Description => "Nested loops that draw the border of a rectangle with X characters.";

    public void Run()
    {
        int width = ConsoleInput.ReadInt("Width:", 1, 80);
        int height = ConsoleInput.ReadInt("Height:", 1, 40);
        Console.Write(Draw(width, height));
    }

    public static string Draw(int width, int height)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(width, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(height, 1);

        var builder = new StringBuilder();
        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                bool isBorder = row == 0 || row == height - 1 || column == 0 || column == width - 1;
                builder.Append(isBorder ? 'X' : ' ');
            }

            builder.Append('\n');
        }

        return builder.ToString();
    }
}
