namespace BuildingBlocks.Common.Extensions;

/// <summary>
/// Extension methods for IEnumerable operations.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Checks if the enumerable is null or empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="source">The enumerable to check.</param>
    /// <returns>true if the enumerable is null or empty; otherwise, false.</returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source)
    {
        return source is null || !source.Any();
    }

    /// <summary>
    /// Returns distinct elements from a sequence by using a key selector.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <param name="source">The enumerable to filter.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <returns>An enumerable of distinct elements.</returns>
    public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
    {
        var seenKeys = new HashSet<TKey>();
        foreach (var element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }

    /// <summary>
    /// Splits the enumerable into chunks of the specified size.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="source">The enumerable to split.</param>
    /// <param name="chunkSize">The size of each chunk.</param>
    /// <returns>An enumerable of chunks.</returns>
    public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
    {
        if (chunkSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(chunkSize), "Chunk size must be greater than 0.");

        var chunk = new List<T>(chunkSize);
        foreach (var item in source)
        {
            chunk.Add(item);
            if (chunk.Count == chunkSize)
            {
                yield return chunk;
                chunk = new List<T>(chunkSize);
            }
        }

        if (chunk.Count > 0)
            yield return chunk;
    }

    /// <summary>
    /// Performs an action on each element of the enumerable.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="source">The enumerable.</param>
    /// <param name="action">The action to perform on each element.</param>
    /// <returns>The original enumerable.</returns>
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item);
        }
        return source;
    }

    /// <summary>
    /// Performs an action on each element of the enumerable with its index.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="source">The enumerable.</param>
    /// <param name="action">The action to perform on each element and its index.</param>
    /// <returns>The original enumerable.</returns>
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
    {
        var index = 0;
        foreach (var item in source)
        {
            action(item, index++);
        }
        return source;
    }

    /// <summary>
    /// Converts the enumerable to a read-only collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="source">The enumerable to convert.</param>
    /// <returns>A read-only collection.</returns>
    public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
    {
        return source.ToList().AsReadOnly();
    }

    /// <summary>
    /// Converts the enumerable to a read-only list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="source">The enumerable to convert.</param>
    /// <returns>A read-only list.</returns>
    public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> source)
    {
        return source.ToList().AsReadOnly();
    }
}