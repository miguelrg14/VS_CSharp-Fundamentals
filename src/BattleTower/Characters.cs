namespace BattleTower;

public enum AttackKind
{
    Physical,
    Magic,
}

/// <summary>Outcome of one attack, so the UI can describe it without knowing the rules.</summary>
public readonly record struct AttackResult(bool Performed, int Damage, int EnergySpent);

/// <summary>
/// Everything a player and an enemy have in common: the six stats and the combat rules,
/// written once for both.
/// </summary>
public abstract class Character
{
    protected Character(string name, int health, int strength, int defense, int magicPower, int magicBarrier, int energy)
    {
        Name = name;
        Health = health;
        Strength = strength;
        Defense = defense;
        MagicPower = magicPower;
        MagicBarrier = magicBarrier;
        Energy = energy;
    }

    public string Name { get; }

    public int Health { get; private set; }

    public int Strength { get; }

    public int Defense { get; }

    public int MagicPower { get; }

    public int MagicBarrier { get; }

    public int Energy { get; private set; }

    public bool IsAlive => Health > 0;

    /// <summary>An attack costs as much energy as the power it uses.</summary>
    public int CostOf(AttackKind kind) => kind == AttackKind.Physical ? Strength : MagicPower;

    public bool CanAfford(AttackKind kind) => Energy >= CostOf(kind);

    /// <summary>
    /// Damage is the attacker's power minus the target's matching protection, with a minimum of 1,
    /// so a strong defense reduces damage instead of cancelling it completely.
    /// </summary>
    public AttackResult Attack(Character target, AttackKind kind)
    {
        if (!CanAfford(kind))
        {
            return new AttackResult(Performed: false, Damage: 0, EnergySpent: 0);
        }

        int power = CostOf(kind);
        int protection = kind == AttackKind.Physical ? target.Defense : target.MagicBarrier;
        int damage = Math.Max(1, power - protection);

        Energy -= power;
        target.TakeDamage(damage);
        return new AttackResult(Performed: true, damage, power);
    }

    public void RecoverEnergy(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        Energy += amount;
    }

    public void TakeDamage(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        Health = Math.Max(0, Health - amount);
    }

    public override string ToString() =>
        $"{Name}: health {Health} | strength {Strength} | defense {Defense} | magic power {MagicPower} | " +
        $"magic barrier {MagicBarrier} | energy {Energy}";

    internal CharacterData ToData() => new(Name, Health, Strength, Defense, MagicPower, MagicBarrier, Energy);
}

public sealed class Player : Character
{
    public Player(string name, int health, int strength, int defense, int magicPower, int magicBarrier, int energy)
        : base(name, health, strength, defense, magicPower, magicBarrier, energy)
    {
    }

    /// <summary>Health and energy scale with the number of enemies, so long runs stay winnable.</summary>
    public static Player Create(string name, int enemyCount, Dice dice) => new(
        name,
        health: dice.Roll(15 * enemyCount, 20 * enemyCount),
        strength: dice.Roll(4, 6),
        defense: dice.Roll(1, 3),
        magicPower: dice.Roll(7, 9),
        magicBarrier: dice.Roll(4, 6),
        energy: dice.Roll(15 * enemyCount, 20 * enemyCount));

    internal static Player FromData(CharacterData d) =>
        new(d.Name, d.Health, d.Strength, d.Defense, d.MagicPower, d.MagicBarrier, d.Energy);
}

public sealed class Enemy : Character
{
    private static readonly string[] Classes =
        ["Warlock", "Warrior", "Archer", "Knight", "Gladiator", "Prince", "Troll", "Giant", "Dwarf", "Beggar"];

    private static readonly string[] Adjectives =
        ["Mad", "Hairy", "Flea-ridden", "Mighty", "Swift", "Nimble", "Filthy", "Limping", "One-eyed", "Ruthless"];

    public Enemy(string name, int health, int strength, int defense, int magicPower, int magicBarrier, int energy)
        : base(name, health, strength, defense, magicPower, magicBarrier, energy)
    {
    }

    public static Enemy Create(Dice dice) => new(
        $"{dice.Pick(Adjectives)} {dice.Pick(Classes)}",
        health: dice.Roll(15, 20),
        strength: dice.Roll(4, 6),
        defense: dice.Roll(1, 3),
        magicPower: dice.Roll(7, 9),
        magicBarrier: dice.Roll(4, 6),
        energy: dice.Roll(15, 20));

    internal static Enemy FromData(CharacterData d) =>
        new(d.Name, d.Health, d.Strength, d.Defense, d.MagicPower, d.MagicBarrier, d.Energy);
}
