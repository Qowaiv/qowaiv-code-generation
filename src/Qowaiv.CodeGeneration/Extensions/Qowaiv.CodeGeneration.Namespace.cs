namespace Qowaiv.CodeGeneration;

/// <summary>Extensions on <see cref="Namespace"/>.</summary>
public static class NamespaceExtensions
{
    /// <summary>Gets the <see cref="Namespace"/> of the type.</summary>
    [Pure]
    public static Namespace NS(this Type type)
        => Guard.NotNull(type).Namespace is { Length: > 0 } ns
            ? new(ns)
            : Namespace.Empty;
}
