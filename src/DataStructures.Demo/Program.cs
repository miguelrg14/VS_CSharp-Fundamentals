using ConsoleUtils;
using DataStructures.Demo.Playgrounds;

namespace DataStructures.Demo;

/// <summary>
/// Runnable front end for the DataStructures library (a class library cannot be run on its own):
/// a guided tour of every structure, plus one interactive playground per structure.
/// </summary>
public static class Program
{
    private static readonly IPlayground[] Playgrounds =
    [
        new StackPlayground(),
        new PriorityQueuePlayground(),
        new SortedListPlayground(),
        new DoublyLinkedListPlayground(),
        new CircularListPlayground(),
    ];

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        try
        {
            string[] options = ["Guided tour of every structure", .. Playgrounds.Select(p => $"Try it yourself: {p.Title}")];
            while (ConsoleMenu.Show("Data structures", options, "Exit") is int choice)
            {
                if (choice == 0)
                {
                    GuidedTour.Run();
                    continue;
                }

                IPlayground playground = Playgrounds[choice - 1];
                ConsoleMenu.Header(playground.Title);
                Console.WriteLine(playground.Description);
                playground.Run();
            }
        }
        catch (EndOfStreamException)
        {
            // Redirected input ran out.
        }
    }
}
