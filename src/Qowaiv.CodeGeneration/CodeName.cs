namespace Qowaiv.CodeGeneration;

/// <summary>Represents the name of a piece of code (class, property, field, method, etc.).</summary>
[DebuggerDisplay("{ToString()}, Convention = {Convention.Name}")]
public readonly struct CodeName : IFormattable
{
    private CodeName(IReadOnlyCollection<string> nameParts, CodeNameConvention convention)
    {
        parts = nameParts;
        Convention = convention;
    }

    private readonly IReadOnlyCollection<string> parts;

    /// <summary>The convention applied to create this (code) name.</summary>
    public CodeNameConvention Convention { get; }

    /// <summary>Creates a new (code) name applying the <see cref="CodeNameConvention.PascalCase"/>.</summary>
    [Pure]
    public static CodeName Create(string? name) => Create(name, CodeNameConvention.PascalCase);

    /// <summary>Creates a new (code) name applying the specified convention.</summary>
    [Pure]
    public static CodeName Create(string? name, CodeNameConvention convention)
        => new(Guard.NotNull(convention).Split(name), convention);

    /// <inheritdoc />
    [Pure]
    public override string ToString() => ToString(Convention);

    /// <inheritdoc />
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider) => format?.ToUpperInvariant() switch
    {
        null or "" => ToString(Convention),
        "PASCAL" or "PASCALCASE" => ToString(CodeNameConvention.CamelCase),
        "CAMEL" or "CAMELCASE" => ToString(CodeNameConvention.CamelCase),
        "KEBAB" or "KEBABCASE" => ToString(CodeNameConvention.KebabCase),
        "SNAKE" or "SNAKECASE" => ToString(CodeNameConvention.SnakeCase),
        "SCREAMINGSNAKE" => ToString(CodeNameConvention.ScreamingSnakeCase),
        _ => throw new FormatException($"Format '{format}' is unknown."),
    };

    /// <summary>Represents the code name as string according to the naming convention.</summary>
    [Pure]
    public string ToString(CodeNameConvention convention) => Guard.NotNull(convention).ToString(parts);

    /// <summary>Escapes the name with an '@' if it a C# keyword.</summary>
    [Pure]
    public static string EscapeKeyword(string name)
        => keywords.Contains(Guard.NotNullOrEmpty(name))
            ? "@" + name
            : name;

    private static readonly string[] keywords = ["default", "new", "class"];
}
