using Qowaiv.Diagnostics.Contracts;

namespace Qowaiv.CodeGeneration.OpenApi;

[Inheritable]
public class OpenApiTypeResolverSettings
{
    /// <summary>Models are generated being sealed (default is true).</summary>
    public bool Sealed { get; init; } = true;

    /// <summary>Models are generated being partial (default is true).</summary>
    public bool Partial { get; init; } = true;

    /// <summary>Model type to generate (default is class).</summary>
    public ModelType ModelType { get; init; }
}
