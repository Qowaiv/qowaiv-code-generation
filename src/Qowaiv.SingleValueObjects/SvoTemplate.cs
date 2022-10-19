using Qowaiv.CodeGeneration;
using Qowaiv.CodeGeneration.Syntax;
using System.IO;

namespace Qowaiv.SingleValueObjects;

internal sealed partial class SvoTemplate
{
    private readonly CodeSnippet Snippet;

    private SvoTemplate(CodeSnippet snippet) => Snippet = snippet;

    [Pure]
    public CodeSnippet Transform(SvoArguments arguments)
    {
        return Snippet
            .Transform(line => ReplacePlaceholders(line, arguments))
            .Transform(Constants(arguments.Features));

        static string ReplacePlaceholders(string line, SvoArguments arguments)
            => line
            .Replace("@TSvo", arguments.Name)
            .Replace("@FullName", arguments.FullName)
            .Replace("@Namespace", arguments.Namespace)
            //.Replace("@type_n", arguments.Underlying.IsValueType ? arguments.Type : arguments.Type + "?")
            .Replace("@type", arguments.Type.Name)
            .Replace("@FormatExceptionMessage", arguments.FormatExceptionMessage);
    
    
        static IReadOnlyCollection<Constant> Constants(SvoFeatures features)
        {
            var constants = new List<Constant>();
            for (var i = 1; i < (int)SvoFeatures.All; i *= 2)
            {
                var flag = (SvoFeatures)i;
                if (features.HasFlag(flag))
                {
                    constants.Add(flag.ToString());
                }
                else
                {
                    constants.Add($"Not{flag}");
                }
            }
            return constants;
        }
    }

    public static SvoTemplate Embedded(string name)
    {
        var path = $"Qowaiv.SingleValueObjects.Snippets.{name}.cs";

        using var stream = typeof(SvoTemplate).Assembly.GetManifestResourceStream(path);

        if (stream is { })
        {
            using var reader = new StreamReader(stream);
            return new(new CodeSnippet(reader.ReadToEnd()));
        }
        else throw new ArgumentException($"The path '{path}' is not a stream.", nameof(name));
    }
}
