using Games.Battleship;

namespace Games.Tests;

public class CoordinateTests
{
    [Theory]
    [InlineData("A1", 0, 0)]
    [InlineData("b3", 1, 2)]
    [InlineData(" E5 ", 4, 4)]
    public void TryParse_ReadsLetterAndNumber(string text, int row, int column)
    {
        Assert.True(Coordinate.TryParse(text, 5, out Coordinate coordinate));
        Assert.Equal(new Coordinate(row, column), coordinate);
        Assert.Equal(text.Trim().ToUpperInvariant(), coordinate.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData("F1")]
    [InlineData("A0")]
    [InlineData("A6")]
    [InlineData("1A")]
    [InlineData("A-1")]
    public void TryParse_RejectsCellsOutsideTheBoard(string text)
    {
        Assert.False(Coordinate.TryParse(text, 5, out _));
    }
}

public class BattleshipBoardTests
{
    [Fact]
    public void TryPlaceShip_RejectsShipsThatDoNotFit()
    {
        var board = new BattleshipBoard(5);

        Assert.False(board.TryPlaceShip(new Coordinate(0, 3), 3, Orientation.Horizontal, out string? error));
        Assert.NotNull(error);
        Assert.Equal(0, board.ShipCount);
    }

    [Fact]
    public void TryPlaceShip_RejectsOverlappingShips()
    {
        var board = new BattleshipBoard(5);
        Assert.True(board.TryPlaceShip(new Coordinate(1, 0), 3, Orientation.Horizontal, out _));

        Assert.False(board.TryPlaceShip(new Coordinate(0, 1), 2, Orientation.Vertical, out _));
        Assert.True(board.TryPlaceShip(new Coordinate(2, 1), 2, Orientation.Vertical, out _));
    }

    [Fact]
    public void Fire_ReportsMissHitSunkAndRepeatedShots()
    {
        var board = new BattleshipBoard(5);
        board.TryPlaceShip(new Coordinate(0, 0), 2, Orientation.Vertical, out _);

        Assert.Equal(ShotResult.Miss, board.Fire(new Coordinate(4, 4)));
        Assert.Equal(ShotResult.Hit, board.Fire(new Coordinate(0, 0)));
        Assert.Equal(ShotResult.AlreadyFired, board.Fire(new Coordinate(0, 0)));
        Assert.False(board.AllShipsSunk);
        Assert.Equal(ShotResult.Sunk, board.Fire(new Coordinate(1, 0)));
        Assert.True(board.AllShipsSunk);
    }

    [Fact]
    public void PlaceRandomly_PlacesEveryShipWithoutOverlaps()
    {
        for (int seed = 0; seed < 50; seed++)
        {
            var board = new BattleshipBoard(5);
            board.PlaceRandomly([3, 2, 1], new Random(seed));

            Assert.Equal(3, board.ShipCount);
            string render = board.Render(revealShips: true);
            Assert.Equal(6, render.Count(c => c == '#')); // 3 + 2 + 1 cells, so nothing overlaps.
        }
    }

    [Fact]
    public void Render_HidesShipsFromTheOpponent()
    {
        var board = new BattleshipBoard(3);
        board.TryPlaceShip(new Coordinate(0, 0), 1, Orientation.Horizontal, out _);

        Assert.DoesNotContain("#", board.Render(revealShips: false));
        Assert.Contains("#", board.Render(revealShips: true));

        board.Fire(new Coordinate(0, 0));
        Assert.Contains("X", board.Render(revealShips: false));
    }
}
