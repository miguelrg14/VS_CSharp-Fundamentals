using ConsoleUtils;
using DataStructures;

namespace Fundamentals.Exercises.DataStructureDemos;

public sealed class SortedListDemo : IExercise
{
    private static readonly string[] Options = ["Add a value", "Remove one occurrence", "Remove every occurrence", "Add 10 random values", "Clear"];

    public string Title => "Sorted linked list";

    public string Description => "SortedLinkedList<T> inserts every value in its place, so the list is always in order.";

    public void Run()
    {
        var list = new SortedLinkedList<int>();
        while (ConsoleMenu.Show($"List: {(list.IsEmpty ? "(empty)" : string.Join(", ", list))}", Options) is int option)
        {
            switch (option)
            {
                case 0:
                    list.Add(ConsoleInput.ReadInt("Value:"));
                    break;
                case 1:
                    int value = ConsoleInput.ReadInt("Value to remove:");
                    Console.WriteLine(list.Remove(value) ? $"Removed one {value}." : $"{value} is not in the list.");
                    break;
                case 2:
                    int all = ConsoleInput.ReadInt("Value to remove:");
                    Console.WriteLine($"Removed {list.RemoveAll(all)} occurrence(s) of {all}.");
                    break;
                case 3:
                    for (int i = 0; i < 10; i++)
                    {
                        list.Add(Random.Shared.Next(0, 100));
                    }

                    break;
                case 4:
                    list.Clear();
                    break;
            }
        }
    }
}
