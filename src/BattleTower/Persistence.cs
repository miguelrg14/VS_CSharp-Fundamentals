using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using DataStructures;

namespace BattleTower;

internal sealed record CharacterData(string Name, int Health, int Strength, int Defense, int MagicPower, int MagicBarrier, int Energy);

/// <summary>Everything needed to continue a run, including the score and the enemies already defeated.</summary>
internal sealed record SaveData(CharacterData Player, CharacterData CurrentEnemy, List<CharacterData> WaitingEnemies, int Score, int DefeatedEnemies);

public sealed record ScoreEntry(string PlayerName, int Points, int Enemies, DateTime Date);

/// <summary>
/// The JSON code is generated at compile time instead of using reflection at runtime. It is faster,
/// and it lets the published executable be trimmed (unused framework code removed) without breaking saves.
/// </summary>
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(SaveData))]
[JsonSerializable(typeof(List<ScoreEntry>))]
internal sealed partial class BattleTowerJson : JsonSerializerContext;

/// <summary>Saves and loads runs as JSON. Missing or corrupt files are reported instead of crashing the game.</summary>
public static class SaveGame
{
    public static void Save(TowerRun run, string path) =>
        File.WriteAllText(path, JsonSerializer.Serialize(run.ToSaveData(), BattleTowerJson.Default.SaveData));

    /// <summary>A finished run is deleted so it cannot be continued (and scored) again.</summary>
    public static void Delete(string path) => File.Delete(path);

    public static bool TryLoad(string path, Dice dice, [NotNullWhen(true)] out TowerRun? run, [NotNullWhen(false)] out string? error)
    {
        run = null;
        error = null;
        if (!File.Exists(path))
        {
            error = "There is no saved game yet.";
            return false;
        }

        try
        {
            SaveData? data = JsonSerializer.Deserialize(File.ReadAllText(path), BattleTowerJson.Default.SaveData);
            if (data is null)
            {
                error = "The saved game is empty.";
                return false;
            }

            run = TowerRun.FromSaveData(data, dice);
            return true;
        }
        catch (Exception ex) when (ex is JsonException or ArgumentException)
        {
            error = $"The saved game is corrupt: {ex.Message}";
            return false;
        }
        catch (IOException ex)
        {
            error = $"The saved game could not be read: {ex.Message}";
            return false;
        }
    }
}

/// <summary>
/// High scores kept in a <see cref="SortedLinkedList{T}"/> ordered from best to worst,
/// from the DataStructures library.
/// </summary>
public sealed class HighScoreTable
{
    private static readonly IComparer<ScoreEntry> BestFirst =
        Comparer<ScoreEntry>.Create((a, b) => b.Points.CompareTo(a.Points));

    private readonly SortedLinkedList<ScoreEntry> _entries = new(BestFirst);

    public IReadOnlyCollection<ScoreEntry> Entries => _entries;

    public void Add(ScoreEntry entry) => _entries.Add(entry);

    public IEnumerable<ScoreEntry> Top(int count) => _entries.Take(count);

    public void Save(string path) =>
        File.WriteAllText(path, JsonSerializer.Serialize(_entries.ToList(), BattleTowerJson.Default.ListScoreEntry));

    /// <summary>Returns an empty table when the file does not exist or cannot be read.</summary>
    public static HighScoreTable Load(string path)
    {
        var table = new HighScoreTable();
        if (!File.Exists(path))
        {
            return table;
        }

        try
        {
            foreach (ScoreEntry entry in JsonSerializer.Deserialize(File.ReadAllText(path), BattleTowerJson.Default.ListScoreEntry) ?? [])
            {
                table.Add(entry);
            }
        }
        catch (Exception ex) when (ex is JsonException or IOException)
        {
            // An unreadable file just means starting a new table.
        }

        return table;
    }
}
