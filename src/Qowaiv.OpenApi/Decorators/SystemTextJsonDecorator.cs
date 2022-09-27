using Qowaiv.CodeGeneration;
using Qowaiv.CodeGeneration.IO;
using System.Text.Json.Serialization;
using Qowaiv.CodeGeneration.Instructions;
using Microsoft.OpenApi.Models;

namespace Qowaiv.OpenApi.Decorators;

public class SystemTextJsonDecorator : CodeDecorator
{
    [Pure]
    public override IEnumerable<Code> Ctor(Class @class, OpenApiSchema schema)
    {
        yield return new Decoration(typeof(JsonConstructorAttribute));
    }

    [Pure]
    public override IEnumerable<Code> Property(Property property, OpenApiProperty schema)
    {
        yield return new Decoration(typeof(JsonPropertyNameAttribute), new object[] { schema.Name });
    }
}
