using Qowaiv.CodeGeneration.OpenApi;
using Qowaiv.Validation.TestTools;
using System.IO;

namespace Open_API.Schema_matching_specs;

public class Matches
{
    private static readonly FileInfo PetShop_json = new("./OpenApi/Definitions/pet-shop.json");

    [Test]
    public void on_IsString()
    {
        var matcher = new MatchingResolver(s => s.IsString);
        OpenApiCode.Resolve(PetShop_json, matcher).Should().BeValid();

        matcher.Paths[0].Should().Be(new OpenApiPath("#/components/schemas/Order/shipDate"));
    }

    [TestCase("SHIPDATE", StringComparison.OrdinalIgnoreCase)]
    [TestCase("shipDate", StringComparison.Ordinal)]
    [TestCase("schemas/*/shipDate", StringComparison.Ordinal)]
    [TestCase("schemas/*/shipD?te", StringComparison.Ordinal)]
    public void on_Matching(string pattern, StringComparison comparisonType)
    {
        var matcher = new MatchingResolver(s => s.Matches(pattern, comparisonType));
        OpenApiCode.Resolve(PetShop_json, matcher).Should().BeValid();

        matcher.Paths.Single().Should().Be(new OpenApiPath("#/components/schemas/Order/shipDate"));
    }

    private sealed class MatchingResolver(Predicate<ResolveOpenApiSchema> matcher) : OpenApiTypeResolver("Matching.Schemas")
    {
        private readonly Predicate<ResolveOpenApiSchema> Matcher = matcher;

        public List<OpenApiPath> Paths { get; } = [];

        protected override Type? ResolveCustomization(ResolveOpenApiSchema schema)
        {
            if (Matcher(schema))
            {
                Paths.Add(schema.Path);
            }
            return null;
        }

    }
}
