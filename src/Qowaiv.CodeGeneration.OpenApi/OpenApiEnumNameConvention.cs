namespace Qowaiv.CodeGeneration.OpenApi;

/// <summary>Implements Open API specific name convention for enum values.</summary>
public class OpenApiEnumNameConvention : CodeNameConvention
{
    internal static readonly OpenApiEnumNameConvention Instance = new();

    /// <inheritdoc />
    public override string Name => "Open API enum name";

    /// <summary>Splitters used to seperates parts of a name.</summary>
    protected virtual IReadOnlyCollection<char> Splitters { get; } =
    [
        ' ', (char)160, '\t', '\r', '\n',
        '-', '_', '.', '!', ',', ';', '#', '/', '\\',
    ];

    /// <inheritdoc />
    [Pure]
    public override IReadOnlyCollection<string> Split(IEnumerable<string?> parts)
        => Guard.NotNull(parts)
            .OfType<string>()
            .Where(p => p is { Length: > 0 })
            .Select(Format)
            .SelectMany(p => p.Split([.. Splitters], StringSplitOptions.RemoveEmptyEntries))
            .ToArray();

    /// <summary>Formats a part.</summary>
    [Pure]
    protected virtual string Format(string part, int index) => part
        .Replace("+", " pls ")
        .Replace("(", string.Empty)
        .Replace(")", string.Empty);

    /// <inheritdoc />
    [Pure]
    public override string ToString(IReadOnlyCollection<string> parts)
    {
        var name = string.Join('_', Guard.NotNull(parts).Where(p => p is { Length: > 0 }));
        if (char.IsAsciiDigit(name[0]))
        {
            name = '_' + name;
        }
        return CodeName.EscapeKeyword(name);
    }
}
