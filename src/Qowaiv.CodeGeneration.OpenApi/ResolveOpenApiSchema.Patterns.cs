using Qowaiv.Text;

namespace Qowaiv.CodeGeneration.OpenApi;

public readonly partial struct ResolveOpenApiSchema
{
    /// <summary>
    /// True if the Open API type is <see cref="OpenApiType.@string"/>.
    /// </summary>
    public bool IsString => OpenApiType == OpenApiType.@string;

    /// <summary>Returns true if the schema path matches the pattern.</summary>
    /// <remarks>
    /// Wildcards are supported.
    /// </remarks>
    [Pure]
    public bool Matches(string pattern, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
    {
        var paths = Path.ToString().Split(OpenApiPath.Splitter);
        var patterns = pattern?.Split(OpenApiPath.Splitter) ?? [];

        if (patterns.Length > 0 && patterns.Length <= paths.Length)
        {
            for (var i = 1; i <= patterns.Length; i++)
            {
                if (!WildcardPattern.IsMatch(patterns[^i], paths[^i], default, comparisonType))
                {
                    return false;
                }
            }
            return true;
        }
        else return false;
    }
}
