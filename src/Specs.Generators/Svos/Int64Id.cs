using Qowaiv.Customization;
using Qowaiv.OpenApi;

namespace Specs;

[OpenApiDataType(
    description: "String based identifier",
    example: "Order-UK-2022-215",
    type: "string",
    format: "long")]
[Id<Behavior, long>]
public readonly partial struct Int64Id
{
    public static Int64Id Create(long value) => new(value);

    private sealed class Behavior : Int64IdBehavior
    {
        public override long NextId() => 42;

        public override string ToString(long value, string? format, IFormatProvider? formatProvider)
            => string.Format(formatProvider, $"PREFIX{{0:{format}}}", value);

        public override bool TryParse(string? str, IFormatProvider? formatProvider, out long id)
            => str is { Length: > 6 } && str[..6] == "PREFIX"
            ? long.TryParse(str[6..], out id)
            : long.TryParse(str, out id);
    }
}
