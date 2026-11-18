using System.Collections;

namespace DataStructures;

/// <summary>
/// Circular singly linked list: the last node points back to the first one.
/// Only a pointer to the tail is stored, because the head is always <c>tail.Next</c>;
/// that makes appending O(1) without a second pointer.
/// </summary>
public sealed class CircularLinkedList<T> : IReadOnlyCollection<T>
{
    private sealed class Node(T value)
    {
        public T Value { get; } = value;
        public Node Next { get; set; } = null!; // Always set right after construction.
    }

    private Node? _tail;

    public int Count { get; private set; }

    public bool IsEmpty => _tail is null;

    /// <exception cref="InvalidOperationException">The list is empty.</exception>
    public T Head => _tail is not null ? _tail.Next.Value : throw new InvalidOperationException("The list is empty.");

    /// <summary>Appends <paramref name="item"/> at the end of the circle (just before the head).</summary>
    public void Add(T item)
    {
        var node = new Node(item);
        if (_tail is null)
        {
            node.Next = node; // A single node points to itself.
        }
        else
        {
            node.Next = _tail.Next;
            _tail.Next = node;
        }

        _tail = node;
        Count++;
    }

    /// <summary>Moves the head one position forward: the old head becomes the last item.</summary>
    public void Rotate()
    {
        if (_tail is not null)
        {
            _tail = _tail.Next;
        }
    }

    public void Clear()
    {
        _tail = null;
        Count = 0;
    }

    /// <summary>Goes around the circle exactly once, starting at the head.</summary>
    public IEnumerator<T> GetEnumerator()
    {
        if (_tail is null)
        {
            yield break;
        }

        Node node = _tail.Next;
        do
        {
            yield return node.Value;
            node = node.Next;
        }
        while (node != _tail.Next);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
