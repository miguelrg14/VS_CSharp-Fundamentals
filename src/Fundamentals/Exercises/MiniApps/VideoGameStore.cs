namespace Fundamentals.Exercises.MiniApps;

/// <summary>
/// A video game in a shop's inventory: encapsulated fields, validation, a copy constructor,
/// value equality and an overloaded operator.
/// </summary>
public sealed class VideoGame : IEquatable<VideoGame>
{
    private decimal _price;

    public VideoGame(string title, string publisher, string genre, int pegiRating, decimal cost, decimal price, int stock)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentOutOfRangeException.ThrowIfNegative(cost);
        ArgumentOutOfRangeException.ThrowIfNegative(stock);
        if (pegiRating is not (3 or 7 or 12 or 16 or 18))
        {
            throw new ArgumentOutOfRangeException(nameof(pegiRating), pegiRating, "PEGI ratings are 3, 7, 12, 16 or 18.");
        }

        Title = title;
        Publisher = publisher;
        Genre = genre;
        PegiRating = pegiRating;
        Cost = cost;
        Price = price;
        Stock = stock;
    }

    /// <summary>Copy constructor: a new, independent object with the same values.</summary>
    public VideoGame(VideoGame other)
        : this(other.Title, other.Publisher, other.Genre, other.PegiRating, other.Cost, other.Price, other.Stock)
    {
    }

    public string Title { get; }

    public string Publisher { get; }

    public string Genre { get; }

    public int PegiRating { get; }

    /// <summary>What the shop paid for each copy.</summary>
    public decimal Cost { get; }

    /// <summary>Retail price. decimal is the right type for money: double cannot store 0.1 exactly.</summary>
    public decimal Price
    {
        get => _price;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            _price = value;
        }
    }

    public int Stock { get; private set; }

    public decimal ProfitPerCopy => Price - Cost;

    /// <exception cref="InvalidOperationException">There are not enough copies in stock.</exception>
    public void Sell(int units)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(units);
        if (units > Stock)
        {
            throw new InvalidOperationException($"Only {Stock} copies of {Title} are in stock.");
        }

        Stock -= units;
    }

    public void Restock(int units)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(units);
        Stock += units;
    }

    /// <summary>
    /// Creates a bundle of two games with a 40% discount. The operands are not modified:
    /// an operator that changed its inputs would be very surprising.
    /// </summary>
    public static VideoGame operator +(VideoGame first, VideoGame second) => new(
        $"Bundle: {first.Title} + {second.Title}",
        first.Publisher == second.Publisher ? first.Publisher : $"{first.Publisher} / {second.Publisher}",
        first.Genre == second.Genre ? first.Genre : $"{first.Genre} / {second.Genre}",
        Math.Max(first.PegiRating, second.PegiRating),
        first.Cost + second.Cost,
        Math.Round((first.Price + second.Price) * 0.6m, 2),
        stock: 1);

    /// <summary>Two objects describe the same product when title and publisher match (stock and price may differ).</summary>
    public bool Equals(VideoGame? other) =>
        other is not null
        && string.Equals(Title, other.Title, StringComparison.OrdinalIgnoreCase)
        && string.Equals(Publisher, other.Publisher, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => Equals(obj as VideoGame);

    public override int GetHashCode() =>
        HashCode.Combine(Title.ToUpperInvariant(), Publisher.ToUpperInvariant());

    public override string ToString() =>
        $"{Title} ({Publisher}, {Genre}, PEGI {PegiRating}) - {Price:0.00} EUR, {Stock} in stock";
}

/// <summary>
/// Inventory-wide statistics belong to the inventory, not to static fields of VideoGame
/// (static counters are shared by every object and are easy to get out of sync).
/// </summary>
public sealed class GameCatalog
{
    private readonly List<VideoGame> _games = [];

    public IReadOnlyList<VideoGame> Games => _games;

    public void Add(VideoGame game) => _games.Add(game);

    public decimal AveragePrice => _games.Count == 0 ? 0 : _games.Average(g => g.Price);

    public decimal StockValue => _games.Sum(g => g.Price * g.Stock);
}

public sealed class VideoGameStore : IExercise
{
    public string Title => "Video game store (classes)";

    public string Description => "Walks through constructors, references vs copies, equality and operator overloading.";

    public void Run()
    {
        var zelda = new VideoGame("Zelda", "Nintendo", "RPG", 7, 19.99m, 29.99m, 500);
        Console.WriteLine($"Created:          {zelda}");

        VideoGame sameReference = zelda;           // Both variables point to the same object.
        var copy = new VideoGame(zelda);            // A new object with the same values.

        zelda.Price = 24.99m;
        Console.WriteLine($"After price cut:  {zelda}");
        Console.WriteLine($"Same reference:   {sameReference}   (changed too)");
        Console.WriteLine($"Copy:             {copy}   (unchanged)");
        Console.WriteLine($"zelda.Equals(copy): {zelda.Equals(copy)}, ReferenceEquals: {ReferenceEquals(zelda, copy)}");

        zelda.Sell(200);
        Console.WriteLine($"After selling 200: {zelda.Stock} in stock");
        zelda.Restock(333);
        Console.WriteLine($"After restocking 333: {zelda.Stock} in stock");

        try
        {
            zelda.Sell(10_000);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Selling 10000 fails: {ex.Message}");
        }

        var mario = new VideoGame("Mario Kart", "Nintendo", "Racing", 3, 30m, 49.99m, 120);
        VideoGame bundle = zelda + mario;
        Console.WriteLine($"Bundle:           {bundle}");

        var catalog = new GameCatalog();
        catalog.Add(zelda);
        catalog.Add(mario);
        catalog.Add(bundle);
        Console.WriteLine($"Catalog: {catalog.Games.Count} games, average price {catalog.AveragePrice:0.00} EUR, stock value {catalog.StockValue:0.00} EUR");
    }
}
