using System.IO;

namespace Qowaiv.OpenApi.Generation;

public class CodeGeneratorSettings
{
    public DirectoryInfo RootLocation { get; init; } = new DirectoryInfo(".");
    public bool DeleteExistingGeneratedFiles { get; init; } = false;

    public ModelType ModelType { get; init; } = ModelType.Class;

    public PropertyAccess PropertyAccess { get; init; } = PropertyAccess.InitOnly;

    public Type CollectionType { get; init; } = typeof(Array);
}
