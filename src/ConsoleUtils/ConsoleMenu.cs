namespace ConsoleUtils;

/// <summary>A numbered console menu. Option 0 is always "Back" / "Exit".</summary>
public static class ConsoleMenu
{
    /// <summary>
    /// Shows <paramref name="options"/> numbered from 1 and returns the chosen index (0-based),
    /// or <c>null</c> when the user picks the back/exit option.
    /// </summary>
    public static int? Show(string title, IReadOnlyList<string> options, string backLabel = "Back")
    {
        Console.WriteLine();
        Console.WriteLine($"=== {title} ===");
        for (int i = 0; i < options.Count; i++)
        {
            Console.WriteLine($"{i + 1,2}. {options[i]}");
        }

        Console.WriteLine($"{0,2}. {backLabel}");

        int choice = ConsoleInput.ReadInt(">", 0, options.Count);
        return choice == 0 ? null : choice - 1;
    }

    /// <summary>Prints a section header used by exercises and games.</summary>
    public static void Header(string title)
    {
        Console.WriteLine();
        Console.WriteLine(title);
        Console.WriteLine(new string('-', title.Length));
    }
}
