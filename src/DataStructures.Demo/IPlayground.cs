namespace DataStructures.Demo;

/// <summary>An interactive menu to try every operation of one data structure.</summary>
public interface IPlayground
{
    string Title { get; }

    string Description { get; }

    void Run();
}
