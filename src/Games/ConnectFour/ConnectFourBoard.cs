using System.Text;

namespace Games.ConnectFour;

public enum Disc
{
    None,
    Red,
    Yellow,
}

/// <summary>Standard 7 x 6 Connect Four rules, without any console code.</summary>
public sealed class ConnectFourBoard
{
    public const int Columns = 7;
    public const int Rows = 6;
    public const int WinningLength = 4;

    /// <summary>Horizontal, vertical and both diagonals. The opposite directions are checked by negating them.</summary>
    private static readonly (int Row, int Column)[] Directions = [(0, 1), (1, 0), (1, 1), (1, -1)];

    // Row 0 is the bottom of the board.
    private readonly Disc[,] _cells = new Disc[Rows, Columns];

    public Disc this[int row, int column] => _cells[row, column];

    public bool IsFull => Enumerable.Range(0, Columns).All(IsColumnFull);

    public bool IsColumnFull(int column) => _cells[Rows - 1, column] != Disc.None;

    /// <summary>
    /// Drops a disc into <paramref name="column"/> (0-based). It falls to the lowest free row.
    /// Returns false if the column is full.
    /// </summary>
    public bool TryDrop(int column, Disc disc, out int row)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(column);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(column, Columns);
        if (disc == Disc.None)
        {
            throw new ArgumentException("A real disc must be dropped.", nameof(disc));
        }

        for (row = 0; row < Rows; row++)
        {
            if (_cells[row, column] == Disc.None)
            {
                _cells[row, column] = disc;
                return true;
            }
        }

        row = -1;
        return false;
    }

    /// <summary>
    /// Only the lines through the last disc can have changed, so it is enough to count, in each of the four
    /// directions, the consecutive discs of the same colour on both sides of it.
    /// </summary>
    public bool IsWinningMove(int row, int column)
    {
        Disc disc = _cells[row, column];
        if (disc == Disc.None)
        {
            return false;
        }

        foreach ((int dRow, int dColumn) in Directions)
        {
            int inLine = 1 + CountFrom(row, column, dRow, dColumn, disc) + CountFrom(row, column, -dRow, -dColumn, disc);
            if (inLine >= WinningLength)
            {
                return true;
            }
        }

        return false;
    }

    public string Render()
    {
        var builder = new StringBuilder();
        for (int row = Rows - 1; row >= 0; row--)
        {
            builder.Append('|');
            for (int column = 0; column < Columns; column++)
            {
                builder.Append(_cells[row, column] switch
                {
                    Disc.Red => " R ",
                    Disc.Yellow => " Y ",
                    _ => " . ",
                });
            }

            builder.Append("|\n");
        }

        builder.Append('+').Append(new string('-', Columns * 3)).Append("+\n ");
        for (int column = 1; column <= Columns; column++)
        {
            builder.Append($" {column} ");
        }

        return builder.Append('\n').ToString();
    }

    private int CountFrom(int row, int column, int dRow, int dColumn, Disc disc)
    {
        int count = 0;
        for (int r = row + dRow, c = column + dColumn;
             r >= 0 && r < Rows && c >= 0 && c < Columns && _cells[r, c] == disc;
             r += dRow, c += dColumn)
        {
            count++;
        }

        return count;
    }
}
