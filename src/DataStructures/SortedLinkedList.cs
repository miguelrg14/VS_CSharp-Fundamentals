using System.Collections;

namespace DataStructures;

/// <summary>
/// Singly linked list that keeps its items sorted on insertion.
/// Equal items keep their insertion order (the insertion is stable).
/// </summary>
public sealed class SortedLinkedList<T> : IReadOnlyCollection<T>
{
    private sealed class Node(T value, Node? next)
    {
        public T Value { get; } = value;
        public Node? Next { get; set; } = next;
    }

    private readonly IComparer<T> _comparer;
    private Node? _head;

    /// <param name="comparer">Defines the order. Defaults to ascending <see cref="Comparer{T}.Default"/>.</param>
    public SortedLinkedList(IComparer<T>? comparer = null)
    {
        _comparer = comparer ?? Comparer<T>.Default;
    }

    public int Count { get; private set; }

    public bool IsEmpty => _head is null;

    /// <summary>Inserts <paramref name="item"/> after every item that is less than or equal to it. O(n).</summary>
    public void Add(T item)
    {
        if (_head is null || _comparer.Compare(item, _head.Value) < 0)
        {
            _head = new Node(item, _head);
        }
        else
        {
            Node previous = _head;
            while (previous.Next is not null && _comparer.Compare(previous.Next.Value, item) <= 0)
            {
                previous = previous.Next;
            }

            previous.Next = new Node(item, previous.Next);
        }

        Count++;
    }

    /// <summary>Removes the first item equal to <paramref name="item"/>. Returns whether one was found.</summary>
    public bool Remove(T item) => RemoveWhere(item, removeAll: false) > 0;

    /// <summary>Removes every item equal to <paramref name="item"/> and returns how many were removed.</summary>
    public int RemoveAll(T item) => RemoveWhere(item, removeAll: true);

    public bool Contains(T item)
    {
        for (Node? node = _head; node is not null; node = node.Next)
        {
            int comparison = _comparer.Compare(node.Value, item);
            if (comparison == 0)
            {
                return true;
            }

            if (comparison > 0)
            {
                return false; // Sorted: everything after this is larger.
            }
        }

        return false;
    }

    public void Clear()
    {
        _head = null;
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

    private int RemoveWhere(T item, bool removeAll)
    {
        int removed = 0;

        // A sentinel node avoids special-casing the head.
        var sentinel = new Node(default!, _head);
        Node previous = sentinel;
        while (previous.Next is not null)
        {
            int comparison = _comparer.Compare(previous.Next.Value, item);
            if (comparison > 0)
            {
                break;
            }

            if (comparison == 0)
            {
                previous.Next = previous.Next.Next;
                removed++;
                if (!removeAll)
                {
                    break;
                }
            }
            else
            {
                previous = previous.Next;
            }
        }

        _head = sentinel.Next;
        Count -= removed;
        return removed;
    }
}
