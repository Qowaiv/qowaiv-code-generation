using DotNetProjectFile.MsBuild.CSharp;
using DotNetProjectFile.MsBuild;
using Microsoft.VisualBasic;
using Qowaiv.Customization;
using Qowaiv.OpenApi;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Specs;

public static class Svo
{
    public static readonly CustomGuid CustomGuid = CustomGuid.Parse("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");

    public static readonly CustomSvo CustomSvo = CustomSvo.Parse("QOWAIV");

    public static readonly StringId StringId = StringId.Parse("Qowaiv-ID");
}

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

[OpenApiDataType(
    description: "Custom SVO Example",
    type: "string",
    example: "QOWAIV",
    format: "custom")]
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

        public override string InvalidFormatMessage(string? str, IFormatProvider? formatProvider)
            => "Is not a valid CustomSvo";
    }
}

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
