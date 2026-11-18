using System.Collections;

namespace DataStructures;

/// <summary>
/// Doubly linked list with head and tail pointers. Adding or removing at either end is O(1),
/// and the list can be walked in both directions.
/// </summary>
public sealed class DoublyLinkedList<T> : IReadOnlyCollection<T>
{
    private sealed class Node(T value)
    {
        public T Value { get; } = value;
        public Node? Previous { get; set; }
        public Node? Next { get; set; }
    }

    private Node? _head;
    private Node? _tail;

    public int Count { get; private set; }

    public bool IsEmpty => _head is null;

    /// <exception cref="InvalidOperationException">The list is empty.</exception>
    public T First => _head is not null ? _head.Value : throw new InvalidOperationException("The list is empty.");

    /// <exception cref="InvalidOperationException">The list is empty.</exception>
    public T Last => _tail is not null ? _tail.Value : throw new InvalidOperationException("The list is empty.");

    public void AddFirst(T item)
    {
        var node = new Node(item) { Next = _head };
        if (_head is null)
        {
            _tail = node;
        }
        else
        {
            _head.Previous = node;
        }

        _head = node;
        Count++;
    }

    public void AddLast(T item)
    {
        var node = new Node(item) { Previous = _tail };
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

    /// <exception cref="InvalidOperationException">The list is empty.</exception>
    public T RemoveFirst()
    {
        Node node = _head ?? throw new InvalidOperationException("The list is empty.");
        Unlink(node);
        return node.Value;
    }

    /// <exception cref="InvalidOperationException">The list is empty.</exception>
    public T RemoveLast()
    {
        Node node = _tail ?? throw new InvalidOperationException("The list is empty.");
        Unlink(node);
        return node.Value;
    }

    /// <summary>Removes the first occurrence of <paramref name="item"/>. Returns whether it was found.</summary>
    public bool Remove(T item)
    {
        EqualityComparer<T> comparer = EqualityComparer<T>.Default;
        for (Node? node = _head; node is not null; node = node.Next)
        {
            if (comparer.Equals(node.Value, item))
            {
                Unlink(node);
                return true;
            }
        }

        return false;
    }

    public void Clear()
    {
        _head = null;
        _tail = null;
        Count = 0;
    }

    /// <summary>Walks the list from the last item to the first.</summary>
    public IEnumerable<T> Backwards()
    {
        for (Node? node = _tail; node is not null; node = node.Previous)
        {
            yield return node.Value;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (Node? node = _head; node is not null; node = node.Next)
        {
            yield return node.Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private void Unlink(Node node)
    {
        if (node.Previous is null)
        {
            _head = node.Next;
        }
        else
        {
            node.Previous.Next = node.Next;
        }

        if (node.Next is null)
        {
            _tail = node.Previous;
        }
        else
        {
            node.Next.Previous = node.Previous;
        }

        Count--;
    }
}
