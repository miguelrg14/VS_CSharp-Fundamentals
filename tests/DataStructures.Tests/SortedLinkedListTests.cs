using DataStructures;

namespace DataStructures.Tests;

public class SortedLinkedListTests
{
    [Fact]
    public void Add_KeepsItemsSorted()
    {
        var list = new SortedLinkedList<int>();
        foreach (int value in new[] { 5, 1, 4, 1, 9, 0 })
        {
            list.Add(value);
        }

        Assert.Equal(new[] { 0, 1, 1, 4, 5, 9 }, list);
        Assert.Equal(6, list.Count);
    }

    [Fact]
    public void Add_WithCustomComparer_SortsDescending()
    {
        var list = new SortedLinkedList<int>(Comparer<int>.Create((a, b) => b.CompareTo(a)));
        list.Add(2);
        list.Add(7);
        list.Add(4);

        Assert.Equal(new[] { 7, 4, 2 }, list);
    }

    [Fact]
    public void Add_IsStableForEqualItems()
    {
        var byLength = Comparer<string>.Create((a, b) => a.Length.CompareTo(b.Length));
        var list = new SortedLinkedList<string>(byLength);
        list.Add("bb");
        list.Add("a");
        list.Add("cc");

        Assert.Equal(new[] { "a", "bb", "cc" }, list);
    }

    [Fact]
    public void Remove_RemovesOnlyTheFirstOccurrence()
    {
        var list = new SortedLinkedList<int>();
        list.Add(3);
        list.Add(3);
        list.Add(1);

        bool removed = list.Remove(3);

        Assert.True(removed);
        Assert.Equal(new[] { 1, 3 }, list);
    }

    [Fact]
    public void RemoveAll_RemovesEveryOccurrence_IncludingTheHead()
    {
        var list = new SortedLinkedList<int>();
        foreach (int value in new[] { 2, 2, 2, 5 })
        {
            list.Add(value);
        }

        int removed = list.RemoveAll(2);

        Assert.Equal(3, removed);
        Assert.Equal(new[] { 5 }, list);
        Assert.Single(list);
    }

    [Fact]
    public void Remove_MissingValue_ReturnsFalse()
    {
        var list = new SortedLinkedList<int>();
        list.Add(1);

        Assert.False(list.Remove(7));
        Assert.Single(list);
    }

    [Theory]
    [InlineData(4, true)]
    [InlineData(5, false)]
    [InlineData(-1, false)]
    public void Contains_FindsExistingValues(int value, bool expected)
    {
        var list = new SortedLinkedList<int>();
        list.Add(2);
        list.Add(4);
        list.Add(8);

        bool found = list.Contains(value);

        Assert.Equal(expected, found);
    }
}
