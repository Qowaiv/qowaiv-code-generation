namespace Qowaiv.CodeGeneration.OpenApi;

/// <summary>Represents an Open API path.</summary>
public readonly struct OpenApiPath(string? path)
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

    /// <summary>Creates a child path.</summary>
    [Pure]
    public OpenApiPath Child(string child) => new(Value is null ? $"#{Splitter}{child}" : $"{Value}{Splitter}{child}");

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Value ?? string.Empty;
}
