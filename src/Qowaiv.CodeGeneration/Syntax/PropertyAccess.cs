namespace Qowaiv.CodeGeneration.Syntax;

public enum PropertyAccess
{
    /// <summary>Init only `{ get; init; }`.</summary>
    InitOnly = 0,

    /// <summary>Public set `{ get; set; }`.</summary>
    PublicSet,

    /// <summary>Protected set `{ get; protected set; }`.</summary>
    ProtectedSet,

    /// <summary>Internal set `{ get; internal set; }`.</summary>
    InternalSet,

    /// <summary>Get only `{ get; }`.</summary>
    GetOnly,
}

public static class PropertyAccessExtensions
{
    [Pure]
    public static Code Code(this PropertyAccess propertyAccess) => new Snippet(propertyAccess);

    private sealed class Snippet : Code
    {
        private readonly PropertyAccess PropertyAccess;

        public Snippet(PropertyAccess propertyAccess)
            => PropertyAccess = Guard.DefinedEnum(propertyAccess);

        public void WriteTo(CSharpWriter writer)
            => writer.Write(PropertyAccess switch
            {
                PropertyAccess.InitOnly => "{ get; init; }",
                PropertyAccess.PublicSet => "{ get; set; }",
                PropertyAccess.ProtectedSet => "{ get; protected set; }",
                PropertyAccess.InternalSet => "{ get; internal set; }",
                PropertyAccess.GetOnly => "{ get; }",
                _ => string.Empty,
            });
    }
}
