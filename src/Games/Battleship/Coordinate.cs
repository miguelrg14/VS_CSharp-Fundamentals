using System.Globalization;

namespace Games.Battleship;

/// <summary>A cell on the board. Shown to players as a letter for the row and a number for the column, e.g. "B3".</summary>
public readonly record struct Coordinate(int Row, int Column)
{
    /// <summary>Parses "B3" / "b3" into row 1, column 2 on a board of the given size.</summary>
    public static bool TryParse(string text, int boardSize, out Coordinate coordinate)
    {
        coordinate = default;
        text = text.Trim().ToUpperInvariant();
        if (text.Length < 2 || text[0] < 'A' || text[0] >= 'A' + boardSize)
        {
            return false;
        }

        if (!int.TryParse(text.AsSpan(1), NumberStyles.None, CultureInfo.InvariantCulture, out int column)
            || column < 1 || column > boardSize)
        {
            return false;
        }

        coordinate = new Coordinate(text[0] - 'A', column - 1);
        return true;
    }

    public override string ToString() => $"{(char)('A' + Row)}{Column + 1}";
}

public enum Orientation
{
    Horizontal,
    Vertical,
}

public enum ShotResult
{
    Miss,
    Hit,
    Sunk,
    AlreadyFired,
}
