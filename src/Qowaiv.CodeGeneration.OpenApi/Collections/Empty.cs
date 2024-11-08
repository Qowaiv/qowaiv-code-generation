namespace Qowaiv.CodeGeneration.OpenApi.Collections;

internal static class Empty
{
    [Pure]
    public static List<T> List<T>() => [];

    [Pure]
    public static HashSet<T> HashSet<T>() => [];

    [Pure]
    public static TypeCollector Types() => [];

    [Pure]
    public static PropertyCollector Properties() => [];
}
