namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Extensions on <see cref="PropertyAccess"/>.</summary>
public static class PropertyAccessExtensions
{
    /// <summary>Represent the property access as accessor code syntax.</summary>
    [Pure]
    public static Code Code(this PropertyAccess propertyAccess) => new Snippet(propertyAccess);

    private sealed class Snippet(PropertyAccess propertyAccess) : Code
    {
        private readonly PropertyAccess PropertyAccess = Guard.DefinedEnum(propertyAccess);

        /// <inheritdoc />
        public void WriteTo(CSharpWriter writer) => writer.Write(PropertyAccess switch
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
