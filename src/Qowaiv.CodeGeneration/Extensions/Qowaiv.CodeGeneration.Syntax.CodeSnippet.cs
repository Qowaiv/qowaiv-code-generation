namespace Qowaiv.CodeGeneration.Syntax;

public static class CodeSnippetExtensions
{
    /// <summary>Concatenates multiple code snippets.</summary>
    [Pure]
    public static CodeSnippet Concat(this IEnumerable<CodeSnippet> snippets)
        => new(snippets.SelectMany(s => s.Lines).ToArray());
}
