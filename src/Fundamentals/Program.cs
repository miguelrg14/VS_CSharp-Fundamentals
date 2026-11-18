using ConsoleUtils;

namespace Fundamentals;

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("C# Fundamentals - console exercises");

        try
        {
            RunMenu();
        }
        catch (EndOfStreamException)
        {
            // Input was redirected from a file and it ran out: just finish.
        }
    }

    private static void RunMenu()
    {
        IReadOnlyList<ExerciseCategory> categories = ExerciseCatalog.Categories;
        while (ConsoleMenu.Show("Categories", [.. categories.Select(c => c.Name)], "Exit") is int categoryIndex)
        {
            ExerciseCategory category = categories[categoryIndex];
            while (ConsoleMenu.Show(category.Name, [.. category.Exercises.Select(e => e.Title)]) is int exerciseIndex)
            {
                IExercise exercise = category.Exercises[exerciseIndex];
                ConsoleMenu.Header(exercise.Title);
                Console.WriteLine(exercise.Description);
                Console.WriteLine();
                exercise.Run();
                ConsoleInput.Pause();
            }
        }
    }
}
