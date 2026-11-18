using DataStructures;

namespace DataStructures.Tests;

public class LinkedQueueTests
{
    [Fact]
    public void Dequeue_ReturnsItemsInArrivalOrder()
    {
        var queue = new LinkedQueue<int>([1, 2, 3]);

        Assert.Equal(1, queue.Dequeue());
        Assert.Equal(2, queue.Dequeue());
        Assert.Equal(3, queue.Dequeue());
        Assert.True(queue.IsEmpty);
    }

    [Fact]
    public void Enqueue_AfterEmptying_StillWorks()
    {
        var queue = new LinkedQueue<string>(["a"]);
        queue.Dequeue();

        queue.Enqueue("b");
        queue.Enqueue("c");

        Assert.Equal(new[] { "b", "c" }, queue);
        Assert.Equal("b", queue.Peek());
    }

    [Fact]
    public void Dequeue_OnEmptyQueue_Throws()
    {
        var queue = new LinkedQueue<int>();

        Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        Assert.Throws<InvalidOperationException>(() => queue.Peek());
        Assert.False(queue.TryDequeue(out _));
    }

    [Fact]
    public void Count_TracksEnqueueAndDequeue()
    {
        var queue = new LinkedQueue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Dequeue();

        Assert.Single(queue);
    }
}
