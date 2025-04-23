using Qowaiv.Customization;
using Qowaiv.OpenApi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Specs.SVOs;

public static class Svo
{
    public static readonly CustomSvo CustomSvo = CustomSvo.Parse("QOWAIV");
}


[OpenApiDataType(description: "Custom SVO Example", type: "string", example: "QOWAIV", format: "custom")]
[Svo<Behavior>]
public readonly partial struct CustomSvo
{
    private sealed class Behavior : SvoBehavior
    {
        public override int MinLength => 3;
        public override int MaxLength => 16;
        public override Regex Pattern => new("^[A-Z]+$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(1));

        public override string NormalizeInput(string? str, IFormatProvider? formatProvider)
            => str?.Replace("-", "").ToUpper(formatProvider ?? CultureInfo.InvariantCulture) ?? string.Empty;
    }
}
