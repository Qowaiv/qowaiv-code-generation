namespace Qowaiv.CodeGeneration.OpenApi;

public readonly struct OpenApiPath
{
    /// <summary>The splitter (/) character.</summary>
    public static readonly char Splitter = '/';

    public static readonly OpenApiPath Root;

    public OpenApiPath(string? path) => Value = path;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string? Value;

    public string Last => Value is null ? "{root}" : Value[(1 + Value.LastIndexOf(Splitter))..];

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Value ?? string.Empty;

    public OpenApiPath Child(string child) => new(Value is null ? $"#{Splitter}{child}" : $"{Value}{Splitter}{child}");
}
