using ConsoleUtils;

namespace Games.Battleship;

/// <summary>Two players on the same computer. Hitting a ship earns another shot.</summary>
public static class BattleshipGame
{
    public const int BoardSize = 5;

    public static readonly IReadOnlyList<int> ShipLengths = [3, 2, 1];

    public static void Play(Random random)
    {
        BattleshipBoard[] boards = [new(BoardSize), new(BoardSize)];
        for (int player = 0; player < 2; player++)
        {
            PlaceFleet(player + 1, boards[player], random);
            HandOver($"Player {player + 1} is done. Pass the keyboard to player {(player == 0 ? 2 : 1)}.");
        }

        int shooter = 0;
        while (true)
        {
            BattleshipBoard target = boards[1 - shooter];
            Console.WriteLine($"Player {shooter + 1}'s turn. Enemy waters:");
            Console.Write(target.Render(revealShips: false));

            Coordinate shot = ReadCoordinate("Fire at (e.g. B3):");
            ShotResult result = target.Fire(shot);
            if (target.AllShipsSunk)
            {
                Console.Write(target.Render(revealShips: true));
                Console.WriteLine($"Hit and sunk! Player {shooter + 1} sank the whole fleet and wins!");
                return;
            }

            Console.WriteLine(result switch
            {
                ShotResult.Miss => "Splash... miss.",
                ShotResult.Hit => "Hit! Shoot again.",
                ShotResult.Sunk => "Hit and sunk! Shoot again.",
                _ => "You already fired there. Try another cell.",
            });

            if (result == ShotResult.Miss)
            {
                shooter = 1 - shooter;
                HandOver($"Pass the keyboard to player {shooter + 1}.");
            }
        }
    }

    private static void PlaceFleet(int player, BattleshipBoard board, Random random)
    {
        Console.WriteLine($"Player {player}, place your fleet (ships of length {string.Join(", ", ShipLengths)}).");
        if (ConsoleInput.ReadYesNo("Place the ships randomly?"))
        {
            board.PlaceRandomly(ShipLengths, random);
        }
        else
        {
            foreach (int length in ShipLengths)
            {
                Console.Write(board.Render(revealShips: true));
                while (true)
                {
                    Coordinate start = ReadCoordinate($"Start cell of the ship of length {length} (e.g. A1):");
                    Orientation orientation = length == 1 || ConsoleInput.ReadYesNo("Horizontal (right)? 'n' places it vertically (down)")
                        ? Orientation.Horizontal
                        : Orientation.Vertical;
                    if (board.TryPlaceShip(start, length, orientation, out string? error))
                    {
                        break;
                    }

                    Console.WriteLine(error);
                }
            }
        }

        Console.Write(board.Render(revealShips: true));
    }

    private static Coordinate ReadCoordinate(string prompt)
    {
        while (true)
        {
            if (Coordinate.TryParse(ConsoleInput.ReadString(prompt), BoardSize, out Coordinate coordinate))
            {
                return coordinate;
            }

            Console.WriteLine($"Use a row letter A-{(char)('A' + BoardSize - 1)} followed by a column 1-{BoardSize}.");
        }
    }

    /// <summary>Clears the screen so the next player cannot see the other fleet.</summary>
    private static void HandOver(string message)
    {
        ConsoleInput.Pause($"{message} Press Enter...");
        if (!Console.IsOutputRedirected)
        {
            Console.Clear();
        }
    }
}
