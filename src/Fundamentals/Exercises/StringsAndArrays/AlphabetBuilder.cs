using System.Text;
using ConsoleUtils;

namespace Fundamentals.Exercises.StringsAndArrays;

public sealed class AlphabetBuilder : IExercise
{
    private static readonly char[] Alphabet = [.. Enumerable.Range('A', 26).Select(c => (char)c)];

    public string Title => "Alphabet builder";

    public string Description => "Builds a word by typing the array index of each letter. Type -1 to finish.";

    public void Run()
    {
        for (int i = 0; i < Alphabet.Length; i++)
        {
            Console.Write($"{i,2}:{Alphabet[i]}  ");
            if (i % 9 == 8)
            {
                Console.WriteLine();
            }
        }

        Console.WriteLine();
        var word = new StringBuilder();
        while (true)
        {
            int index = ConsoleInput.ReadInt($"Letter index (0-{Alphabet.Length - 1}, -1 to finish):", -1, Alphabet.Length - 1);
            if (index == -1)
            {
                break;
            }

            word.Append(Alphabet[index]);
            Console.WriteLine($"So far: {word}");
        }

        Console.WriteLine($"Your word is: {word}");
    }
}
