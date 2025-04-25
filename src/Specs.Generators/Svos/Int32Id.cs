using Qowaiv.Customization;
using Qowaiv.OpenApi;

namespace Specs;

[OpenApiDataType(
    description: "String based identifier",
    example: "Order-UK-2022-215",
    type: "string",
    format: "int")]
[Id<Behavior, int>]
public readonly partial struct Int32Id
{
    public static Int32Id Create(int value) => new(value);

    private sealed class Behavior : Int32IdBehavior
    {
        public override int NextId() => 42;

        public override string ToString(int value, string? format, IFormatProvider? formatProvider)
            => string.Format(formatProvider, $"PREFIX{{0:{format}}}", value);

        public override bool TryParse(string? str, IFormatProvider? formatProvider, out int id)
            => str is { Length: > 6 } && str[..6] == "PREFIX"
            ? int.TryParse(str[6..], out id)
            : int.TryParse(str, out id);
    }
}
