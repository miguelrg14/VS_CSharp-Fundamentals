namespace DataStructures;

public static class Sorting
{
    /// <summary>
    /// Sorts <paramref name="items"/> in place with bubble sort.
    /// Each pass pushes the largest remaining item to the end, so the next pass can stop one
    /// position earlier, and the loop ends as soon as a pass makes no swaps (O(n) on sorted input).
    /// </summary>
    public static void BubbleSort<T>(IList<T> items, IComparer<T>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(items);
        comparer ??= Comparer<T>.Default;

        int unsortedLength = items.Count;
        bool swapped = true;
        while (swapped && unsortedLength > 1)
        {
            swapped = false;
            for (int i = 0; i < unsortedLength - 1; i++)
            {
                if (comparer.Compare(items[i], items[i + 1]) > 0)
                {
                    (items[i], items[i + 1]) = (items[i + 1], items[i]);
                    swapped = true;
                }
            }

            unsortedLength--;
        }
    }
}
