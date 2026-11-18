using Games.ConnectFour;
using Games.TicTacToe;

namespace Games.Tests;

public class TicTacToeBoardTests
{
    private static TicTacToeBoard Play(params (int Cell, Mark Mark)[] moves)
    {
        var board = new TicTacToeBoard();
        foreach ((int cell, Mark mark) in moves)
        {
            Assert.True(board.TryPlace(cell, mark));
        }

        return board;
    }

    [Fact]
    public void Row_Wins()
    {
        TicTacToeBoard board = Play((0, Mark.X), (3, Mark.O), (1, Mark.X), (4, Mark.O), (2, Mark.X));

        Assert.Equal(Mark.X, board.Winner);
        Assert.True(board.IsOver);
    }

    [Fact]
    public void Diagonal_Wins()
    {
        TicTacToeBoard board = Play((2, Mark.O), (4, Mark.O), (6, Mark.O));

        Assert.Equal(Mark.O, board.Winner);
    }

    [Fact]
    public void FullBoardWithoutLine_IsADraw()
    {
        // X O X
        // X O O
        // O X X
        TicTacToeBoard board = Play(
            (0, Mark.X), (1, Mark.O), (2, Mark.X),
            (3, Mark.X), (4, Mark.O), (5, Mark.O),
            (6, Mark.O), (7, Mark.X), (8, Mark.X));

        Assert.Equal(Mark.None, board.Winner);
        Assert.True(board.IsFull);
        Assert.True(board.IsOver);
    }

    [Fact]
    public void TakenCell_IsRejected()
    {
        TicTacToeBoard board = Play((4, Mark.X));

        Assert.False(board.TryPlace(4, Mark.O));
        Assert.Equal(Mark.X, board[4]);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(9)]
    public void CellOutsideTheBoard_Throws(int cell)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new TicTacToeBoard().TryPlace(cell, Mark.X));
    }
}

public class ConnectFourBoardTests
{
    private static (int Row, int Column) Drop(ConnectFourBoard board, int column, Disc disc)
    {
        Assert.True(board.TryDrop(column, disc, out int row));
        return (row, column);
    }

    [Fact]
    public void Discs_FallToTheLowestFreeRow()
    {
        var board = new ConnectFourBoard();

        Assert.Equal((0, 3), Drop(board, 3, Disc.Red));
        Assert.Equal((1, 3), Drop(board, 3, Disc.Yellow));
    }

    [Fact]
    public void FullColumn_RejectsMoreDiscs()
    {
        var board = new ConnectFourBoard();
        for (int i = 0; i < ConnectFourBoard.Rows; i++)
        {
            Drop(board, 0, i % 2 == 0 ? Disc.Red : Disc.Yellow);
        }

        Assert.True(board.IsColumnFull(0));
        Assert.False(board.TryDrop(0, Disc.Red, out _));
    }

    [Fact]
    public void FourInARow_Horizontally_Wins_EvenWhenTheLastDiscIsInTheMiddle()
    {
        var board = new ConnectFourBoard();
        Drop(board, 0, Disc.Red);
        Drop(board, 1, Disc.Red);
        Drop(board, 3, Disc.Red);

        (int row, int column) = Drop(board, 2, Disc.Red);

        Assert.True(board.IsWinningMove(row, column));
    }

    [Fact]
    public void FourInAColumn_Wins()
    {
        var board = new ConnectFourBoard();
        (int Row, int Column) last = default;
        for (int i = 0; i < 4; i++)
        {
            last = Drop(board, 6, Disc.Yellow);
        }

        Assert.True(board.IsWinningMove(last.Row, last.Column));
    }

    [Fact]
    public void FourInADiagonal_Wins()
    {
        // Builds the staircase:  . . . R
        //                        . . R Y
        //                        . R Y Y
        //                        R Y Y Y
        var board = new ConnectFourBoard();
        Drop(board, 0, Disc.Red);
        Drop(board, 1, Disc.Yellow);
        Drop(board, 1, Disc.Red);
        Drop(board, 2, Disc.Yellow);
        Drop(board, 2, Disc.Yellow);
        Drop(board, 2, Disc.Red);
        Drop(board, 3, Disc.Yellow);
        Drop(board, 3, Disc.Yellow);
        Drop(board, 3, Disc.Yellow);

        (int row, int column) = Drop(board, 3, Disc.Red);

        Assert.True(board.IsWinningMove(row, column));
    }

    [Fact]
    public void ThreeInARow_DoesNotWin()
    {
        var board = new ConnectFourBoard();
        Drop(board, 0, Disc.Red);
        Drop(board, 1, Disc.Red);

        (int row, int column) = Drop(board, 2, Disc.Red);

        Assert.False(board.IsWinningMove(row, column));
    }

    [Fact]
    public void LineBrokenByTheOtherColour_DoesNotWin()
    {
        var board = new ConnectFourBoard();
        Drop(board, 0, Disc.Red);
        Drop(board, 1, Disc.Red);
        Drop(board, 2, Disc.Yellow);
        Drop(board, 3, Disc.Red);

        (int row, int column) = Drop(board, 4, Disc.Red);

        Assert.False(board.IsWinningMove(row, column));
    }
}
