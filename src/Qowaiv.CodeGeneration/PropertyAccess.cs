using Qowaiv.CodeGeneration.IO;

namespace Qowaiv.CodeGeneration;

public enum PropertyAccess
{
    /// <summary>{ get; init; }</summary>
    InitOnly = 0,

    /// <summary>{ get; set; }</summary>
    PublicSet,

    /// <summary>{ get; protected set; }</summary>
    ProtectedSet,

    /// <summary>{ get; internal set; }</summary>
    InternalSet,

    /// <summary>{ get; }</summary>
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
            => PropertyAccess = Guard.DefinedEnum(propertyAccess, nameof(propertyAccess));

        public void Write(CSharpWriter writer)
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

