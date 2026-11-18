using ConsoleUtils;

namespace Fundamentals.Exercises.StringsAndArrays;

public sealed class PalindromeNumber : IExercise
{
    public string Title => "Palindrome number";

    public string Description => "Checks whether a number reads the same backwards, using only arithmetic.";

    public void Run()
    {
        int number = ConsoleInput.ReadInt("Number:", 0);
        Console.WriteLine(IsPalindrome(number) ? $"{number} is a palindrome." : $"{number} is not a palindrome.");
    }

    /// <summary>
    /// Rebuilds the number backwards digit by digit (% 10 takes the last digit, / 10 removes it).
    /// Note that comparing two arrays with == compares references, not contents, so it would always be false.
    /// </summary>
    public static bool IsPalindrome(long number)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(number);

        long remaining = number;
        long reversed = 0;
        while (remaining > 0)
        {
            reversed = (reversed * 10) + (remaining % 10);
            remaining /= 10;
        }

        return reversed == number;
    }
}
