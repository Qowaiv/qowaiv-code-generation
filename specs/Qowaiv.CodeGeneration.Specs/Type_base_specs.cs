using Qowaiv.CodeGeneration.Syntax;
using System.Reflection;

namespace Type_base_specs;

public class IsSubclassOf
{
    [Test]
    public void base_type()
    {
        Type @class = new Class(new()
        {
            TypeName = new TypeName("UnitTests.Models", "SomeModel"),
            BaseType = typeof(SomeBaseClass),
        });

        @class.IsSubclassOf(typeof(SomeBaseClass)).Should().BeTrue();
    }
}

public class GetProperties
{
    [Test]
    public void returns_from_self_and_bases()
    {
        Type @class = new Class(new()
        {
            TypeName = new TypeName("UnitTests.Models", "SomeModel"),
            BaseType = typeof(SomeBaseClass),
            Properties = 
            [
                new Property(new()
                {
                    Name = CodeName.Create("Label"),
                    PropertyType = typeof(string),
                    DeclaringType = typeof(SomeChildClass),
                }),
                // This means that exists twice.
                typeof(SomeBaseClass).GetProperties()[0],
            ]
        });

        var props = @class.GetProperties().Select(Prop.New);
        var reference = typeof(SomeChildClass).GetProperties().Select(Prop.New);
        props.Should().BeEquivalentTo(reference);
    }

    private record Prop(string Name, string Type)
    {
        public static Prop New(PropertyInfo p) => new(p.Name, p.PropertyType.ToCSharpString());
    }
}

internal class SomeChildClass : SomeBaseClass
{
    public required string Label { get; init; }
}

internal class SomeBaseClass
{
    public int Value { get; } = 42;
}
