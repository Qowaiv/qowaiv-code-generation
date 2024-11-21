using Qowaiv.CodeGeneration.Syntax;
using Qowaiv.Diagnostics.Contracts;

namespace Qowaiv.CodeGeneration.OpenApi;

/// <summary>Methods to extend a <see cref="Class"/> definition.</summary>
public static class ClassExtender
{
    /// <summary>Overrides the All-of collection to a single base that the class has to obey.</summary>
    [FluentSyntax]
    public static Class WithBase(this Class @class, Type @base, ResolveOpenApiSchema schema)
    {
        Guard.NotNull(@base);
        Guard.NotNull(schema);

        var data = schema.Context.GetEntry(schema)!.Data!;
        data.Manual.BaseType = @base;

        return @class;
    }
}
