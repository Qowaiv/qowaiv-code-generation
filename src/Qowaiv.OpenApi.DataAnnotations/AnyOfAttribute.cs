namespace Qowaiv.OpenApi.DataAnnotations;

/// <summary>Specifies the involved sub schema's of which at least one should be picked.</summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class AnyOfAttribute(params string[] schemas) : SubSchemasAttribute(schemas) { }
