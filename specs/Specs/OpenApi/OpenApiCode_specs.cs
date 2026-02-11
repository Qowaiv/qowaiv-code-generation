using Qowaiv.CodeGeneration.OpenApi;
using Qowaiv.CodeGeneration.Syntax;
using System.Reflection;

namespace Specs.OpenApi.OpenApiCode_specs;

public class Does_not_save
{
    [Test]
    public void duplicate_types()
    {
        var @enum = new Enumeration(new()
        {
            TypeName = new("Specs", "SomeType"),
        });
        var @class = new Class(new()
        {
            TypeName = new("Specs", "SomeType"),
        });

        var code = Create([@enum, @class]);

        Action save = () => code.Save(new());

        save.Should()
            .Throw<DuplicateType>()
            .WithMessage("Contains 'Specs.SomeType' multiple times");

    }

    private static OpenApiCode Create(IReadOnlyCollection<Code> code)
        => (OpenApiCode)typeof(OpenApiCode)
            .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0]
            .Invoke([code])!;
}
