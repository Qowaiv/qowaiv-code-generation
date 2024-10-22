using Qowaiv.CodeGeneration.Syntax;

namespace Syntax.Attribute_decoration_specs;

public class Supports
{
    [Test]
    public void argumentless()
        => new AttributeInfo(typeof(TestAttribute))
       .Should().HaveContent("[NUnit.Framework.Test]\r\n");

    [Test]
    public void ctor_arguments()
        => new AttributeInfo(typeof(TestAttribute), new object[] { typeof(int) } )
       .Should().HaveContent("[NUnit.Framework.Test(typeof(int))]\r\n");

    [Test]
    public void parameter_sets()
        => new AttributeInfo(typeof(TestAttribute),
            null, KeyValuePair.Create("Author", (object)"Qowaiv"))
       .Should().HaveContent("[NUnit.Framework.Test(Author = \"Qowaiv\")]\r\n");
}
