using Qowaiv.CodeGeneration.Syntax;

namespace Code_specs;

public class Stringifies
{
    [Test]
    public void Code()
    {
        var str = Headers.NullabilityEnabled.Stringify();
        str.Should().Be("#nullable enable\r\n\r\n");
    }
}
