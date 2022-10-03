namespace Qowaiv.SingleValueObjects;

public class SvoArguments
{
    public SvoFeatures Features { get; init; } = SvoFeatures.Default;
    public Type Underlying { get; init; } = typeof(string);
    public Type Type { get; init; } = typeof(object);
    public string? Name { get; init; } 
    public string? FullName { get; init; }
    public string Namespace { get; init; } = "Qowaiv";
    public string? FormatExceptionMessage { get; init; }

    public bool HasFeature(SvoFeatures feature) => Features.HasFlag(feature);
    public bool LacksFeature(SvoFeatures feature) => !HasFeature(feature);
}
