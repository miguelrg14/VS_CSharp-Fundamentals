using ConsoleUtils;
using Games.Battleship;
using Games.ConnectFour;
using Games.TicTacToe;

namespace Games;

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        try
        {
            while (ConsoleMenu.Show("Console games (2 players, same keyboard)", ["Tic-tac-toe", "Connect Four", "Battleship"], "Exit") is int choice)
            {
                switch (choice)
                {
                    case 0:
                        TicTacToeGame.Play();
                        break;
                    case 1:
                        ConnectFourGame.Play();
                        break;
                    case 2:
                        BattleshipGame.Play(Random.Shared);
                        break;
                }
            }
        }
        catch (EndOfStreamException)
        {
            // Redirected input ran out.
        }
    }
}
