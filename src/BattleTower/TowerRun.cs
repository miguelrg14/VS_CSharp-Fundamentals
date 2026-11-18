using DataStructures;

namespace BattleTower;

public enum PlayerAction
{
    Attack,
    MagicMissile,
    Rest,
}

public enum RunState
{
    InProgress,
    Won,
    Lost,
}

/// <summary>
/// One climb up the tower: the player fights the enemies one by one, in the order of a queue.
/// This class holds all the rules and never touches the console; every turn returns a log
/// of what happened, which the UI prints.
/// </summary>
public sealed class TowerRun
{
    public const int PointsPerEnemy = 50;
    public const int PointsForVictory = 500;

    private readonly Dice _dice;
    private readonly LinkedQueue<Enemy> _waiting;

    public TowerRun(Player player, IEnumerable<Enemy> enemies, Dice dice, int score = 0, int defeatedEnemies = 0)
    {
        _dice = dice;
        _waiting = new LinkedQueue<Enemy>(enemies);
        Player = player;
        Score = score;
        DefeatedEnemies = defeatedEnemies;
        CurrentEnemy = _waiting.TryDequeue(out Enemy? first)
            ? first
            : throw new ArgumentException("A run needs at least one enemy.", nameof(enemies));
    }

    public Player Player { get; }

    public Enemy CurrentEnemy { get; private set; }

    /// <summary>Enemies still waiting after the current one, in order.</summary>
    public IReadOnlyCollection<Enemy> WaitingEnemies => _waiting;

    public int Score { get; private set; }

    public int DefeatedEnemies { get; private set; }

    public RunState State { get; private set; } = RunState.InProgress;

    public static TowerRun StartNew(string playerName, int enemyCount, Dice dice)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(enemyCount, 1);
        Enemy[] enemies = [.. Enumerable.Range(0, enemyCount).Select(_ => Enemy.Create(dice))];
        return new TowerRun(Player.Create(playerName, enemyCount, dice), enemies, dice);
    }

    /// <summary>Plays the player's action, the enemy's answer and the end-of-turn energy recovery.</summary>
    public IReadOnlyList<string> PlayTurn(PlayerAction action)
    {
        if (State != RunState.InProgress)
        {
            throw new InvalidOperationException("The run is already over.");
        }

        var log = new List<string>();
        switch (action)
        {
            case PlayerAction.Attack or PlayerAction.MagicMissile:
                AttackKind kind = action == PlayerAction.Attack ? AttackKind.Physical : AttackKind.Magic;
                if (!PlayerAttacks(kind, log))
                {
                    return log; // Not enough energy: the turn is not used up.
                }

                break;
            case PlayerAction.Rest:
                int recovered = _dice.Roll(2, 4);
                Player.RecoverEnergy(recovered);
                log.Add($"You rest and recover {recovered} energy.");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, "Unknown action.");
        }

        if (!CurrentEnemy.IsAlive)
        {
            EnemyDefeated(log);
            return log;
        }

        EnemyTurn(log);
        if (!Player.IsAlive)
        {
            State = RunState.Lost;
            log.Add("You have been defeated...");
            return log;
        }

        int regained = _dice.Roll(1, 2);
        Player.RecoverEnergy(regained);
        CurrentEnemy.RecoverEnergy(_dice.Roll(1, 2));
        log.Add($"You catch your breath (+{regained} energy).");
        return log;
    }

    internal SaveData ToSaveData() =>
        new(Player.ToData(), CurrentEnemy.ToData(), [.. _waiting.Select(e => e.ToData())], Score, DefeatedEnemies);

    internal static TowerRun FromSaveData(SaveData data, Dice dice) =>
        new(
            Player.FromData(data.Player),
            [Enemy.FromData(data.CurrentEnemy), .. data.WaitingEnemies.Select(Enemy.FromData)],
            dice,
            data.Score,
            data.DefeatedEnemies);

    private bool PlayerAttacks(AttackKind kind, List<string> log)
    {
        string actionName = kind == AttackKind.Physical ? "attack" : "magic missile";
        AttackResult result = Player.Attack(CurrentEnemy, kind);
        if (!result.Performed)
        {
            log.Add($"You are too tired for a {actionName} (it costs {Player.CostOf(kind)} energy, you have {Player.Energy}). Try resting.");
            return false;
        }

        Score += result.Damage;
        log.Add($"Your {actionName} deals {result.Damage} damage to {CurrentEnemy.Name} ({CurrentEnemy.Health} health left).");
        return true;
    }

    /// <summary>The enemy picks at random among the actions it can afford, and rests when it cannot afford any.</summary>
    private void EnemyTurn(List<string> log)
    {
        List<AttackKind> affordable = [.. Enum.GetValues<AttackKind>().Where(CurrentEnemy.CanAfford)];
        bool rests = affordable.Count == 0 || _dice.Roll(1, affordable.Count + 1) > affordable.Count;
        if (rests)
        {
            int recovered = _dice.Roll(2, 4);
            CurrentEnemy.RecoverEnergy(recovered);
            log.Add($"{CurrentEnemy.Name} rests and recovers {recovered} energy.");
            return;
        }

        AttackKind kind = _dice.Pick(affordable);
        AttackResult result = CurrentEnemy.Attack(Player, kind);
        string actionName = kind == AttackKind.Physical ? "attacks you" : "casts a magic missile at you";
        log.Add($"{CurrentEnemy.Name} {actionName}: {result.Damage} damage ({Player.Health} health left).");
    }

    private void EnemyDefeated(List<string> log)
    {
        Score += PointsPerEnemy;
        DefeatedEnemies++;
        log.Add($"{CurrentEnemy.Name} has been defeated! (+{PointsPerEnemy} points)");

        if (_waiting.TryDequeue(out Enemy? next))
        {
            CurrentEnemy = next;
            log.Add($"Next floor: {next.Name} blocks the way.");
            return;
        }

        Score += PointsForVictory;
        State = RunState.Won;
        log.Add($"You reached the top of the tower! (+{PointsForVictory} points)");
    }
}
