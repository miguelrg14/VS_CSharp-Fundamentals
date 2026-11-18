using DataStructures;

namespace DataStructures.Tests;

public class DoublyLinkedListTests
{
    [Fact]
    public void AddFirstAndAddLast_BuildTheListFromBothEnds()
    {
        var list = new DoublyLinkedList<int>();
        list.AddLast(2);
        list.AddFirst(1);
        list.AddLast(3);

        Assert.Equal(new[] { 1, 2, 3 }, list);
        Assert.Equal(new[] { 3, 2, 1 }, list.Backwards());
        Assert.Equal(1, list.First);
        Assert.Equal(3, list.Last);
    }

    [Fact]
    public void Remove_FromTheMiddle_KeepsBothDirectionsLinked()
    {
        var list = new DoublyLinkedList<int>();
        foreach (int value in new[] { 1, 2, 3 })
        {
            list.AddLast(value);
        }

        Assert.True(list.Remove(2));

        Assert.Equal(new[] { 1, 3 }, list);
        Assert.Equal(new[] { 3, 1 }, list.Backwards());
    }

    [Fact]
    public void RemoveFirstAndLast_UntilEmpty()
    {
        var list = new DoublyLinkedList<string>();
        list.AddLast("a");
        list.AddLast("b");

        Assert.Equal("a", list.RemoveFirst());
        Assert.Equal("b", list.RemoveLast());
        Assert.True(list.IsEmpty);
        Assert.Throws<InvalidOperationException>(() => list.RemoveFirst());
        Assert.Empty(list.Backwards());
    }
}

public class CircularLinkedListTests
{
    [Fact]
    public void Enumeration_GoesAroundExactlyOnce()
    {
        var list = new CircularLinkedList<string>();
        list.Add("Ana");
        list.Add("Ben");
        list.Add("Cris");

        Assert.Equal(new[] { "Ana", "Ben", "Cris" }, list);
        Assert.Equal("Ana", list.Head);
    }

    [Fact]
    public void Rotate_MovesTheHeadForwardAndWrapsAround()
    {
        var list = new CircularLinkedList<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);

        list.Rotate();
        Assert.Equal(new[] { 2, 3, 1 }, list);

        list.Rotate();
        list.Rotate();
        Assert.Equal(new[] { 1, 2, 3 }, list);
    }

    [Fact]
    public void SingleItem_PointsToItself()
    {
        var list = new CircularLinkedList<int>();
        list.Add(42);
        list.Rotate();

        Assert.Equal(42, list.Head);
        Assert.Single(list);
    }

    [Fact]
    public void EmptyList_EnumeratesNothing()
    {
        Assert.Empty(new CircularLinkedList<int>());
    }
}

public class PriorityLevelQueueTests
{
    [Fact]
    public void Dequeue_ServesHigherPrioritiesFirst_AndArrivalOrderWithinALevel()
    {
        var queue = new PriorityLevelQueue<string>();
        queue.Enqueue("low", Priority.Low);
        queue.Enqueue("high 1", Priority.High);
        queue.Enqueue("medium", Priority.Medium);
        queue.Enqueue("high 2", Priority.High);

        string[] served = [.. Enumerable.Range(0, 4).Select(_ => queue.Dequeue())];

        Assert.Equal(new[] { "high 1", "high 2", "medium", "low" }, served);
        Assert.True(queue.IsEmpty);
    }

    [Fact]
    public void CountOf_CountsEachLevelSeparately()
    {
        var queue = new PriorityLevelQueue<int>();
        queue.Enqueue(1, Priority.Low);
        queue.Enqueue(2, Priority.Low);
        queue.Enqueue(3, Priority.High);

        Assert.Equal(2, queue.CountOf(Priority.Low));
        Assert.Equal(3, queue.Count);
        Assert.Empty(queue.ItemsOf(Priority.Medium));
    }

    [Fact]
    public void TryDequeue_ReportsThePriority()
    {
        var queue = new PriorityLevelQueue<int>();
        queue.Enqueue(7, Priority.Medium);

        Assert.True(queue.TryDequeue(out int item, out Priority priority));
        Assert.Equal(7, item);
        Assert.Equal(Priority.Medium, priority);
        Assert.False(queue.TryDequeue(out _, out _));
    }
}

public class SortingTests
{
    [Theory]
    [InlineData(new[] { 5, 3, 8, 1, 2 }, new[] { 1, 2, 3, 5, 8 })]
    [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
    [InlineData(new[] { 3, 3, 1 }, new[] { 1, 3, 3 })]
    [InlineData(new int[0], new int[0])]
    public void BubbleSort_SortsAscending(int[] input, int[] expected)
    {
        int[] items = [.. input];

        Sorting.BubbleSort(items);

        Assert.Equal(expected, items);
    }

    [Fact]
    public void BubbleSort_UsesTheComparer()
    {
        var items = new List<string> { "ccc", "a", "bb" };

        Sorting.BubbleSort(items, Comparer<string>.Create((a, b) => b.Length.CompareTo(a.Length)));

        Assert.Equal(new[] { "ccc", "bb", "a" }, items);
    }
}
