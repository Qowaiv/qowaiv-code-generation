using FluentAssertions;
using Qowaiv.Customization;


namespace Specs.SVO_Generation;

public class Generates
{
    [Test]
    public void SVO()
    {
        var svo = CustomSvo.Parse("test");
        svo.Length.Should().Be(12);
    }
}

[Svo<MyBehavior>]
public readonly partial struct CustomSvo
{
    private sealed class MyBehavior : SvoBehavior
    {
        public override int MinLength => 3;

        public override int MaxLength => 4;

        public override int Length(string str) => str.Length * 3;
    }
}
