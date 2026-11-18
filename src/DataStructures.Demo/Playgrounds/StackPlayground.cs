using System.Globalization;
using ConsoleUtils;

namespace DataStructures.Demo.Playgrounds;

public sealed class StackPlayground : IPlayground
{
    public const string FileName = "stack.txt";

    private static readonly string[] Options =
    [
        "Push", "Pop", "Peek", "Maximum and its position", "Swap top with maximum",
        "Contains?", "Reverse", "Save to file", "Load from file", "Clear",
    ];

    public string Title => "Stack";

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
                    Console.WriteLine(stack.TryGetMax(out int max, out int position)
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

    private static void Load(LinkedStack<int> stack)
    {
        if (!File.Exists(FileName))
        {
            Console.WriteLine($"{FileName} does not exist. Save the stack first.");
            return;
        }

        var values = new List<int>();
        foreach (string line in File.ReadLines(FileName))
        {
            if (!int.TryParse(line, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
            {
                Console.WriteLine($"{FileName} is corrupt (\"{line}\" is not a number). The stack was not changed.");
                return;
            }

            values.Add(value);
        }

        // The file lists the values from top to bottom, so they are pushed in reverse order.
        stack.Clear();
        for (int i = values.Count - 1; i >= 0; i--)
        {
            stack.Push(values[i]);
        }

        Console.WriteLine($"Loaded {stack.Count} value(s).");
    }

    private static string Format(LinkedStack<int> stack) => stack.IsEmpty ? "(empty)" : string.Join(" -> ", stack);
}
