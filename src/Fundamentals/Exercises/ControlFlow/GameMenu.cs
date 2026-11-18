using ConsoleUtils;

namespace Fundamentals.Exercises.ControlFlow;

public sealed class GameMenu : IExercise
{
    public string Title => "Game main menu";

    public string Description => "A do-while loop with a switch: the menu repeats until the player chooses to quit.";

    public void Run()
    {
        int option;
        do
        {
            Console.WriteLine();
            Console.WriteLine("1. Start game");
            Console.WriteLine("2. Options");
            Console.WriteLine("3. Credits");
            Console.WriteLine("4. Quit");

            // ConsoleInput rejects anything outside 1-4, which covers the "invalid input" case.
            option = ConsoleInput.ReadInt("Choose an option:", 1, 4);
            Console.WriteLine(option switch
            {
                1 => "Starting the game...",
                2 => "Loading options...",
                3 => "Loading credits...",
                _ => "Bye bye!",
            });
        }
        while (option != 4);
    }
}
