using ConsoleUtils;

namespace DataStructures.Demo;

/// <summary>
/// A scripted walkthrough of every structure in the library: each step runs an operation
/// and prints the result, so you can see how the structure behaves without typing anything.
/// </summary>
public static class GuidedTour
{
    public static void Run(bool pauseBetweenSections = true)
    {
        Section("LinkedStack<T> - last in, first out", StackDemo, pauseBetweenSections);
        Section("LinkedQueue<T> - first in, first out", QueueDemo, pauseBetweenSections);
        Section("SortedLinkedList<T> - always in order", SortedListDemo, pauseBetweenSections);
        Section("DoublyLinkedList<T> - walk it both ways", DoublyLinkedListDemo, pauseBetweenSections);
        Section("CircularLinkedList<T> - the last node points to the first", CircularListDemo, pauseBetweenSections);
        Section("PriorityLevelQueue<T> - urgent work first", PriorityQueueDemo, pauseBetweenSections);
        Section("Sorting.BubbleSort - in-place sorting", BubbleSortDemo, pauseBetweenSections: false);
    }

    private static void StackDemo()
    {
        var stack = new LinkedStack<int>();
        foreach (int value in new[] { 5, 9, 2, 7 })
        {
            stack.Push(value);
            Step($"Push({value})", stack);
        }

        Console.WriteLine($"Peek()           -> {stack.Peek()} (it stays on top)");
        Console.WriteLine($"Pop()            -> {stack.Pop()}");
        Step("after Pop", stack);

        stack.TryGetMax(out int max, out int position);
        Console.WriteLine($"TryGetMax()      -> {max}, at position {position} from the top");

        stack.SwapTopWithMax();
        Step("SwapTopWithMax()", stack);

        stack.Reverse();
        Step("Reverse()", stack);

        Console.WriteLine($"TryPop on empty  -> {new LinkedStack<int>().TryPop(out _)} (no exception)");
    }

    private static void QueueDemo()
    {
        var queue = new LinkedQueue<string>();
        foreach (string customer in new[] { "Ana", "Ben", "Cris" })
        {
            queue.Enqueue(customer);
            Step($"Enqueue(\"{customer}\")", queue);
        }

        Console.WriteLine($"Dequeue()        -> {queue.Dequeue()} (the first to arrive)");
        Step("after Dequeue", queue);
        queue.Enqueue("Dani");
        Step("Enqueue(\"Dani\")", queue);
    }

    private static void SortedListDemo()
    {
        var list = new SortedLinkedList<int>();
        foreach (int value in new[] { 40, 10, 30, 10, 20 })
        {
            list.Add(value);
            Step($"Add({value})", list);
        }

        Console.WriteLine($"Contains(30)     -> {list.Contains(30)}");
        Console.WriteLine($"Remove(10)       -> {list.Remove(10)} (only the first 10)");
        Step("after Remove", list);
        Console.WriteLine($"RemoveAll(10)    -> {list.RemoveAll(10)} removed");
        Step("after RemoveAll", list);

        var descending = new SortedLinkedList<int>(Comparer<int>.Create((a, b) => b.CompareTo(a)));
        foreach (int value in new[] { 3, 8, 1 })
        {
            descending.Add(value);
        }

        Step("Descending order", descending);
    }

    private static void DoublyLinkedListDemo()
    {
        var list = new DoublyLinkedList<int>();
        list.AddLast(2);
        Step("AddLast(2)", list);
        list.AddFirst(1);
        Step("AddFirst(1)", list);
        list.AddLast(3);
        Step("AddLast(3)", list);

        Console.WriteLine($"Backwards()      -> [{string.Join(", ", list.Backwards())}]");
        Console.WriteLine($"RemoveFirst()    -> {list.RemoveFirst()}");
        Console.WriteLine($"RemoveLast()     -> {list.RemoveLast()}");
        Step("after removals", list);
    }

    private static void CircularListDemo()
    {
        var players = new CircularLinkedList<string>();
        foreach (string name in new[] { "Ana", "Ben", "Cris" })
        {
            players.Add(name);
        }

        Step("Add x3", players);
        for (int turn = 1; turn <= 5; turn++)
        {
            Console.WriteLine($"Turn {turn}: {players.Head}");
            players.Rotate();
        }

        Console.WriteLine("After the last player the turn goes back to the first one.");
    }

    private static void PriorityQueueDemo()
    {
        var tickets = new PriorityLevelQueue<string>();
        tickets.Enqueue("Update the docs", Priority.Low);
        tickets.Enqueue("Server is down", Priority.High);
        tickets.Enqueue("Typo in the menu", Priority.Medium);
        tickets.Enqueue("Payments failing", Priority.High);

        foreach (Priority level in Enum.GetValues<Priority>())
        {
            Console.WriteLine($"{level,-6}: [{string.Join(", ", tickets.ItemsOf(level))}]");
        }

        Console.WriteLine("Serving in order:");
        while (tickets.TryDequeue(out string? ticket, out Priority priority))
        {
            Console.WriteLine($"  {priority,-6} {ticket}");
        }
    }

    private static void BubbleSortDemo()
    {
        int[] numbers = [64, 25, 12, 22, 11];
        Console.WriteLine($"Before: [{string.Join(", ", numbers)}]");
        Sorting.BubbleSort(numbers);
        Console.WriteLine($"After:  [{string.Join(", ", numbers)}]");

        var words = new List<string> { "pear", "fig", "banana", "kiwi" };
        Sorting.BubbleSort(words, Comparer<string>.Create((a, b) => a.Length.CompareTo(b.Length)));
        Console.WriteLine($"Words by length: [{string.Join(", ", words)}] (equal lengths keep their order)");
    }

    private static void Section(string title, Action demo, bool pauseBetweenSections)
    {
        ConsoleMenu.Header(title);
        demo();
        if (pauseBetweenSections)
        {
            ConsoleInput.Pause();
        }
    }

    private static void Step<T>(string operation, IEnumerable<T> structure) =>
        Console.WriteLine($"{operation,-16} -> [{string.Join(", ", structure)}]");
}
