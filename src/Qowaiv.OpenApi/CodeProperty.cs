namespace Qowaiv.OpenApi;

[DebuggerDisplay("{Type} {Name}")]
public sealed class CodeProperty
{
    public CodeProperty(CodeType type, string name, OpenApiNamedSchema openApi)
    {
        Type = type;
        Name = name;
        Schema = openApi;
    }

    public CodeType Type { get; }
    public CodeModel Model { get; internal set; }
    public string Name { get; }
    public string JsonName => Schema.Name;
    public OpenApiNamedSchema Schema { get; }
}
