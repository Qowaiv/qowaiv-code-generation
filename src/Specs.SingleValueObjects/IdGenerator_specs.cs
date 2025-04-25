using Qowaiv.CodeGeneration.SingleValueObjects;

namespace Specs.Svos.IdGenerator_specs;

public class Can_be
{
    [Test]
    public void Initialized()
    {
        var sut = new IdTemplate(new IdParameters
        {
            Behavior = "MyBehavior",
            Namespace = "Specs.Example",
            Svo = "MyId",
            Value = "Guid",
        });
        
        sut.Invoking(m => m.ToString()).Should().NotThrow();
    }
}
