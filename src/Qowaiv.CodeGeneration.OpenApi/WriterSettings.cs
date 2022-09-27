namespace Qowaiv.OpenApi;

public record WriterSettings
{
    public DirectoryInfo RootLocation { get; init; } = new DirectoryInfo(".");

    public ModelType ModelType { get; init; } = ModelType.Class;

    public PropertyAccess PropertyAccess { get; init; } = PropertyAccess.InitOnly;

    public SerializerType SerializerType { get; init; } = SerializerType.Newtonsoft;

    public IReadOnlyCollection<Namespace> Globals { get; init; } = Array.Empty<Namespace>();

    public bool DeleteExistingGeneratedFiles { get; init; } = false;

    public string NewLine { get; init; } = "\r\n";
}
