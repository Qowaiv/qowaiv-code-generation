namespace Qowaiv.CodeGeneration;

/// <summary>Represents the name of a piece of code (class, property, field, method, etc.).</summary>
[DebuggerDisplay("{ToString()}, Covention = {Convention.Name}")]
public readonly struct CodeName : IFormattable
{
    private CodeName(IReadOnlyCollection<string> nameParts, CodeNameConvention nameConvention)
    {
        parts = nameParts;
        convention = nameConvention;
    }

    private readonly IReadOnlyCollection<string> parts;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly CodeNameConvention? convention;

    /// <summary>The convention applied to create this (code) name.</summary>
    public CodeNameConvention Convention => convention ?? CodeNameConvention.None;

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

    /// <summary>Represents the code name as property name for the specified enclosing type.</summary>
    /// <param name="enclosing">
    /// The enclosing type.
    /// </param>
    /// <param name="convention">
    /// The optional naming convention, <see cref="CodeNameConvention.PascalCase"/> if not specified.
    /// </param>
    [Pure]
    public string PropertyFor(Type enclosing, CodeNameConvention? convention = null)
    {
        Guard.NotNull(enclosing);
        convention ??= CodeNameConvention.PascalCase;

        var name = convention.ToString(parts);
        return enclosing.Name == name || char.IsAsciiDigit(name[0])
            ? '_' + name
            : EscapeKeyword(name);
    }

    /// <summary>Escapes the name with an '@' if it a C# keyword.</summary>
    [Pure]
    public static string EscapeKeyword(string name)
        => keywords.Contains(Guard.NotNullOrEmpty(name))
            ? "@" + name
            : name;

    private static readonly string[] keywords = ["default", "new", "class"];
}
