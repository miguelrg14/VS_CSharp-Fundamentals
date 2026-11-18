using ConsoleUtils;

namespace Games.ConnectFour;

public static class ConnectFourGame
{
    public static void Play()
    {
        do
        {
            PlayRound();
        }
        while (ConsoleInput.ReadYesNo("Play again?"));
    }

    private static void PlayRound()
    {
        var board = new ConnectFourBoard();
        Disc current = Disc.Red;
        while (true)
        {
            Console.WriteLine();
            Console.Write(board.Render());
            int column = ConsoleInput.ReadInt($"{current}, choose a column (1-{ConnectFourBoard.Columns}):", 1, ConnectFourBoard.Columns) - 1;
            if (!board.TryDrop(column, current, out int row))
            {
                Console.WriteLine("That column is full, choose another one.");
                continue;
            }

            if (board.IsWinningMove(row, column))
            {
                Console.WriteLine();
                Console.Write(board.Render());
                Console.WriteLine($"{current} connects four and wins!");
                return;
            }

            if (board.IsFull)
            {
                Console.WriteLine();
                Console.Write(board.Render());
                Console.WriteLine("The board is full: it's a draw.");
                return;
            }

            current = current == Disc.Red ? Disc.Yellow : Disc.Red;
        }
    }
}
