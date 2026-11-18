using ConsoleUtils;
using DataStructures;

namespace Fundamentals.Exercises.DataStructureDemos;

public sealed class DoublyLinkedListDemo : IExercise
{
    private static readonly string[] Options =
    [
        "Add at the front", "Add at the back", "Remove the first", "Remove the last", "Remove a value", "Show backwards",
    ];

    public string Title => "Doubly linked list";

    public string Description => "DoublyLinkedList<T>: every node knows its neighbours, so both ends are O(1) and it can be walked backwards.";

    public void Run()
    {
        var list = new DoublyLinkedList<int>();
        while (ConsoleMenu.Show($"List: {(list.IsEmpty ? "(empty)" : string.Join(" <-> ", list))}", Options) is int option)
        {
            switch (option)
            {
                case 0:
                    list.AddFirst(ConsoleInput.ReadInt("Value:"));
                    break;
                case 1:
                    list.AddLast(ConsoleInput.ReadInt("Value:"));
                    break;
                case 2 or 3 when list.IsEmpty:
                    Console.WriteLine("The list is empty.");
                    break;
                case 2:
                    Console.WriteLine($"Removed {list.RemoveFirst()}.");
                    break;
                case 3:
                    Console.WriteLine($"Removed {list.RemoveLast()}.");
                    break;
                case 4:
                    int value = ConsoleInput.ReadInt("Value to remove:");
                    Console.WriteLine(list.Remove(value) ? $"Removed {value}." : $"{value} is not in the list.");
                    break;
                case 5:
                    Console.WriteLine($"Backwards: {string.Join(" <-> ", list.Backwards())}");
                    break;
            }
        }
    }
}
