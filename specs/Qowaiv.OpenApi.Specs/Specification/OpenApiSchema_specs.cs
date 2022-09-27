using FluentAssertions;
using NUnit.Framework;
using Qowaiv.CodeGeneration;
using Qowaiv.OpenApi;
using System.IO;
using System.Linq;

namespace Specification_Open_API_Schema;

public class Serialization
{
    [Test]
    public void API_Overzicht()
    {
        var root = new DirectoryInfo(@"C:\TEMP\Tjip.Hypotheekservices.Api\Overzicht");
        var globals = new FileInfo(Path.Combine(root.FullName, @"..\Properties\GlobalUsings.cs"));

        using var stream = new FileStream(
            @"C:\_TJIP\abn-hypotheek\services\hypotheekdossier-api\extern\Tjip.Hypotheekservices.Api\Overzicht\TjipOverzicht.swagger.json", FileMode.Open);

        var reader = new Microsoft.OpenApi.Readers.OpenApiStreamReader();
        var openApiDocument = reader.Read(stream, out var diagnostic);

        var resolver = new OpenApiTypeResolver("Tjip.Hypotheek.Overzicht");
        var types = resolver.Resolve(openApiDocument).ToArray();

        var enums = types.OfType<Enumeration>().ToArray();

        types.Should().HaveCount(1000);

        //var collect = Collector.Empty(new TjipHypotheekservicesResolver())
        //    .WithPropertyNaming(NamingStrategy.PascalCase)
        //    .Collect(stream);

        //collect.Write(new CodeGeneratorSettings
        //{
        //    RootLocation = root,
        //    PropertyAccess = PropertyAccess.InitOnly,
        //    ModelType = ModelType.Class,
        //    DeleteExistingGeneratedFiles = true,
        //},
        //new CSharpWriterSettings
        //{
        //    Globals = Namespace.Globals(globals).ToArray()
        //},
        //new DocumentationDecorator(),
        //new DataAnnotationDecorator(),
        ////new SystemTextJsonDecorator(),
        //new NewtonsoftJsontJsonDecorator()
        //);

        //collect.Should().NotBeEmpty();
    }

    //[Test]
    //public void can_be_loaded_from_JSON()
    //{
    //    using var stream = Embedded.Stream("Files.definitions.json");
    //    var reader = new Microsoft.OpenApi.Readers.OpenApiStreamReader();
    //    var loaded = reader.Read(stream, out var diagnostic);
    //    loaded.Should().NotBeNull();

    //    var collect = Collector.Empty(new TypeResolver("Generated"))
    //        .WithPropertyNaming(NamingStrategy.PascalCase)
    //        .Collect(loaded.Components.NamedSchemas());

    //    var root = new DirectoryInfo(@"c:\TEMP\OpenApi");
    //    var globals = new FileInfo(Path.Combine(root.FullName, @"GlobalUsings.cs"));

    //    collect.Write(new CodeGeneratorSettings
    //    {
    //        DeleteExistingGeneratedFiles = true,
    //        RootLocation = root,
    //        PropertyAccess = PropertyAccess.GetOnly,
    //        ModelType = ModelType.Class,
    //    },
    //    new CSharpWriterSettings
    //    {
    //        Globals = Namespace.Globals(globals).ToArray()
    //    });

    //    collect.Should().NotBeEmpty();
    //}

    //[Test]
    //public void can_generate_Achmea_Gateway()
    //{
    //    using var stream = Embedded.Stream("Files.other.json");
    //    var reader = new Microsoft.OpenApi.Readers.OpenApiStreamReader();

    //    var collect = Collector.Empty(new TypeResolver("Achmea.Gateway"))
    //        .WithPropertyNaming(NamingStrategy.PascalCase)
    //        .Collect(reader.Read(stream, out var diagnostic).Components.NamedSchemas());

    //    var root = new DirectoryInfo(@"c:\TEMP\Achmea.Gateway\Models");
    //    var globals = new FileInfo(Path.Combine(root.FullName, @"..\Properties\GlobalUsings.cs"));

    //    collect.Write(new CodeGeneratorSettings
    //    {
    //        DeleteExistingGeneratedFiles = true,
    //        RootLocation = root,
    //        PropertyAccess = PropertyAccess.GetOnly,
    //        ModelType = ModelType.Class,
    //    },
    //    new CSharpWriterSettings
    //    {
    //        Globals = Namespace.Globals(globals).ToArray()
    //    });

    //    collect.Should().NotBeEmpty();
    //}
    
    //[Test]
    //public void can_be_loaded_from_YAML()
    //{
    //    var root = new DirectoryInfo(@"C:\TEMP\Tjip.Hexon.Adapter.Stater\Models");
    //    var globals = new FileInfo(Path.Combine(root.FullName, @"..\Properties\GlobalUsings.cs"));

    //    using var stream = new FileStream(
    //        @"C:\_TJIP\hypotheek-e-aanvraag\src\apps\Tjip.Hexon.Adapter.Stater\OpenApi\oas-experience-eapplication_v0.10.0_08-09-2022.fixed.yaml", 
    //        new FileStreamOptions
    //        {
    //            Access = FileAccess.Read,
    //            Mode = FileMode.Open,
    //        });

    //    var reader = new Microsoft.OpenApi.Readers.OpenApiStreamReader();
    //    var loaded = reader.Read(stream, out var diagnostic);
    //    loaded.Should().NotBeNull();

    //    var collect = Collector.Empty(new StaterTypeResolver())
    //        .WithPropertyNaming(NamingStrategy.PascalCase)
    //        .Collect(loaded.Components.NamedSchemas());

    //    collect.Write(new CodeGeneratorSettings
    //    {
    //        RootLocation = root,
    //        PropertyAccess = PropertyAccess.InitOnly,
    //        ModelType = ModelType.Class,
    //        DeleteExistingGeneratedFiles = true,
    //    },
    //    new CSharpWriterSettings
    //    {
    //        Globals = Namespace.Globals(globals).ToArray()
    //    });

    //    collect.Should().NotBeEmpty();
    //}

    //private sealed class TjipHypotheekservicesResolver: TypeResolver
    //{
    //    public TjipHypotheekservicesResolver() : base("Tjip.Hypotheekservices.Api.Overzicht") { }

    //    public override CodeType? Custom(OpenApiNamedSchema schema)
    //    {
    //        if (schema.Type == OpenApiType.number && schema.Format == "double")
    //        {
    //            return CodeType.Amount;
    //        }
    //        else if (schema.Type == OpenApiType.@string && schema.Format == "date-time")
    //        {
    //            return CodeType.DateOnly;
    //        }
    //        if(schema.Name.EndsWith("Percentage", System.StringComparison.OrdinalIgnoreCase))
    //        {
    //            return CodeType.Percentage;
    //        }
    //        var name = schema.Name.ToUpperInvariant();

    //        return name switch
    //        {
    //            "JAAR" => CodeType.Year,
    //            "MAAND" => CodeType.Month,
    //            _ => null,
    //        };
    //    }
    //}

    //private sealed class StaterTypeResolver : TypeResolver
    //{
    //    public StaterTypeResolver() : base("Tjip.Hexon.Adapter.Stater") { }

    //    public override CodeType? Custom(OpenApiNamedSchema schema)
    //    {
    //        if ("EMAILADDRESS".Equals(schema.Name, System.StringComparison.InvariantCultureIgnoreCase))
    //        {
    //            return CodeType.EmailAddress;
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }
    //}
}
