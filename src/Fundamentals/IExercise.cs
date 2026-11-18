namespace Fundamentals;

/// <summary>One self-contained console exercise that can be launched from the main menu.</summary>
public interface IExercise
{
    string Title { get; }

    /// <summary>One line shown before the exercise starts.</summary>
    string Description { get; }

    void Run();
}

public sealed record ExerciseCategory(string Name, IReadOnlyList<IExercise> Exercises);
