using Qowaiv.CodeGeneration.Syntax;
using System.Reflection;

namespace Qowaiv.CodeGeneration;

/// <summary>A collection of data needed to create a <see cref="TypeBase"/>.</summary>
[Inheritable]
public record TypeInfo
{
    /// <summary>The type name.</summary>
    public required TypeName TypeName { get; init; }

    /// <summary>The optional base type.</summary>
    public Type? BaseType { get; init; } = typeof(object);

    /// <summary>True if the type is static.</summary>
    public bool IsStatic { get; init; }

    /// <summary>True if the type is abstract.</summary>
    public bool IsAbstract { get; init; }

    /// <summary>True if the type is sealed.</summary>
    public bool IsSealed { get; init; }

    /// <summary>True if the type is partial.</summary>
    public bool IsPartial { get; init; }

    /// <summary>The viability of the type.</summary>
    public CodeVisibility Visibility { get; init; } = CodeVisibility.Public;

    /// <summary>Collection of <see cref="AttributeInfo"/>s.</summary>
    public IReadOnlyCollection<AttributeInfo> Attributes { get; init; } = [];

    /// <summary>Collection of <see cref="ConstructorInfo"/>s.</summary>
    public IReadOnlyCollection<ConstructorInfo> Constructors { get; init; } = [];

    /// <summary>Collection of <see cref="EventInfo"/>s.</summary>
    public IReadOnlyCollection<EventInfo> Events { get; init; } = [];

    /// <summary>Collection of <see cref="FieldInfo"/>s.</summary>
    public IReadOnlyCollection<FieldInfo> Fields { get; init; } = [];

    /// <summary>Collection of interface <see cref="Type"/>s.</summary>
    public IReadOnlyCollection<Type> Interfaces { get; init; } = [];

    /// <summary>Collection of <see cref="MethodInfo"/>s.</summary>
    public IReadOnlyCollection<MethodInfo> Methods { get; init; } = [];

    /// <summary>Collection of <see cref="PropertyInfo"/>s.</summary>
    public IReadOnlyCollection<PropertyInfo> Properties { get; init; } = [];

    /// <summary>Collection of derived <see cref="Type"/>s.</summary>
    public IReadOnlyCollection<Type> DerivedTypes { get; init; } = [];

    /// <summary>The (optional) documentation.</summary>
    public XmlDocumentation? Documentation { get; init; }

    /// <summary>The type attributes.</summary>
    public TypeAttributes TypeAttributes
    {
        get
        {
            TypeAttributes attributes = default;
            attributes |= Visibility == CodeVisibility.Public ? TypeAttributes.Public : default;
            attributes |= IsSealed && DerivedTypes.Count == 0 ? TypeAttributes.Sealed : default;
            attributes |= IsAbstract ? TypeAttributes.Abstract : default;
            attributes |= IsStatic ? (TypeAttributes.Sealed | TypeAttributes.Abstract) : default;
            return attributes;
        }
    }
}
