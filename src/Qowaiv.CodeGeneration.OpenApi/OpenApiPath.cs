namespace Qowaiv.CodeGeneration.OpenApi;

public readonly struct OpenApiPath
{
    private const char Split = '/';

    public static readonly OpenApiPath Root;

    public OpenApiPath(string? path) => Value = path;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string? Value;

    public string Last => Value is null ? "{root}" : Value[(1 + Value.LastIndexOf(Split))..];

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Value ?? string.Empty;

    public OpenApiPath Child(string child) => new(Value is null ? $"#{Split}{child}" : $"{Value}{Split}{child}");
}
