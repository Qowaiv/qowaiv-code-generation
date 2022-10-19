using Qowaiv.CodeGeneration.Syntax;

namespace Syntax.Attribute_decoration_specs;

public class Supports
{
    [Test]
    public void argumentless()
        => new AttributeDecoration(typeof(TestAttribute))
       .Should().HaveContent("[NUnit.Framework.Test]\r\n");

    [Test]
    public void ctor_arguments()
        => new AttributeDecoration(typeof(TestAttribute), new object[] { typeof(int) } )
       .Should().HaveContent("[NUnit.Framework.Test(typeof(int))]\r\n");

    [Test]
    public void parameter_sets()
        => new AttributeDecoration(typeof(TestAttribute),
            null, KeyValuePair.Create("Author", (object)"Qowaiv"))
       .Should().HaveContent("[NUnit.Framework.Test(Author = \"Qowaiv\")]\r\n");
}
