﻿using Qowaiv.CodeGeneration;
using Qowaiv.CodeGeneration.Syntax;
using System.IO;

namespace Qowaiv.SingleValueObjects;

public sealed class SvoTemplate : Code
{
    private readonly CodeSnippet Snippet;

    /// <summary>Creates new instance of the <see cref="SvoTemplate"/> class.</summary>
    private SvoTemplate(CodeSnippet snippet) => Snippet = snippet;

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
        => Guard.NotNull(writer, nameof(writer))
        .Write(new AutoGeneratedHeader())
        .Write(Snippet);

    /// <inheritdoc />
    [Pure]
    public override string ToString() => this.Stringify();

    [Pure]
    public SvoTemplate Transform(SvoArguments arguments)
    {
        Guard.NotNull(arguments, nameof(arguments));

        return new(Snippet
            .Transform(line => ReplacePlaceholders(line, arguments))
            .Transform(Constants(arguments.Features)));

        static string ReplacePlaceholders(string line, SvoArguments arguments)
            => line
            .Replace("@TSvo", arguments.Name)
            .Replace("@FullName", arguments.FullName)
            .Replace("@Namespace", arguments.Namespace)
            .Replace("@type_n", arguments.Underlying.IsValueType 
                ? arguments.Underlying.ToCSharpString(false) 
                : arguments.Underlying.ToCSharpString(false) + "?")
            .Replace("@type", arguments.Underlying.ToCSharpString(false))
            .Replace("@FormatExceptionMessage", arguments.FormatExceptionMessage);

        static IReadOnlyCollection<Constant> Constants(SvoFeatures features)
        {
            var constants = new List<Constant>
            {
                (features & SvoFeatures.Structure) != default
                ? "Structure"
                : "NotStructure"
            };

            foreach (var flag in Enum.GetValues<SvoFeatures>().Where(f => f != SvoFeatures.Structure))
            {
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

    public static SvoTemplate GeneratedCode { get; } = new SvoTemplate(new[]
    {
        Embedded("Structure"),
        Embedded("IEquatable"),
        Embedded("IComparable"),
        Embedded("IFormattable"),
        Embedded("ISerializable"),
        Embedded("IJsonSerializable"),
        Embedded("IXmlSerializable"),
        Embedded("Parsing"),
        Embedded("Validation"),
        //Embedded("Utf8"),
    }.Concat());

    public static SvoTemplate Initial { get; } = new SvoTemplate(Embedded(nameof(Initial)));
    
    public static SvoTemplate JsonConverter { get; } = new SvoTemplate(Embedded(nameof(JsonConverter)));
    
    public static SvoTemplate Specs { get; } = new SvoTemplate(Embedded(nameof(Specs)));

    private static CodeSnippet Embedded(string name)
    {
        var path = $"Qowaiv.CodeGeneration.SingleValueObjects.Snippets.{name}.cs";

        using var stream = typeof(SvoTemplate).Assembly.GetManifestResourceStream(path);

        if (stream is { })
        {
            using var reader = new StreamReader(stream);
            return new CodeSnippet(reader.ReadToEnd());
        }
        else throw new ArgumentException($"The path '{path}' is not a stream.", nameof(name));
    }
}
