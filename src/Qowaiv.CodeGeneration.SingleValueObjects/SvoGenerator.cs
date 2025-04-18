using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Threading;

namespace Qowaiv.CodeGeneration.SingleValueObjects;

[Generator]
internal sealed class SvoGenerator : IIncrementalGenerator
{
    private const string MetadataName = "Qowaiv.Customization.SvoAttribute`1";

    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var symbols = context.SyntaxProvider.ForAttributeWithMetadataName(MetadataName, Filter, Collect).Collect();
        context.RegisterSourceOutput(symbols, Generate);
    }

    /// <summary>Collects the SVO parameters.</summary>
    private SvoParameters Collect(GeneratorAttributeSyntaxContext context, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var symbol = (INamedTypeSymbol)context.TargetSymbol;
        var attr = context.Attributes.First(m => $"{m.AttributeClass?.ContainingNamespace}.{m.AttributeClass?.MetadataName}" == MetadataName).AttributeClass!;

        return new()
        {
            Svo = symbol.Name,
            Behavior = FullName(attr.TypeArguments[0]),
            Namespace = symbol.ContainingNamespace.ToString(),
        };
    }

    /// <inheritdoc />
    /// <remarks>
    /// Currently, all syntax nodes are accepted.
    /// </remarks>
    private static bool Filter(SyntaxNode node, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        return true;
    }

    /// <summary>Generates the source code for the SVO.</summary>
    private static void Generate(SourceProductionContext context, ImmutableArray<SvoParameters> parameters)
    {
        foreach (var pars in parameters)
        {
            var template = new SvoTemplate(pars);
            context.AddSource($"{pars.Namespace}.{pars.Svo}.g.cs", template.ToString());
        }
    }

    /// <summary>Gets the full name of <see cref="ITypeSymbol"/>.</summary>
    private static string FullName(ITypeSymbol symbol)
        => symbol.ContainingType is ITypeSymbol containing
        ? $"{FullName(containing)}.{symbol.Name}"
        : $"{symbol.ContainingNamespace}.{symbol.Name}";
}
