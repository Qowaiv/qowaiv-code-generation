using Qowaiv.Diagnostics.Contracts;

namespace Qowaiv.CodeGeneration.OpenApi;

/// <summary>
/// Used to determine how the OpenApi reader should handle certain situations.
/// </summary>
[Mutable]
public sealed class OpenApiReaderOptions
{
    /// <summary>
    /// Determines whether or not not errors will be treated as warnings
    /// (default: <c>false</c>).
    /// </summary>
    public bool IgnoreErrors { get; set; } = false;
}
