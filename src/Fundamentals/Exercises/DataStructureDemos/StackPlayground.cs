using System.Globalization;
using ConsoleUtils;
using DataStructures;

namespace Fundamentals.Exercises.DataStructureDemos;

public sealed class StackPlayground : IExercise
{
    public const string FileName = "stack.txt";

    private static readonly string[] Options =
    [
        "Push", "Pop", "Peek", "Maximum and its position", "Swap top with maximum",
        "Contains?", "Reverse", "Save to file", "Load from file", "Clear",
    ];

    public string Title => "Stack playground";

    public string Description => "Every operation of LinkedStack<T>, a stack built on a singly linked list.";

    public void Run()
    {
        var stack = new LinkedStack<int>();
        while (ConsoleMenu.Show($"Stack (top -> bottom): {Format(stack)}", Options) is int option)
        {
            switch (option)
            {
                case 0:
                    stack.Push(ConsoleInput.ReadInt("Value to push:"));
                    break;
                case 1:
                    Console.WriteLine(stack.TryPop(out int popped) ? $"Popped {popped}." : "The stack is empty.");
                    break;
                case 2:
                    Console.WriteLine(stack.TryPeek(out int top) ? $"Top: {top}." : "The stack is empty.");
                    break;
                case 3:
                    Console.WriteLine(FindMax(stack) is (int max, int position)
                        ? $"Maximum: {max}, at position {position} from the top."
                        : "The stack is empty.");
                    break;
                case 4:
                    Console.WriteLine(stack.SwapTopWithMax() ? "Top swapped with the maximum." : "The stack is empty.");
                    break;
                case 5:
                    int value = ConsoleInput.ReadInt("Value to look for:");
                    Console.WriteLine(stack.Contains(value) ? $"{value} is in the stack." : $"{value} is not in the stack.");
                    break;
                case 6:
                    stack.Reverse();
                    break;
                case 7:
                    File.WriteAllLines(FileName, stack.Select(n => n.ToString(CultureInfo.InvariantCulture)));
                    Console.WriteLine($"Saved {stack.Count} value(s) to {Path.GetFullPath(FileName)}.");
                    break;
                case 8:
                    Load(stack);
                    break;
                case 9:
                    stack.Clear();
                    break;
            }
        }
    }

    /// <summary>The largest value and its 1-based position counted from the top, or null if the stack is empty.</summary>
    public static (int Max, int Position)? FindMax(LinkedStack<int> stack)
    {
        (int Max, int Position)? best = null;
        int position = 1;
        foreach (int value in stack)
        {
            if (best is null || value > best.Value.Max)
            {
                best = (value, position);
            }

            position++;
        }

        return best;
    }

    private static void Load(LinkedStack<int> stack)
    {
        if (!File.Exists(FileName))
        {
            Console.WriteLine($"{FileName} does not exist. Save the stack first.");
            return;
        }

        // The file lists the values from top to bottom, so they are pushed in reverse order.
        int[] values = [.. File.ReadLines(FileName).Select(line => int.Parse(line, CultureInfo.InvariantCulture))];
        stack.Clear();
        for (int i = values.Length - 1; i >= 0; i--)
        {
            stack.Push(values[i]);
        }

        Console.WriteLine($"Loaded {stack.Count} value(s).");
    }

    private static string Format(LinkedStack<int> stack) => stack.IsEmpty ? "(empty)" : string.Join(" -> ", stack);
}
