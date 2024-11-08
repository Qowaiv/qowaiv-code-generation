using Qowaiv.Identifiers;
using Qowaiv.OpenApi;

namespace Qowaiv.CodeGeneration.OpenApi;

[OpenApiDataType(
    description: "Reference ID in an Open API schema",
    type: "string",
    example: "")]
public sealed class ForReference : StringIdBehavior { }
