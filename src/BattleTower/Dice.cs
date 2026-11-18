namespace BattleTower;

/// <summary>
/// Thin wrapper around <see cref="Random"/> with an inclusive upper bound, which is how game rules are
/// usually written ("between 1 and 3"). Random.Next(1, 3) only returns 1 or 2, an easy off-by-one to miss.
/// Passing a seeded Random makes a whole run reproducible, which the tests rely on.
/// </summary>
public sealed class Dice(Random random)
{
    public int Roll(int min, int maxInclusive) => random.Next(min, maxInclusive + 1);

    public T Pick<T>(IReadOnlyList<T> options) => options[random.Next(options.Count)];
}
