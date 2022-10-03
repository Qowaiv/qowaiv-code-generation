using Qowaiv.CodeGeneration;
using Qowaiv.CodeGeneration.Syntax;
using System.IO;

namespace Qowaiv.SingleValueObjects;

internal sealed partial class SvoTemplate : Code
{
    private readonly CodeSnippet Snippet;

    private SvoTemplate(CodeSnippet snippet) => Snippet = snippet;


    public void WriteTo(CSharpWriter writer)
    {
        throw new NotImplementedException();
    }

    [Pure]
    public CodeSnippet Transform(SvoArguments arguments)
    {

    }

    //public SvoTemplate Transform(SvoArguments arguments)
    //{
    //    var write = true;
    //    var prev = string.Empty;
    //    var sb = new StringBuilder();
    //    foreach (var line in Lines)
    //    {
    //        if (line == "#endif") { write = true; }
    //        else if (line == "#else") { write = !write; }
    //        else if (line.StartsWith("#if !Not"))
    //        {
    //            if (Enum.TryParse<SvoFeatures>(line[8..], out var feature))
    //            {
    //                write = arguments.HasFeature(feature);
    //            }
    //            else throw new FormatException($"Line '{line}' ({line[8..]}) can not be parsed.");
    //        }
    //        else if (line.StartsWith("#if Not"))
    //        {
    //            if (Enum.TryParse<SvoFeatures>(line[7..], out var feature))
    //            {
    //                write = !arguments.HasFeature(feature);
    //            }
    //            else throw new FormatException($"Line '{line}' ({line[8..]}) can not be parsed.");
    //        }
    //        else if (write)
    //        {
    //            if (line != string.Empty || prev != string.Empty)
    //            {
    //                sb.AppendLine(line
    //                    .Replace("@TSvo", arguments.Name)
    //                    .Replace("@FullName", arguments.FullName)
    //                    .Replace("@Namespace", arguments.Namespace)
    //                    //.Replace("@type_n", arguments.Underlying.IsValueType ? arguments.Type : arguments.Type + "?")
    //                    .Replace("@type", arguments.Type.Name)
    //                    .Replace("@FormatExceptionMessage", arguments.FormatExceptionMessage));
    //            }
    //            prev = line;
    //        }
    //    }
    //    return new(sb.ToString());
    //}

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
