using DataStructures;

namespace DataStructures.Tests;

public class LinkedStackTests
{
    [Fact]
    public void Pop_ReturnsItemsInReverseOrderOfPush()
    {
        var stack = new LinkedStack<int>([1, 2, 3]);

        Assert.Equal(3, stack.Pop());
        Assert.Equal(2, stack.Pop());
        Assert.Equal(1, stack.Pop());
        Assert.True(stack.IsEmpty);
    }

    [Fact]
    public void Enumeration_GoesFromTopToBottom()
    {
        var stack = new LinkedStack<int>([1, 2, 3]);

        Assert.Equal(new[] { 3, 2, 1 }, stack);
    }

    [Fact]
    public void Peek_DoesNotRemoveTheItem()
    {
        var stack = new LinkedStack<string>(["a", "b"]);

        Assert.Equal("b", stack.Peek());
        Assert.Equal(2, stack.Count);
    }

    [Fact]
    public void Pop_OnEmptyStack_Throws()
    {
        var stack = new LinkedStack<int>();

        Assert.Throws<InvalidOperationException>(() => stack.Pop());
        Assert.False(stack.TryPop(out _));
        Assert.False(stack.TryPeek(out _));
    }

    [Fact]
    public void Reverse_RelinksTheNodesInPlace()
    {
        var stack = new LinkedStack<int>([1, 2, 3, 4]);

        stack.Reverse();

        Assert.Equal(new[] { 1, 2, 3, 4 }, stack);
        Assert.Equal(1, stack.Peek());
        Assert.Equal(4, stack.Count);
    }

    [Fact]
    public void Reverse_OnEmptyStack_DoesNothing()
    {
        var stack = new LinkedStack<int>();

        stack.Reverse();

        Assert.Empty(stack);
    }

    [Fact]
    public void SwapTopWithMax_MovesTheLargestValueToTheTop()
    {
        var stack = new LinkedStack<int>([5, 9, 2, 7]); // top -> bottom: 7, 2, 9, 5

        bool swapped = stack.SwapTopWithMax();

        Assert.True(swapped);
        Assert.Equal(new[] { 9, 2, 7, 5 }, stack);
    }

    [Fact]
    public void TryGetMax_ReportsTheValueAndItsPositionFromTheTop()
    {
        var stack = new LinkedStack<int>([4, 8, 1, 8]); // top -> bottom: 8, 1, 8, 4

        Assert.True(stack.TryGetMax(out int max, out int position));
        Assert.Equal(8, max);
        Assert.Equal(1, position); // The first maximum from the top wins.

        stack.Pop();
        Assert.True(stack.TryGetMax(out max, out position));
        Assert.Equal((8, 2), (max, position));
    }

    [Fact]
    public void TryGetMax_OnEmptyStack_ReturnsFalse()
    {
        Assert.False(new LinkedStack<int>().TryGetMax(out _, out int position));
        Assert.Equal(0, position);
    }

    [Fact]
    public void SwapTopWithMax_OnEmptyStack_ReturnsFalse()
    {
        Assert.False(new LinkedStack<int>().SwapTopWithMax());
    }

    [Fact]
    public void Clear_EmptiesTheStack()
    {
        var stack = new LinkedStack<int>([1, 2]);

        stack.Clear();

        Assert.True(stack.IsEmpty);
        Assert.Empty(stack);
    }
}
