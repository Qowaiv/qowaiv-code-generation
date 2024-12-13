namespace Qowaiv.OpenApi.DataAnnotations;

/// <summary>Specifies the involved sub schema's.</summary>
public abstract class SubSchemasAttribute(params string[] schemas) : Attribute
{
    /// <summary>The specified sub schema's.</summary>
    public IReadOnlyCollection<string> Schemas { get; } = schemas;
}
