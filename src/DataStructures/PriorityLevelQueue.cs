using System.Diagnostics.CodeAnalysis;

namespace DataStructures;

public enum Priority
{
    High,
    Medium,
    Low,
}

/// <summary>
/// Queue with three priority levels, implemented as one <see cref="LinkedQueue{T}"/> per level.
/// <see cref="Dequeue"/> always serves the highest non-empty level first, and items with the same
/// priority are served in arrival order. Every operation is O(1).
/// </summary>
public sealed class PriorityLevelQueue<T>
{
    private readonly LinkedQueue<T>[] _levels =
        [.. Enum.GetValues<Priority>().Select(_ => new LinkedQueue<T>())];

    public int Count => _levels.Sum(level => level.Count);

    public bool IsEmpty => _levels.All(level => level.IsEmpty);

    public void Enqueue(T item, Priority priority) => Level(priority).Enqueue(item);

    /// <exception cref="InvalidOperationException">Every level is empty.</exception>
    public T Dequeue() =>
        TryDequeue(out T? item, out _) ? item : throw new InvalidOperationException("The queue is empty.");

    public bool TryDequeue([MaybeNullWhen(false)] out T item, out Priority priority)
    {
        foreach (Priority level in Enum.GetValues<Priority>())
        {
            if (Level(level).TryDequeue(out item))
            {
                priority = level;
                return true;
            }
        }

        item = default;
        priority = default;
        return false;
    }

    /// <summary>Dequeues from one specific level.</summary>
    public bool TryDequeue(Priority priority, [MaybeNullWhen(false)] out T item) => Level(priority).TryDequeue(out item);

    public int CountOf(Priority priority) => Level(priority).Count;

    /// <summary>The items waiting at one level, in the order they will be served.</summary>
    public IReadOnlyCollection<T> ItemsOf(Priority priority) => Level(priority);

    private LinkedQueue<T> Level(Priority priority) =>
        Enum.IsDefined(priority)
            ? _levels[(int)priority]
            : throw new ArgumentOutOfRangeException(nameof(priority), priority, "Unknown priority.");
}
