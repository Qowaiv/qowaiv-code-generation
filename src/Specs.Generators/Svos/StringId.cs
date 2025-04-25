using Qowaiv.Customization;
using Qowaiv.OpenApi;

namespace Specs;

[OpenApiDataType(
    description: "String based identifier",
    example: "Order-UK-2022-215",
    type: "string",
    format: "identifier")]
[Id<Behavior, string>]
public readonly partial struct StringId
{
    private sealed class Behavior : StringIdBehavior
    {
        public override string NextId() => "Next-ID";
    }
}
