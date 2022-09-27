namespace Qowaiv.OpenApi;

public sealed record Property(
    DotNetType Type,
    string Name,
    string JsonName,
    PropertyInfo? Info)
{
    public override string ToString() => $"{Name}: {Type}";
}
