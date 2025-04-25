using Qowaiv;
using Qowaiv.Customization;
using Qowaiv.OpenApi;

namespace Specs;

[OpenApiDataType(
    description: "UUID based identifier",
    example: "Qowaiv_SVOLibrary_GUIA",
    type: "string",
    format: "guid")]
[Id<Behavior, Uuid>]
public readonly partial struct CustomUuid
{
    private sealed class Behavior : UuidBehavior
    {
        public override Uuid NextId() => Uuid.Parse("12345678-1234-1234-1234-123456789abc");
    }
}
