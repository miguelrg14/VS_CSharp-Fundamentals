using Fundamentals.Exercises.DataStructureDemos;
using Fundamentals.Exercises.Files;
using Fundamentals.Exercises.Methods;
using Fundamentals.Exercises.MiniApps;
using Fundamentals.Exercises.StringsAndArrays;
using DataStructures;

namespace Fundamentals.Tests;

public class MethodsTests
{
    [Theory]
    [InlineData(2.0, 10, 1024.0)]
    [InlineData(2.0, -2, 0.25)]
    [InlineData(5.0, 0, 1.0)]
    [InlineData(-3.0, 3, -27.0)]
    public void Power_HandlesPositiveZeroAndNegativeExponents(double @base, int exponent, double expected)
    {
        Assert.Equal(expected, SquareAndPower.Power(@base, exponent), 10);
    }

    [Fact]
    public void Power_ZeroToANegativeExponent_Throws()
    {
        Assert.Throws<DivideByZeroException>(() => SquareAndPower.Power(0, -1));
    }

    [Theory]
    [InlineData(0, 1L)]
    [InlineData(1, 1L)]
    [InlineData(5, 120L)]
    [InlineData(20, 2432902008176640000L)]
    public void Factorial_RecursiveAndIterativeAgree(int n, long expected)
    {
        Assert.Equal(expected, RecursionBasics.FactorialRecursive(n));
        Assert.Equal(expected, RecursionBasics.FactorialIterative(n));
    }

    [Fact]
    public void Countdown_EndsAtZero()
    {
        Assert.Equal(new[] { 3, 2, 1, 0 }, RecursionBasics.Countdown(3));
    }

    [Fact]
    public void Fibonacci_SequenceMatchesTheRecursiveDefinition()
    {
        long[] sequence = [.. Fibonacci.Sequence(15)];

        Assert.Equal(new long[] { 0, 1, 1, 2, 3, 5, 8, 13 }, sequence.Take(8));
        for (int n = 0; n < sequence.Length; n++)
        {
            Assert.Equal(Fibonacci.Recursive(n), sequence[n]);
        }
    }

    [Fact]
    public void Fibonacci_LastTermFitsInALong()
    {
        Assert.Equal(7540113804746346429L, Fibonacci.Sequence(Fibonacci.MaxTerm + 1).Last());
    }

    [Fact]
    public void RefAndOut_ModifyTheCallersVariables()
    {
        string sentence = "Hello";
        RefAndOutParameters.AppendOh(ref sentence);
        RefAndOutParameters.SplitNumber(-3.75, out long whole, out double fraction);

        Assert.Equal("Hello, oh", sentence);
        Assert.Equal(-3L, whole);
        Assert.Equal(-0.75, fraction, 10);
    }

    [Fact]
    public void NumberPair_ReturnsWhatWasStored()
    {
        var pair = new NumberPair();
        pair.Store(4, 9);
        (int first, int second) = pair;

        Assert.Equal((4, 9), (first, second));
    }
}

public class StringsAndArraysTests
{
    [Theory]
    [InlineData("abc", 1, "bcd")]
    [InlineData("xyz", 3, "abc")]
    [InlineData("Hello, World!", 1, "Ifmmp, Xpsme!")]
    [InlineData("abc", -1, "zab")]
    public void CaesarShift_ShiftsOnlyLettersAndWrapsAround(string text, int shift, string expected)
    {
        Assert.Equal(expected, StringToolkit.CaesarShift(text, shift));
    }

    [Fact]
    public void CaesarShift_CanBeUndone()
    {
        const string Text = "Video games 4 ever";

        Assert.Equal(Text, StringToolkit.CaesarShift(StringToolkit.CaesarShift(Text, 7), -7));
    }

    [Fact]
    public void Reverse_ReversesTheText()
    {
        Assert.Equal("olleh", StringToolkit.Reverse("hello"));
        Assert.Equal(string.Empty, StringToolkit.Reverse(string.Empty));
    }

    [Fact]
    public void Max_DoesNotModifyTheArray()
    {
        int[] numbers = [3, 9, 1];

        Assert.Equal(9, ArrayBasics.Max(numbers));
        Assert.Equal(new[] { 3, 9, 1 }, numbers);
    }

    [Fact]
    public void SortedDescendingAndReversed_ReturnNewArrays()
    {
        int[] numbers = [3, 9, 1];

        Assert.Equal(new[] { 9, 3, 1 }, ArrayBasics.SortedDescending(numbers));
        Assert.Equal(new[] { 1, 9, 3 }, ArrayBasics.Reversed(numbers));
        Assert.Equal(new[] { 3, 9, 1 }, numbers);
    }

    [Fact]
    public void Average_UsesDecimalDivision()
    {
        Assert.Equal(1.5, RandomArrayStats.Average([1, 2]), 10);
        Assert.Equal(5050L, RandomArrayStats.Sum(RandomArrayStats.Consecutive(1, 100)));
    }

    [Fact]
    public void RandomArray_StaysWithinTheInclusiveRange()
    {
        int[] numbers = RandomArrayStats.RandomArray(1000, 1, 3, new Random(42));

        Assert.All(numbers, n => Assert.InRange(n, 1, 3));
        Assert.Contains(3, numbers); // The upper bound must be reachable.
    }

    [Theory]
    [InlineData(0L, true)]
    [InlineData(7L, true)]
    [InlineData(12321L, true)]
    [InlineData(1221L, true)]
    [InlineData(10L, false)]
    [InlineData(123L, false)]
    public void IsPalindrome_ComparesTheNumberWithItsReverse(long number, bool expected)
    {
        Assert.Equal(expected, PalindromeNumber.IsPalindrome(number));
    }
}

public class FilesAndAppsTests
{
    [Fact]
    public void ParseNumbers_ReadsTheSavedFormat()
    {
        Assert.Equal(new[] { 4, 1, 3 }, BubbleSortWithFile.ParseNumbers("4 / 1 / 3"));
        Assert.Equal("1 / 3 / 4", BubbleSortWithFile.Format([1, 3, 4]));
    }

    [Fact]
    public void Contact_SerializationRoundTrips()
    {
        var contact = new Contact("Ana", "García", 30, "+34 600 000 000");

        Assert.True(AddressBook.TryDeserialize(AddressBook.Serialize(contact), out Contact copy));
        Assert.Equal(contact, copy);
        Assert.False(AddressBook.TryDeserialize("not a contact", out _));
    }

    [Theory]
    [InlineData(6.0, 3.0, Calculator.Operation.Divide, 2.0)]
    [InlineData(2.0, 3.0, Calculator.Operation.Power, 8.0)]
    [InlineData(2.5, 0.5, Calculator.Operation.Subtract, 2.0)]
    public void Calculator_ComputesTheOperation(double left, double right, Calculator.Operation operation, double expected)
    {
        Assert.True(Calculator.TryCalculate(left, right, operation, out double result, out _));
        Assert.Equal(expected, result, 10);
    }

    [Fact]
    public void Calculator_RefusesToDivideByZero()
    {
        Assert.False(Calculator.TryCalculate(1, 0, Calculator.Operation.Divide, out _, out string? error));
        Assert.NotNull(error);
    }

    [Fact]
    public void VideoGame_BundleDoesNotChangeTheOriginalGames()
    {
        var a = new VideoGame("A", "Studio", "RPG", 12, 10m, 20m, 5);
        var b = new VideoGame("B", "Studio", "RPG", 16, 10m, 30m, 5);

        VideoGame bundle = a + b;

        Assert.Equal(30.00m, bundle.Price);
        Assert.Equal(16, bundle.PegiRating);
        Assert.Equal(5, a.Stock);
        Assert.Equal(5, b.Stock);
    }

    [Fact]
    public void VideoGame_CopyIsEqualButNotTheSameObject()
    {
        var game = new VideoGame("Zelda", "Nintendo", "RPG", 7, 20m, 30m, 10);
        var copy = new VideoGame(game);

        Assert.Equal(game, copy);
        Assert.NotSame(game, copy);
    }

    [Fact]
    public void VideoGame_CannotSellMoreThanTheStock()
    {
        var game = new VideoGame("Zelda", "Nintendo", "RPG", 7, 20m, 30m, 10);

        Assert.Throws<InvalidOperationException>(() => game.Sell(11));
        game.Sell(10);
        Assert.Equal(0, game.Stock);
    }

    [Fact]
    public void StackPlayground_FindMax_ReportsThePositionFromTheTop()
    {
        var stack = new LinkedStack<int>([4, 8, 1]); // top -> bottom: 1, 8, 4

        Assert.Equal((8, 2), StackPlayground.FindMax(stack));
        Assert.False(StackPlayground.FindMax(new LinkedStack<int>()).HasValue);
    }
}
