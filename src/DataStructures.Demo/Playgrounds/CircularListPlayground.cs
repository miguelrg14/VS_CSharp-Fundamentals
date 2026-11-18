using ConsoleUtils;

namespace DataStructures.Demo.Playgrounds;

public sealed class CircularListPlayground : IPlayground
{
    private const string EndWord = "END";

    public string Title => "Circular list of names";

    public string Description => $"Stores names in a CircularLinkedList<T> (type {EndWord} to finish) and passes the turn around the circle.";

    public void Run()
    {
        var names = new CircularLinkedList<string>();
        while (true)
        {
            string name = ConsoleInput.ReadString($"Name ({EndWord} to finish):");
            if (name.Equals(EndWord, StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

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
