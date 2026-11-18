using System.Text;

namespace Games.TicTacToe;

public enum Mark
{
    None,
    X,
    O,
}

/// <summary>Game rules only, with no console code, so they can be unit tested.</summary>
public sealed class TicTacToeBoard
{
    public const int CellCount = 9;

    /// <summary>Every line of three cells that wins the game: rows, columns and diagonals.</summary>
    private static readonly int[][] Lines =
    [
        [0, 1, 2], [3, 4, 5], [6, 7, 8],
        [0, 3, 6], [1, 4, 7], [2, 5, 8],
        [0, 4, 8], [2, 4, 6],
    ];

    private readonly Mark[] _cells = new Mark[CellCount];

    public Mark this[int cell] => _cells[cell];

    public bool IsFull => _cells.All(c => c != Mark.None);

    /// <summary>The player who completed a line, or <see cref="Mark.None"/>.</summary>
    public Mark Winner
    {
        get
        {
            foreach (int[] line in Lines)
            {
                Mark first = _cells[line[0]];
                if (first != Mark.None && _cells[line[1]] == first && _cells[line[2]] == first)
                {
                    return first;
                }
            }

            return Mark.None;
        }
    }

    public bool IsOver => Winner != Mark.None || IsFull;

    /// <summary>Places a mark in a free cell (0-8). Returns false if the cell is already taken.</summary>
    public bool TryPlace(int cell, Mark mark)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(cell);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(cell, CellCount);
        if (mark == Mark.None)
        {
            throw new ArgumentException("A real mark must be placed.", nameof(mark));
        }

        if (_cells[cell] != Mark.None)
        {
            return false;
        }

        _cells[cell] = mark;
        return true;
    }

    /// <summary>Empty cells show their number (1-9) so players know what to type.</summary>
    public string Render()
    {
        var builder = new StringBuilder();
        for (int row = 0; row < 3; row++)
        {
            builder.Append(' ');
            for (int column = 0; column < 3; column++)
            {
                int cell = (row * 3) + column;
                builder.Append(_cells[cell] == Mark.None ? (char)('1' + cell) : _cells[cell].ToString()[0]);
                builder.Append(column < 2 ? " | " : "\n");
            }

            if (row < 2)
            {
                builder.Append("---+---+---\n");
            }
        }

        return builder.ToString();
    }
}
