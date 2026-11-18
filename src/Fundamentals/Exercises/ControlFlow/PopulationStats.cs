using ConsoleUtils;

namespace Fundamentals.Exercises.ControlFlow;

public sealed class PopulationStats : IExercise
{
    public enum Group
    {
        Boy,
        Girl,
        Man,
        Woman,
    }

    public string Title => "Population statistics";

    public string Description => "Type one letter per person: B (boy), G (girl), M (man), W (woman). Any other input ends the count.";

    public void Run()
    {
        var people = new List<Group>();
        while (TryParseGroup(ConsoleInput.ReadString("Person (B/G/M/W):", allowEmpty: true), out Group group))
        {
            people.Add(group);
        }

        if (people.Count == 0)
        {
            Console.WriteLine("Nobody was counted.");
            return;
        }

        Console.WriteLine($"Total: {people.Count}");
        foreach ((Group group, int count, double percentage) in Summarize(people))
        {
            Console.WriteLine($"{group,-6} {count,4}  ({percentage:0.#}%)");
        }
    }

    public static bool TryParseGroup(string text, out Group group)
    {
        Group? parsed = text.Trim().ToUpperInvariant() switch
        {
            "B" => Group.Boy,
            "G" => Group.Girl,
            "M" => Group.Man,
            "W" => Group.Woman,
            _ => null,
        };

        group = parsed.GetValueOrDefault();
        return parsed.HasValue;
    }

    /// <summary>Count and percentage of every group (0% when nobody belongs to it).</summary>
    public static IReadOnlyList<(Group Group, int Count, double Percentage)> Summarize(IReadOnlyCollection<Group> people)
    {
        if (people.Count == 0)
        {
            throw new ArgumentException("There must be at least one person.", nameof(people));
        }

        return
        [
            .. Enum.GetValues<Group>().Select(group =>
            {
                int count = people.Count(p => p == group);
                return (group, count, count * 100.0 / people.Count);
            }),
        ];
    }
}
