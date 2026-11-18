using ConsoleUtils;

namespace Fundamentals.Exercises.ControlFlow;

public sealed class PrizeDoors : IExercise
{
    private const int Picks = 3;

    private static readonly string[] Prizes =
    [
        "a brand new car",
        "the latest blockbuster video game",
        "a next-gen game console",
        "a gaming PC",
        "a trip to Miami",
    ];

    public string Title => "Prize doors";

    public string Description => $"Pick {Picks} doors out of {Prizes.Length} and see what is behind them.";

    public void Run()
    {
        for (int attempt = 1; attempt <= Picks; attempt++)
        {
            int door = ConsoleInput.ReadInt($"Attempt #{attempt} - choose a door (1-{Prizes.Length}):", 1, Prizes.Length);
            Console.WriteLine($"Behind door {door} there is {Prizes[door - 1]}!");
        }
    }
}
