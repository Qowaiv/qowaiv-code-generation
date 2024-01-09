using Qowaiv.CodeGeneration.OpenApi;
using Qowaiv.CodeGeneration.Syntax;
using Specs.TestTools;

namespace Open_API.Resolve_specs;

public class Some
{
    [Test]
    public void X()
    {
        var resolver = new OpenApiTypeResolver("PetShopBoys");
        //var types = resolver.Resolve(new System.IO.FileInfo("./OpenApi/Definitions/pet-shop.json"), out var diagnostic).ToArray();
        //diagnostic.Errors.Should().BeEmpty();
        //types.Should().NotBeEmpty();

        //foreach(var code in types.OfType<Code>())
        //{
        //    Console.WriteLine(TestWriter.Write(code));
        //}
    }
}
