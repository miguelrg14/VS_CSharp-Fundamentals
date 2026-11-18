using ConsoleUtils;
using DataStructures;

namespace Fundamentals.Exercises.DataStructureDemos;

public sealed class CircularNamesList : IExercise
{
    private const string EndWord = "END";

    public string Title => "Circular list of names";

    public string Description => $"Stores names in a CircularLinkedList<T> (type {EndWord} to finish) and passes the turn around the circle.";

    public void Run()
    {
        var names = new CircularLinkedList<string>();
        string name;
        while (!(name = ConsoleInput.ReadString($"Name ({EndWord} to finish):")).Equals(EndWord, StringComparison.OrdinalIgnoreCase))
        {
            names.Add(name);
        }

        Console.WriteLine($"{names.Count} name(s) in the circle.");
        if (names.IsEmpty)
        {
            return;
        }

        Console.WriteLine($"Circle: {string.Join(" -> ", names)} -> (back to {names.Head})");

        int turns = ConsoleInput.ReadInt("How many turns should be played?", 0, 100);
        for (int turn = 1; turn <= turns; turn++)
        {
            Console.WriteLine($"Turn {turn}: {names.Head}");
            names.Rotate();
        }
    }
}
