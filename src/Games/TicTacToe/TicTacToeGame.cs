using ConsoleUtils;

namespace Games.TicTacToe;

public static class TicTacToeGame
{
    public static void Play()
    {
        var wins = new Dictionary<Mark, int> { [Mark.X] = 0, [Mark.O] = 0 };
        int draws = 0;
        Mark starter = Mark.X;

        do
        {
            Mark winner = PlayRound(starter);
            if (winner == Mark.None)
            {
                draws++;
                Console.WriteLine("It's a draw!");
            }
            else
            {
                wins[winner]++;
                Console.WriteLine($"Player {winner} wins!");
            }

            Console.WriteLine($"Score  X: {wins[Mark.X]}  O: {wins[Mark.O]}  draws: {draws}");
            starter = Opponent(starter); // The other player starts the next round.
        }
        while (ConsoleInput.ReadYesNo("Play another round?"));
    }

    private static Mark PlayRound(Mark starter)
    {
        var board = new TicTacToeBoard();
        Mark current = starter;
        while (!board.IsOver)
        {
            Console.WriteLine();
            Console.Write(board.Render());
            int cell = ConsoleInput.ReadInt($"Player {current}, choose a cell (1-9):", 1, 9) - 1;
            if (!board.TryPlace(cell, current))
            {
                Console.WriteLine("That cell is already taken.");
                continue;
            }

            current = Opponent(current);
        }

        Console.WriteLine();
        Console.Write(board.Render());
        return board.Winner;
    }

    private static Mark Opponent(Mark mark) => mark == Mark.X ? Mark.O : Mark.X;
}
