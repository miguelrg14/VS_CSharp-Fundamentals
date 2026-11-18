using System.Reflection;
using ConsoleUtils;

namespace BattleTower;

/// <summary>Console front end. All the rules live in <see cref="TowerRun"/>.</summary>
public static class Program
{
    private const string SavePath = "battletower-save.json";
    private const string ScoresPath = "battletower-scores.json";

    private static readonly Dice GameDice = new(Random.Shared);

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine(ReadAsset("title.txt"));
        try
        {
            MainMenu();
        }
        catch (EndOfStreamException)
        {
            // Redirected input ran out.
        }
    }

    private static void MainMenu()
    {
        while (ConsoleMenu.Show("Battle Tower", ["New game", "Continue saved game", "High scores"], "Exit") is int option)
        {
            switch (option)
            {
                case 0:
                    string name = ConsoleInput.ReadString("Your hero's name:");
                    int enemies = ConsoleInput.ReadInt("How many enemies will you face (1-10)?", 1, 10);
                    Play(TowerRun.StartNew(name, enemies, GameDice));
                    break;
                case 1:
                    if (SaveGame.TryLoad(SavePath, GameDice, out TowerRun? run, out string? error))
                    {
                        Play(run!);
                    }
                    else
                    {
                        Console.WriteLine(error);
                    }

                    break;
                case 2:
                    ShowHighScores(HighScoreTable.Load(ScoresPath));
                    break;
            }
        }
    }

    private static void Play(TowerRun run)
    {
        Console.WriteLine($"{run.CurrentEnemy.Name} stands in front of you.");
        string[] actions = ["Attack", "Magic missile", "Analyze", "Rest", "Save game"];
        while (run.State == RunState.InProgress)
        {
            Console.WriteLine();
            Console.WriteLine($"{run.Player.Name}: {run.Player.Health} health, {run.Player.Energy} energy | " +
                              $"{run.CurrentEnemy.Name}: {run.CurrentEnemy.Health} health | score {run.Score}");

            int? choice = ConsoleMenu.Show("Your move", actions, "Back to main menu (progress since the last save is lost)");
            switch (choice)
            {
                case null:
                    return;
                case 0:
                    Print(run.PlayTurn(PlayerAction.Attack));
                    break;
                case 1:
                    Print(run.PlayTurn(PlayerAction.MagicMissile));
                    break;
                case 2:
                    Console.WriteLine(run.Player);
                    Console.WriteLine(run.CurrentEnemy);
                    Console.WriteLine($"Enemies waiting: {run.WaitingEnemies.Count}");
                    break;
                case 3:
                    Print(run.PlayTurn(PlayerAction.Rest));
                    break;
                case 4:
                    SaveGame.Save(run, SavePath);
                    Console.WriteLine($"Game saved to {Path.GetFullPath(SavePath)}.");
                    break;
            }
        }

        if (run.State == RunState.Lost)
        {
            Console.WriteLine(ReadAsset("game-over.txt"));
            Console.WriteLine($"Final score: {run.Score} (only victories enter the high score table).");
            return;
        }

        Console.WriteLine($"VICTORY! Final score: {run.Score}");
        HighScoreTable scores = HighScoreTable.Load(ScoresPath);
        scores.Add(new ScoreEntry(run.Player.Name, run.Score, run.DefeatedEnemies, DateTime.Now));
        scores.Save(ScoresPath);
        ShowHighScores(scores);
    }

    private static void ShowHighScores(HighScoreTable scores)
    {
        ConsoleMenu.Header("High scores");
        if (scores.Entries.Count == 0)
        {
            Console.WriteLine("No victories yet.");
            return;
        }

        int position = 1;
        foreach (ScoreEntry entry in scores.Top(10))
        {
            Console.WriteLine($"{position++,2}. {entry.Points,6} pts  {entry.PlayerName,-15} {entry.Enemies,2} enemies  {entry.Date:yyyy-MM-dd HH:mm}");
        }
    }

    private static void Print(IEnumerable<string> log)
    {
        foreach (string line in log)
        {
            Console.WriteLine($"  {line}");
        }
    }

    private static string ReadAsset(string fileName)
    {
        using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"BattleTower.Assets.{fileName}");
        if (stream is null)
        {
            return string.Empty;
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
