namespace Qowaiv.OpenApi.DataAnnotations;

/// <summary>Specifies the involved sub schema's of which all should be picked.</summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class AllOfAttribute(params string[] schemas) : SubSchemasAttribute(schemas) { }
