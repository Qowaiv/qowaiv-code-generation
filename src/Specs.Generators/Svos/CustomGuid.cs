using Qowaiv.Customization;
using Qowaiv.OpenApi;

namespace Specs;

[OpenApiDataType(
    description: "GUID based identifier",
    example: "8a1a8c42-d2ff-e254-e26e-b6abcbf19420",
    type: "string",
    format: "guid")]
[Id<Behavior, Guid>]
public readonly partial struct CustomGuid
{
    private sealed class Behavior : GuidBehavior
    {
        public override Guid NextId() => Guid.Parse("12345678-1234-1234-1234-123456789abc");
    }
}
