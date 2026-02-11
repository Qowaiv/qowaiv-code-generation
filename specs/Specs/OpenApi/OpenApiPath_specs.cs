using Qowaiv.CodeGeneration.OpenApi;

namespace Open_API.OpenApiPath_specs;

public class Opearates
{
    [Test]
    public void Child_extends()
    {
        var path = OpenApiPath.Root.Child("#root/child");
        path.Child("grand-child").Should().Be(OpenApiPath.Root.Child("#root/child/grand-child"));
    }

    [Test]
    public void Parent_drops_last()
    {
        var path = OpenApiPath.Root.Child("#root/child/grand-child");
        path.Parent.Should().Be(OpenApiPath.Root.Child("#root/child"));
    }
}
