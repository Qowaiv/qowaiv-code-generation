using Qowaiv.CodeGeneration.Syntax;

namespace Qowaiv.CodeGeneration;

/// <summary>A collection of data needed to create a <see cref="Syntax.Property"/>.</summary>
[Inheritable]
public record PropertyData
{
    /// <inheritdoc cref="Property.Name" />
    public required CodeName Name { get; init; }

    /// <inheritdoc cref="Property.PropertyType" />
    public required Type PropertyType { get; init; }

    /// <inheritdoc cref="Property.DeclaringType" />
    public required Type DeclaringType { get; init; }

    /// <inheritdoc cref="Property.PropertyAccess" />
    public PropertyAccess PropertyAccess { get; init; }

    /// <inheritdoc cref="Property.AttributeInfos" />
    public IReadOnlyCollection<AttributeInfo>? Attributes { get; init; } = [];

    /// <inheritdoc cref="Property.Documentation" />
    public XmlDocumentation? Documentation { get; init; }

    /// <inheritdoc cref="Property.IsNullable" />
    public bool IsNullable { get; init; }

    /// <inheritdoc cref="Property.IsRequired" />
    public bool IsRequired { get; init; }

    /// <summary>Gets the name convention (default is Pascal Case).</summary>
    public CodeNameConvention NameConvention { get; init; } = CodeNameConvention.PascalCase;
}
