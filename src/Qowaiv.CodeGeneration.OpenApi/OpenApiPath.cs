using Qowaiv.Text;

namespace Qowaiv.CodeGeneration.OpenApi;

/// <summary>Represents an Open API path.</summary>
public readonly struct OpenApiPath(string? path) : IEquatable<OpenApiPath>
{
    /// <summary>The splitter (/) character.</summary>
    public static readonly char Splitter = '/';

    /// <summary>Represents an (empty) root.</summary>
    public static readonly OpenApiPath Root;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string? Value = path;

    /// <summary>Gets the last item in the path.</summary>
    public string Last => this[^1];

    /// <summary>Gets the specified item.</summary>
    public string this[Index index]
        => (Value ?? "{root}").Split(Splitter)[index];

    /// <summary>Returns true if the path matches the pattern.</summary>
    /// <remarks>
    /// Wildcards are supported.
    /// </remarks>
    [Pure]
    public bool Matches(string pattern, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
    {
        var paths = ToString().Split(Splitter);
        var patterns = pattern?.Split(Splitter) ?? [];

        if (patterns.Length > 0 && patterns.Length <= paths.Length)
        {
            for (var i = 1; i <= patterns.Length; i++)
            {
                if (!WildcardPattern.IsMatch(patterns[^i], paths[^i], default, comparisonType))
                {
                    return false;
                }
            }
            return true;
        }
        else return false;
    }

    /// <summary>Creates a child path.</summary>
    [Pure]
    public OpenApiPath Child(string child) => new(Value is null ? $"#{Splitter}{child}" : $"{Value}{Splitter}{child}");

    /// <summary>Gets the parent path.</summary>
    public OpenApiPath Parent
        => Value?.LastIndexOf(Splitter) is { } index && index > -1
        ? new(Value[..index])
        : Root;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Value ?? string.Empty;

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is OpenApiPath other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(OpenApiPath other) => ToString() == other.ToString();

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => ToString().GetHashCode();

    /// <summary>Returns true if of paths are equal.</summary>
    public static bool operator ==(OpenApiPath l, OpenApiPath r) => l.Equals(r);

    /// <summary>Returns false if of paths are equal.</summary>
    public static bool operator !=(OpenApiPath l, OpenApiPath r) => !(l == r);
}
