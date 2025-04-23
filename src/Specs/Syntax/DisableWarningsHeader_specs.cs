using Qowaiv.CodeGeneration.Syntax;

namespace Specs.Syntax.DisableWarningsHeader_specs;

public class Generates
{
    [Test]
    public void DisableWarnings()
        => Headers.DisableWarnings.ToString().Should().Be("#pragma warning disable\r\n\r\n");
}
