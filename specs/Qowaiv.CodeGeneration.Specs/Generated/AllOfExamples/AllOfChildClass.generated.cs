// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

#nullable enable

namespace AllOfExamples;

public sealed partial record AllOfChildClass
{
    [System.Text.Json.Serialization.JsonPropertyName("property1")]
    [Qowaiv.Validation.DataAnnotations.Optional]
    public long Property1 { get; init; }

    [System.Text.Json.Serialization.JsonPropertyName("property2")]
    [Qowaiv.Validation.DataAnnotations.Optional]
    public string? Property2 { get; init; }
}