using ConsoleUtils;

namespace Fundamentals.Exercises.ControlFlow;

public sealed class GuessTheNumber : IExercise
{
    private const int Min = 1;
    private const int Max = 100;

    public string Title => "Guess the number";

    public string Description => $"The computer picks a number between {Min} and {Max}. Guess it in as few attempts as you can.";

    public void Run()
    {
        // Random.Next's upper bound is exclusive, hence Max + 1.
        int secret = Random.Shared.Next(Min, Max + 1);
        int attempts = 0;
        int guess;
        do
        {
            guess = ConsoleInput.ReadInt("Your guess:", Min, Max);
            attempts++;
            Console.WriteLine(Hint(guess, secret));
        }
        while (guess != secret);

        Console.WriteLine($"You needed {attempts} attempt{(attempts == 1 ? "" : "s")}.");
    }

    public static string Hint(int guess, int secret) => guess.CompareTo(secret) switch
    {
        < 0 => $"{guess} is too low.",
        > 0 => $"{guess} is too high.",
        _ => "Correct, well done!",
    };
}
