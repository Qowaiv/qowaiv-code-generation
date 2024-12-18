using System.Reflection;

namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>File headers.</summary>
public static class Headers
{
    /// <summary>
    /// Provides a header to indicate that the code in the file has been generated.
    /// </summary>
    /// <param name="toolchain">
    /// The tools used to generate the code.
    /// </param>
    /// <remarks>
    /// Every involved assembly is added to the header with its version. If
    /// no this not preferred, an empty collection should be passed as an argument.
    /// </remarks>
    [Pure]
    public static AutoGeneratedHeader AutoGenerated(IEnumerable<Assembly>? toolchain = null) => new(toolchain);

    /// <summary>
    /// Provides a header to indicate that nullability has been enabled.
    /// </summary>
    public static readonly NullableHeader NullabilityEnabled = new();

    /// <summary>
    /// Provides a header to indicate that #pragma warnings have been disabled.
    /// </summary>
    public static readonly DisableWarningsHeader DisableWarnings = new();
}
