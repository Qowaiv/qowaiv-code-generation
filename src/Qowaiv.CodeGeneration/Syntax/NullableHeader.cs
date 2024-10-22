namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>
/// Represents a file header to indicate that nullability has been enabled.
/// </summary>
public sealed class NullableHeader : Code
{
    /// <summary>
    /// Represents a file header to indicate that nullability has been enabled
    /// </summary>
    public static readonly NullableHeader Enable = new();

    private NullableHeader() { }

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
        => Guard.NotNull(writer)
        .Line("#nullable enable")
        .Line();
}
