using Qowaiv.CodeGeneration;

namespace Specs.Nill_specs;

public class Singlton
{
    [Test]
    public void Value()
    {
        var nill = Nill.Value;
        nill.Should().BeSameAs(Nill.Value);
    }
}
