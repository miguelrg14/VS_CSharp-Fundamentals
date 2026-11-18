using ConsoleUtils;

namespace Fundamentals.Tests;

/// <summary>Console.In/Out are global, so these tests redirect them and restore them afterwards.</summary>
public sealed class ConsoleInputTests : IDisposable
{
    private readonly TextReader _originalIn = Console.In;
    private readonly TextWriter _originalOut = Console.Out;
    private readonly StringWriter _output = new();

    public ConsoleInputTests() => Console.SetOut(_output);

    public void Dispose()
    {
        Console.SetIn(_originalIn);
        Console.SetOut(_originalOut);
        _output.Dispose();
    }

    [Fact]
    public void ReadInt_KeepsAskingUntilTheValueIsValid()
    {
        Console.SetIn(new StringReader("abc\n42\n7\n"));

        int value = ConsoleInput.ReadInt("Number:", 1, 10);

        Assert.Equal(7, value);
        Assert.Contains("between 1 and 10", _output.ToString());
    }

    [Theory]
    [InlineData("7.5")]
    [InlineData("7,5")]
    public void ReadDouble_AcceptsDotAndComma(string input)
    {
        Console.SetIn(new StringReader(input + "\n"));

        Assert.Equal(7.5, ConsoleInput.ReadDouble("Number:"), 10);
    }

    [Fact]
    public void ReadYesNo_UnderstandsSeveralAnswers()
    {
        Console.SetIn(new StringReader("maybe\nYES\nn\n"));

        Assert.True(ConsoleInput.ReadYesNo("Continue?"));
        Assert.False(ConsoleInput.ReadYesNo("Continue?"));
    }

    [Fact]
    public void ReadString_ThrowsWhenTheInputEnds()
    {
        Console.SetIn(new StringReader(string.Empty));

        Assert.Throws<EndOfStreamException>(() => ConsoleInput.ReadString("Name:"));
    }

    [Fact]
    public void Menu_ReturnsNullForTheBackOption()
    {
        Console.SetIn(new StringReader("2\n0\n"));

        Assert.Equal(1, ConsoleMenu.Show("Test", ["a", "b"]));
        Assert.False(ConsoleMenu.Show("Test", ["a", "b"]).HasValue);
    }
}
