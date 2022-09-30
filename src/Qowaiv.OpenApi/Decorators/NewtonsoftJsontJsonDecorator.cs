using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Qowaiv.CodeGeneration;
using Qowaiv.CodeGeneration.Instructions;

namespace Qowaiv.OpenApi.Decorators;

public class NewtonsoftJsontJsonDecorator : CodeDecorator
{
    [Pure]
    public override IEnumerable<Code> Ctor(Class @class, OpenApiSchema schema)
    {
        yield return new Decoration(typeof(JsonConstructorAttribute));
    }

    [Pure]
    public override IEnumerable<Code> Property(Property property, OpenApiProperty schema)
    {
        yield return new Decoration(typeof(JsonPropertyAttribute), new object[] { schema.Name });
    }
}
