using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DataStructures;

/// <summary>
/// FIFO queue backed by a singly linked list with head and tail pointers,
/// so <see cref="Enqueue"/> and <see cref="Dequeue"/> are O(1).
/// Enumeration goes from the front to the back.
/// </summary>
public sealed class LinkedQueue<T> : IReadOnlyCollection<T>
{
    private sealed class Node(T value)
    {
        public T Value { get; } = value;
        public Node? Next { get; set; }
    }

    private Node? _head;
    private Node? _tail;

    public LinkedQueue()
    {
    }

    public LinkedQueue(IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            Enqueue(item);
        }
    }

    public int Count { get; private set; }

    public bool IsEmpty => _head is null;

    public void Enqueue(T item)
    {
        var node = new Node(item);
        if (_tail is null)
        {
            _head = node;
        }
        else
        {
            _tail.Next = node;
        }

        _tail = node;
        Count++;
    }

    /// <exception cref="InvalidOperationException">The queue is empty.</exception>
    public T Dequeue() => TryDequeue(out T? item) ? item : throw new InvalidOperationException("The queue is empty.");

    /// <exception cref="InvalidOperationException">The queue is empty.</exception>
    public T Peek() => _head is not null ? _head.Value : throw new InvalidOperationException("The queue is empty.");

    public bool TryDequeue([MaybeNullWhen(false)] out T item)
    {
        if (_head is null)
        {
            item = default;
            return false;
        }

        item = _head.Value;
        _head = _head.Next;
        if (_head is null)
        {
            _tail = null;
        }

        Count--;
        return true;
    }

    public void Clear()
    {
        _head = null;
        _tail = null;
        Count = 0;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (Node? node = _head; node is not null; node = node.Next)
        {
            yield return node.Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
