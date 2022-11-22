using System.Reflection;

namespace Reflection_specs;

public class Property
{
    [Test]
    public void Is_nullable_when_marked()
        => typeof(Model).GetProperty(nameof(Model.Nullable))!.IsNullable().Should().BeTrue();

    [Test]
    public void Is_not_nullable_when_not_marked()
        => typeof(Model).GetProperty(nameof(Model.NotNullable))!.IsNullable().Should().BeFalse();

    class Model
    {
        public object? Nullable { get; }

        public object NotNullable { get; } = new();
    }
}
