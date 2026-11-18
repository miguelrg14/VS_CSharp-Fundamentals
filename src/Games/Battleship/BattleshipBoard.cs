using System.Text;

namespace Games.Battleship;

/// <summary>One player's fleet and the shots the opponent has fired at it.</summary>
public sealed class BattleshipBoard
{
    private sealed class Ship(IReadOnlyList<Coordinate> cells)
    {
        public IReadOnlyList<Coordinate> Cells { get; } = cells;

        public HashSet<Coordinate> Hits { get; } = [];

        public bool IsSunk => Hits.Count == Cells.Count;
    }

    private readonly List<Ship> _ships = [];
    private readonly HashSet<Coordinate> _shots = [];

    public BattleshipBoard(int size)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(size, 2);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(size, 26); // One letter per row.
        Size = size;
    }

    public int Size { get; }

    public int ShipCount => _ships.Count;

    public bool AllShipsSunk => _ships.Count > 0 && _ships.All(s => s.IsSunk);

    /// <summary>
    /// Places a ship starting at <paramref name="start"/> and extending right (horizontal) or down (vertical).
    /// Fails with a reason when it would leave the board or overlap another ship.
    /// </summary>
    public bool TryPlaceShip(Coordinate start, int length, Orientation orientation, out string? error)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(length, 1);

        Coordinate[] cells =
        [
            .. Enumerable.Range(0, length).Select(i => orientation == Orientation.Horizontal
                ? start with { Column = start.Column + i }
                : start with { Row = start.Row + i }),
        ];

        if (!cells.All(IsInside))
        {
            error = "The ship does not fit on the board there.";
            return false;
        }

        if (cells.Any(HasShip))
        {
            error = "The ship would overlap another ship.";
            return false;
        }

        _ships.Add(new Ship(cells));
        error = null;
        return true;
    }

    /// <summary>Places every ship at a random valid position.</summary>
    public void PlaceRandomly(IEnumerable<int> shipLengths, Random random)
    {
        foreach (int length in shipLengths)
        {
            bool placed = false;
            for (int attempt = 0; attempt < 1000 && !placed; attempt++)
            {
                var start = new Coordinate(random.Next(Size), random.Next(Size));
                var orientation = (Orientation)random.Next(2);
                placed = TryPlaceShip(start, length, orientation, out _);
            }

            if (!placed)
            {
                throw new InvalidOperationException($"Could not find room for a ship of length {length}.");
            }
        }
    }

    public ShotResult Fire(Coordinate target)
    {
        if (!IsInside(target))
        {
            throw new ArgumentOutOfRangeException(nameof(target), target, "The shot is outside the board.");
        }

        if (!_shots.Add(target))
        {
            return ShotResult.AlreadyFired;
        }

        Ship? ship = _ships.Find(s => s.Cells.Contains(target));
        if (ship is null)
        {
            return ShotResult.Miss;
        }

        ship.Hits.Add(target);
        return ship.IsSunk ? ShotResult.Sunk : ShotResult.Hit;
    }

    /// <summary>
    /// Draws the board. The owner sees their ships (<paramref name="revealShips"/>); the opponent only sees
    /// hits (X), misses (o) and unknown water (~).
    /// </summary>
    public string Render(bool revealShips)
    {
        var builder = new StringBuilder("   ");
        for (int column = 1; column <= Size; column++)
        {
            builder.Append($"{column,-3}");
        }

        builder.Append('\n');
        for (int row = 0; row < Size; row++)
        {
            builder.Append((char)('A' + row)).Append("  ");
            for (int column = 0; column < Size; column++)
            {
                var cell = new Coordinate(row, column);
                bool shot = _shots.Contains(cell);
                bool ship = HasShip(cell);
                char symbol = (shot, ship) switch
                {
                    (true, true) => 'X',
                    (true, false) => 'o',
                    (false, true) when revealShips => '#',
                    _ => '~',
                };
                builder.Append(symbol).Append("  ");
            }

            builder.Append('\n');
        }

        return builder.ToString();
    }

    private bool IsInside(Coordinate c) => c.Row >= 0 && c.Row < Size && c.Column >= 0 && c.Column < Size;

    private bool HasShip(Coordinate c) => _ships.Exists(s => s.Cells.Contains(c));
}
