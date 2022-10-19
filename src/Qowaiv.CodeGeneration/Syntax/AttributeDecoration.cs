namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents an attribute decoration.</summary>
public sealed class AttributeDecoration : Code
{
    /// <summary>Creates a new instance of the <see cref="AttributeDecoration"/> class.</summary>
    public AttributeDecoration(Type attribute, object[]? ctorArguments = null, params KeyValuePair<string, object>[] propertyValues)
    {
        Attribute = Guard.NotNull(attribute, nameof(attribute));
        CtorArguments = ctorArguments ?? Array.Empty<object>();
        PropertyValues = propertyValues ?? Array.Empty<KeyValuePair<string, object>>();
    }

    private readonly Type Attribute;

    private readonly IReadOnlyCollection<object> CtorArguments;

    private readonly IReadOnlyCollection<KeyValuePair<string, object>> PropertyValues;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => this.Stringify();

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        writer
            .Indent()
            .Write('[')
            .Write(Attribute, attribute: true);

        if (CtorArguments.Any() || PropertyValues.Any())
        {
            writer.Write('(');
            writer.Write(CtorArguments.Select(a => CtorArgument(a)), writer => writer.Write(", "));

            if (CtorArguments.Any() && PropertyValues.Any()) writer.Write(", ");

            writer.Write(PropertyValues.Select(a => PropertyValue(a)), writer => writer.Write(", "));
            writer.Write(')');
        }

        writer.Line(']');

        Action<CSharpWriter> CtorArgument(object arg) => (writer) => writer.Literal(arg);

        Action<CSharpWriter> PropertyValue(KeyValuePair<string, object> assignment) => (writer) => writer.Write($"{assignment.Key} = ").Literal(assignment.Value);
    }
}
