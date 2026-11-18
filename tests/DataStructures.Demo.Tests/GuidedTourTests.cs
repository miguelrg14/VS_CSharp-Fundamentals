using DataStructures.Demo;

namespace DataStructures.Demo.Tests;

public sealed class GuidedTourTests : IDisposable
{
    private readonly TextWriter _originalOut = Console.Out;
    private readonly StringWriter _output = new();

    public GuidedTourTests() => Console.SetOut(_output);

    public void Dispose()
    {
        Console.SetOut(_originalOut);
        _output.Dispose();
    }

    [Fact]
    public void Tour_RunsEverySectionAndPrintsTheExpectedResults()
    {
        GuidedTour.Run(pauseBetweenSections: false);
        string output = _output.ToString();

        Assert.Contains("Pop()            -> 7", output);
        Assert.Contains("Dequeue()        -> Ana (the first to arrive)", output);
        Assert.Contains("Add(20)          -> [10, 10, 20, 30, 40]", output);
        Assert.Contains("Backwards()      -> [3, 2, 1]", output);
        Assert.Contains("Turn 4: Ana", output);
        Assert.Contains("High   Server is down", output);
        Assert.Contains("After:  [11, 12, 22, 25, 64]", output);
    }
}
