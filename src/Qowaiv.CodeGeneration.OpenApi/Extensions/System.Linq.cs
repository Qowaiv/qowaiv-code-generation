namespace System.Linq;

internal static class LinqExtensions
{
    [Pure]
    public static bool HasMultiple<T>(this IEnumerable<T> enumerable) => enumerable.Skip(1).Any();
}
