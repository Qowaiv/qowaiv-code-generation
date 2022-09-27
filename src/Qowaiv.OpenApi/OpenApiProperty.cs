using Microsoft.OpenApi.Models;

namespace Qowaiv.OpenApi;

public readonly struct OpenApiProperty
{
    public readonly string Name;
    public readonly OpenApiSchema Schema;

    public OpenApiProperty(string name, OpenApiSchema schema)
    {
        Name = name;
        Schema = schema;
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"{Name}: {Schema}";
}
