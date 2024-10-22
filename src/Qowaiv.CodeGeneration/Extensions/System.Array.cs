namespace System;

internal static class QowaivCodeGenerationArrayExtensions
{
    /// <inheritdoc cref="Array.Find{T}(T[], Predicate{T})" />
    [Pure]
    public static T? Find<T>(this T[] array, Predicate<T> match)
        => Array.Find(array, match);

    /// <inheritdoc cref="Array.Exists{T}(T[], Predicate{T})" />
    [Pure]
    public static bool Exists<T>(this T[] array, Predicate<T> match)
        => Array.Exists(array, match);
}
