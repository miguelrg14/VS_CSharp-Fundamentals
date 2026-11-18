using ConsoleUtils;
using DataStructures;

namespace Fundamentals.Exercises.DataStructureDemos;

public sealed class PriorityQueueDemo : IExercise
{
    private static readonly string[] Options =
    [
        "Add a task", "Serve the next task", "Show the queues", "Fill with random tasks", "Largest task number",
    ];

    public string Title => "Priority queue";

    public string Description => "PriorityLevelQueue<T>: one FIFO queue per priority; high priority tasks are always served first.";

    public void Run()
    {
        var queue = new PriorityLevelQueue<int>();
        while (ConsoleMenu.Show($"Priority queue ({queue.Count} tasks)", Options) is int option)
        {
            switch (option)
            {
                case 0:
                    queue.Enqueue(ConsoleInput.ReadInt("Task number:"), ReadPriority());
                    break;
                case 1:
                    Console.WriteLine(queue.TryDequeue(out int task, out Priority priority)
                        ? $"Serving task {task} ({priority} priority)."
                        : "There are no tasks waiting.");
                    break;
                case 2:
                    foreach (Priority level in Enum.GetValues<Priority>())
                    {
                        IReadOnlyCollection<int> items = queue.ItemsOf(level);
                        Console.WriteLine($"{level,-6} ({items.Count}): {string.Join(", ", items)}");
                    }

                    break;
                case 3:
                    for (int i = 0; i < 10; i++)
                    {
                        queue.Enqueue(Random.Shared.Next(1, 1000), (Priority)Random.Shared.Next(0, 3));
                    }

                    Console.WriteLine("Added 10 random tasks.");
                    break;
                case 4:
                    Console.WriteLine(queue.IsEmpty
                        ? "There are no tasks waiting."
                        : $"Largest task number: {Enum.GetValues<Priority>().SelectMany(queue.ItemsOf).Max()}");
                    break;
            }
        }
    }

    private static Priority ReadPriority() =>
        (Priority)(ConsoleInput.ReadInt("Priority (1 = high, 2 = medium, 3 = low):", 1, 3) - 1);
}
