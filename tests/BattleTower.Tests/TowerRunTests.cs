using BattleTower;

namespace BattleTower.Tests;

public class CharacterTests
{
    private static Player Hero(int strength = 6, int magicPower = 8, int energy = 100) =>
        new("Hero", health: 50, strength, defense: 2, magicPower, magicBarrier: 5, energy);

    private static Enemy Dummy(int health = 20, int defense = 2, int magicBarrier = 5) =>
        new("Dummy", health, strength: 4, defense, magicPower: 7, magicBarrier, energy: 20);

    [Fact]
    public void Attack_DealsPowerMinusProtection_AndCostsEnergy()
    {
        Player hero = Hero(strength: 6);
        Enemy enemy = Dummy(defense: 2);

        AttackResult result = hero.Attack(enemy, AttackKind.Physical);

        Assert.True(result.Performed);
        Assert.Equal(4, result.Damage);
        Assert.Equal(16, enemy.Health);
        Assert.Equal(94, hero.Energy);
    }

    [Fact]
    public void Attack_AlwaysDealsAtLeastOneDamage()
    {
        Enemy armored = Dummy(defense: 50);

        Assert.Equal(1, Hero().Attack(armored, AttackKind.Physical).Damage);
    }

    [Fact]
    public void Attack_WithoutEnoughEnergy_DoesNothing()
    {
        Player tired = Hero(magicPower: 8, energy: 7);
        Enemy enemy = Dummy();

        AttackResult result = tired.Attack(enemy, AttackKind.Magic);

        Assert.False(result.Performed);
        Assert.Equal(20, enemy.Health);
        Assert.Equal(7, tired.Energy);
    }

    [Fact]
    public void Health_NeverGoesBelowZero()
    {
        Enemy enemy = Dummy(health: 3);

        enemy.TakeDamage(10);

        Assert.Equal(0, enemy.Health);
        Assert.False(enemy.IsAlive);
    }

    [Fact]
    public void Dice_UpperBoundIsInclusive()
    {
        var dice = new Dice(new Random(1));
        int[] rolls = [.. Enumerable.Range(0, 500).Select(_ => dice.Roll(1, 3))];

        Assert.All(rolls, r => Assert.InRange(r, 1, 3));
        Assert.Contains(3, rolls);
    }

    [Fact]
    public void EnemyNames_CanUseEveryWord()
    {
        var dice = new Dice(new Random(7));
        string[] names = [.. Enumerable.Range(0, 2000).Select(_ => Enemy.Create(dice).Name)];

        Assert.Contains(names, n => n.EndsWith("Beggar", StringComparison.Ordinal)); // Last item of the list.
        Assert.Contains(names, n => n.StartsWith("Ruthless", StringComparison.Ordinal));
    }
}

public class TowerRunTests
{
    private static TowerRun OneWeakEnemy(Dice dice) => new(
        new Player("Hero", 100, strength: 6, defense: 3, magicPower: 9, magicBarrier: 6, energy: 200),
        [new Enemy("Weakling", health: 3, strength: 4, defense: 1, magicPower: 7, magicBarrier: 4, energy: 20)],
        dice);

    [Fact]
    public void DefeatingTheLastEnemy_WinsAndAddsTheBonus()
    {
        TowerRun run = OneWeakEnemy(new Dice(new Random(0)));

        run.PlayTurn(PlayerAction.Attack); // 6 - 1 = 5 damage against 3 health.

        Assert.Equal(RunState.Won, run.State);
        Assert.Equal(1, run.DefeatedEnemies);
        Assert.Equal(5 + TowerRun.PointsPerEnemy + TowerRun.PointsForVictory, run.Score);
        Assert.Throws<InvalidOperationException>(() => run.PlayTurn(PlayerAction.Attack));
    }

    [Fact]
    public void Enemies_AreFoughtInQueueOrder()
    {
        var dice = new Dice(new Random(0));
        var run = new TowerRun(
            new Player("Hero", 100, 10, 3, 9, 6, 500),
            [
                new Enemy("First", 1, 4, 1, 7, 4, 20),
                new Enemy("Second", 1, 4, 1, 7, 4, 20),
            ],
            dice);

        Assert.Equal("First", run.CurrentEnemy.Name);
        run.PlayTurn(PlayerAction.Attack);

        Assert.Equal("Second", run.CurrentEnemy.Name);
        Assert.Equal(RunState.InProgress, run.State);
        Assert.Empty(run.WaitingEnemies);
    }

    [Fact]
    public void AttackWithoutEnergy_DoesNotGiveTheEnemyAFreeTurn()
    {
        var run = new TowerRun(
            new Player("Hero", 100, strength: 6, defense: 3, magicPower: 9, magicBarrier: 6, energy: 0),
            [new Enemy("Enemy", 20, 4, 1, 7, 4, 20)],
            new Dice(new Random(0)));

        IReadOnlyList<string> log = run.PlayTurn(PlayerAction.Attack);

        Assert.Single(log);
        Assert.Equal(100, run.Player.Health);
    }

    [Fact]
    public void LosingAllHealth_EndsTheRun()
    {
        var run = new TowerRun(
            new Player("Hero", 1, strength: 1, defense: 0, magicPower: 1, magicBarrier: 0, energy: 0),
            [new Enemy("Brute", 50, strength: 20, defense: 10, magicPower: 20, magicBarrier: 10, energy: 1000)],
            new Dice(new Random(3)));

        for (int turn = 0; turn < 50 && run.State == RunState.InProgress; turn++)
        {
            run.PlayTurn(PlayerAction.Rest);
        }

        Assert.Equal(RunState.Lost, run.State);
        Assert.False(run.Player.IsAlive);
    }

    [Fact]
    public void StartNew_CreatesTheRequestedNumberOfEnemies()
    {
        TowerRun run = TowerRun.StartNew("Hero", 4, new Dice(new Random(5)));

        Assert.Equal(3, run.WaitingEnemies.Count);
        Assert.InRange(run.Player.Health, 60, 80);
    }
}

public class PersistenceTests : IDisposable
{
    private readonly string _directory = Directory.CreateTempSubdirectory("battletower-tests").FullName;

    public void Dispose() => Directory.Delete(_directory, recursive: true);

    [Fact]
    public void SaveAndLoad_RestoresTheWholeRun()
    {
        TowerRun run = TowerRun.StartNew("Hero", 3, new Dice(new Random(11)));
        run.PlayTurn(PlayerAction.Attack);
        string path = Path.Combine(_directory, "save.json");

        SaveGame.Save(run, path);
        Assert.True(SaveGame.TryLoad(path, new Dice(new Random(0)), out TowerRun? loaded, out _));

        Assert.NotNull(loaded);
        Assert.Equal(run.Score, loaded.Score);
        Assert.Equal(run.Player.ToString(), loaded.Player.ToString());
        Assert.Equal(run.CurrentEnemy.ToString(), loaded.CurrentEnemy.ToString());
        Assert.Equal(run.WaitingEnemies.Select(e => e.ToString()), loaded.WaitingEnemies.Select(e => e.ToString()));
    }

    [Fact]
    public void TryLoad_MissingOrCorruptFile_ReportsAnError()
    {
        string missing = Path.Combine(_directory, "missing.json");
        string corrupt = Path.Combine(_directory, "corrupt.json");
        File.WriteAllText(corrupt, "{ not json");

        Assert.False(SaveGame.TryLoad(missing, new Dice(new Random()), out _, out string? missingError));
        Assert.False(SaveGame.TryLoad(corrupt, new Dice(new Random()), out _, out string? corruptError));
        Assert.NotNull(missingError);
        Assert.NotNull(corruptError);
    }

    [Fact]
    public void HighScores_AreSortedBestFirst_AndSurviveASaveAndLoad()
    {
        string path = Path.Combine(_directory, "scores.json");
        var table = new HighScoreTable();
        table.Add(new ScoreEntry("Low", 100, 1, new DateTime(2026, 1, 1)));
        table.Add(new ScoreEntry("High", 900, 5, new DateTime(2026, 1, 2)));
        table.Add(new ScoreEntry("Mid", 500, 3, new DateTime(2026, 1, 3)));

        table.Save(path);
        HighScoreTable loaded = HighScoreTable.Load(path);

        Assert.Equal(new[] { "High", "Mid", "Low" }, loaded.Entries.Select(e => e.PlayerName));
        Assert.Equal(new[] { "High", "Mid" }, loaded.Top(2).Select(e => e.PlayerName));
    }

    [Fact]
    public void HighScores_MissingFile_GivesAnEmptyTable()
    {
        Assert.Empty(HighScoreTable.Load(Path.Combine(_directory, "nothing.json")).Entries);
    }
}
