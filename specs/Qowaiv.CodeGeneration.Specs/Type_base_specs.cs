using Qowaiv.CodeGeneration.Syntax;

namespace Type_base_specs;

public class Is_subclass_of
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

internal class SomeBaseClass
{
    public int Value { get; } = 42;
}
