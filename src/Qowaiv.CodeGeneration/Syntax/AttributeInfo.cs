namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents an attribute decoration.</summary>
public sealed partial class AttributeInfo : Code, IEquatable<AttributeInfo>
{
    /// <summary>Initializes a new instance of the <see cref="AttributeInfo"/> class.</summary>
    public AttributeInfo(Type attribute, object[]? ctorArguments = null, params KeyValuePair<string, object?>[] propertyValues)
    {
        AttributeType = Guard.NotNull(attribute);
        CtorArguments = ctorArguments ?? [];
        PropertyValues = propertyValues ?? [];
    }

    /// <summary>The type of the attribute.</summary>
    public Type AttributeType { get; }

    private readonly IReadOnlyCollection<object> CtorArguments;

    private readonly IReadOnlyCollection<KeyValuePair<string, object?>> PropertyValues;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => this.Stringify();

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        Guard.NotNull(writer)
            .Indent()
            .Write('[')
            .Write(AttributeType, attribute: true);

        if (CtorArguments.Any() || PropertyValues.Any())
        {
            writer.Write('(');
            writer.Write(CtorArguments.Select(CtorArgument), writer => writer.Write(", "));

            if (CtorArguments.Any() && PropertyValues.Any()) writer.Write(", ");

            writer.Write(PropertyValues.Select(PropertyValue), writer => writer.Write(", "));
            writer.Write(')');
        }

        writer.Line(']');

        Action<CSharpWriter> CtorArgument(object arg) => (writer) => writer.Literal(arg);

        Action<CSharpWriter> PropertyValue(KeyValuePair<string, object?> assignment) => (writer) => writer.Write($"{assignment.Key} = ").Literal(assignment.Value);
    }

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is AttributeInfo other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(AttributeInfo? other)
        => other is not null
        && AttributeType.Equals(other.AttributeType)
        && Equals(CtorArguments, other.CtorArguments)
        && Equals(PropertyValues, other.PropertyValues);

    [Pure]
    private static bool Equals(IReadOnlyCollection<object> self, IReadOnlyCollection<object> othr)
    {
        if (self.Count != othr.Count) return false;

        using var this_iterator = self.GetEnumerator();
        using var othr_iterator = othr.GetEnumerator();

        while (this_iterator.MoveNext() && othr_iterator.MoveNext())
        {
            if (!Equals(this_iterator.Current, othr_iterator.Current))
            {
                return false;
            }
        }
        return true;
    }

    [Pure]
    private static bool Equals(IReadOnlyCollection<KeyValuePair<string, object?>> self, IReadOnlyCollection<KeyValuePair<string, object?>> othr)
    {
        if (self.Count != othr.Count) return false;

        using var this_iterator = self.GetEnumerator();
        using var othr_iterator = othr.GetEnumerator();

        while (this_iterator.MoveNext() && othr_iterator.MoveNext())
        {
            if (this_iterator.Current.Key != othr_iterator.Current.Key
                || !Equals(this_iterator.Current.Value, othr_iterator.Current.Value))
            {
                return false;
            }
        }
        return true;
    }

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode()
    {
        var hash = AttributeType.GetHashCode();

        foreach (var args in CtorArguments)
        {
            hash = HashCode.Combine(hash, args.GetHashCode());
        }
        foreach (var kvp in PropertyValues)
        {
            hash = HashCode.Combine(hash, kvp.Key, kvp.Value);
        }

        return hash;
    }

    /// <summary>Returns true if both have the same values.</summary>
    public static bool operator ==(AttributeInfo? l, AttributeInfo? r) => l is null ? r is null : l.Equals(r);

    /// <summary>Returns false if both have the same values.</summary>
    public static bool operator !=(AttributeInfo? l, AttributeInfo? r) => !(l == r);
}
