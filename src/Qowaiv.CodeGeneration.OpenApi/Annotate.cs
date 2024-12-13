using Qowaiv.CodeGeneration.Syntax;

namespace Qowaiv.CodeGeneration.OpenApi;

/// <summary>Annotate models with Open API annotations.</summary>
public static class Annotate
{
    /// <inheritdoc cref="Qowaiv.OpenApi.DataAnnotations.AllOfAttribute" />
    [Pure]
    public static AttributeInfo AllOf(params string[] types)
        => new(typeof(Qowaiv.OpenApi.DataAnnotations.AllOfAttribute), types);

    /// <inheritdoc cref="Qowaiv.OpenApi.DataAnnotations.AnyOfAttribute" />
    [Pure]
    public static AttributeInfo AnyOf(params string[] types)
        => new(typeof(Qowaiv.OpenApi.DataAnnotations.AnyOfAttribute), types);

    /// <inheritdoc cref="Qowaiv.OpenApi.DataAnnotations.OneOfAttribute" />
    [Pure]
    public static AttributeInfo OneOf(params string[] types)
        => new(typeof(Qowaiv.OpenApi.DataAnnotations.OneOfAttribute), types);
}
