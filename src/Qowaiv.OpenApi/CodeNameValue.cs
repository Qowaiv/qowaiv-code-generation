using Microsoft.OpenApi.Any;

namespace Qowaiv.OpenApi;

public sealed record CodeNameValue(string Name, string Value, IOpenApiAny Schema);
