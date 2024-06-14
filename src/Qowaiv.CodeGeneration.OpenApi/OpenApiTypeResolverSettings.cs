using Qowaiv.CodeGeneration.Syntax;
using Qowaiv.Diagnostics.Contracts;

namespace Qowaiv.CodeGeneration.OpenApi;

[Inheritable]
public class OpenApiTypeResolverSettings
{
    /// <summary>Models are generated being sealed (default is true).</summary>
    public bool Sealed { get; init; } = true;

    /// <summary>Models are generated being partial (default is true).</summary>
    public bool Partial { get; init; } = true;

    /// <summary>Model type to generate (default is <see cref="ModelType.Record"/>).</summary>
    public ModelType ModelType { get; init; } = ModelType.Record;

    /// <summary>Property access to generate (default is <see cref="PropertyAccess.InitOnly"/>).</summary>
    public PropertyAccess PropertyAccess { get; init; }

    /// <summary>Property required type (default is <see cref="RequiredTypes.IsRequired"/>).</summary>
    public RequiredTypes RequiredType { get; init; } = RequiredTypes.IsRequired;

    /// <summary>Indicates that all properties that hare value types should be nullable (default is false).</summary>
    /// <remarks>
    /// Value types like <see cref="Guid"/> and <see cref="EmailAddress"/> that
    /// nullable by design are excluded from this setting.
    /// </remarks>
    public bool NullableValueTypes { get; init; }
}
