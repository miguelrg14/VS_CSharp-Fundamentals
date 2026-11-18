using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DataStructures;

/// <summary>
/// LIFO stack backed by a singly linked list. The top of the stack is the head of the list,
/// so <see cref="Push"/>, <see cref="Pop"/> and <see cref="Peek"/> are O(1).
/// Enumeration goes from the top to the bottom.
/// </summary>
public sealed class LinkedStack<T> : IReadOnlyCollection<T>
{
    private sealed class Node(T value, Node? next)
    {
        public T Value { get; set; } = value;
        public Node? Next { get; set; } = next;
    }

    private Node? _top;

    public LinkedStack()
    {
    }

    /// <summary>Creates a stack by pushing <paramref name="items"/> in order (the last item ends on top).</summary>
    public LinkedStack(IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            Push(item);
        }
    }

    public int Count { get; private set; }

    public bool IsEmpty => _top is null;

    public void Push(T item)
    {
        _top = new Node(item, _top);
        Count++;
    }

    /// <exception cref="InvalidOperationException">The stack is empty.</exception>
    public T Pop() => TryPop(out T? item) ? item : throw new InvalidOperationException("The stack is empty.");

    /// <exception cref="InvalidOperationException">The stack is empty.</exception>
    public T Peek() => TryPeek(out T? item) ? item : throw new InvalidOperationException("The stack is empty.");

    public bool TryPop([MaybeNullWhen(false)] out T item)
    {
        if (_top is null)
        {
            item = default;
            return false;
        }

        item = _top.Value;
        _top = _top.Next;
        Count--;
        return true;
    }

    public bool TryPeek([MaybeNullWhen(false)] out T item)
    {
        if (_top is null)
        {
            item = default;
            return false;
        }

        item = _top.Value;
        return true;
    }

    public void Clear()
    {
        _top = null;
        Count = 0;
    }

    /// <summary>Reverses the stack in place in O(n) by relinking the nodes (no copies).</summary>
    public void Reverse()
    {
        Node? previous = null;
        Node? current = _top;
        while (current is not null)
        {
            Node? next = current.Next;
            current.Next = previous;
            previous = current;
            current = next;
        }

        _top = previous;
    }

    /// <summary>
    /// Swaps the value on top of the stack with the largest value in the stack.
    /// Returns <c>false</c> when the stack is empty.
    /// </summary>
    public bool SwapTopWithMax(IComparer<T>? comparer = null)
    {
        if (_top is null)
        {
            return false;
        }

        comparer ??= Comparer<T>.Default;
        Node max = _top;
        for (Node? node = _top.Next; node is not null; node = node.Next)
        {
            if (comparer.Compare(node.Value, max.Value) > 0)
            {
                max = node;
            }
        }

        (_top.Value, max.Value) = (max.Value, _top.Value);
        return true;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (Node? node = _top; node is not null; node = node.Next)
        {
            yield return node.Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
