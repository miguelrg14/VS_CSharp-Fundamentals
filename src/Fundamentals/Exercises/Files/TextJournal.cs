using ConsoleUtils;

namespace Fundamentals.Exercises.Files;

public sealed class TextJournal : IExercise
{
    public const string FileName = "journal.txt";
    private const string ExitWord = "exit";

    public string Title => "Text journal";

    public string Description => $"Shows what {FileName} already contains and appends new lines until you type \"{ExitWord}\".";

    public void Run()
    {
        string path = Path.GetFullPath(FileName);
        if (File.Exists(path))
        {
            Console.WriteLine($"Current content of {path}:");
            foreach (string line in File.ReadLines(path))
            {
                Console.WriteLine($"  {line}");
            }

            if (ConsoleInput.ReadYesNo("Delete the current content and start again?"))
            {
                File.WriteAllText(path, string.Empty);
            }
        }
        else
        {
            Console.WriteLine($"{path} does not exist yet; it will be created.");
        }

        // 'using' closes the file even if an exception is thrown while writing.
        using StreamWriter writer = File.AppendText(path);
        int added = 0;
        while (true)
        {
            string entry = ConsoleInput.ReadString($"New line (\"{ExitWord}\" to finish):", allowEmpty: true);
            if (entry.Equals(ExitWord, StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            writer.WriteLine(entry);
            added++;
        }

        Console.WriteLine($"{added} line(s) added to {FileName}.");
    }
}
