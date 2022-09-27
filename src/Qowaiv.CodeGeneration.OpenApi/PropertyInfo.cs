namespace Qowaiv.OpenApi;

public record PropertyInfo
{
    public string? Type { get; init; }
    public string? Format { get; init; }
    public string? Pattern { get; init; }
    public string? Description { get; init; }
}
