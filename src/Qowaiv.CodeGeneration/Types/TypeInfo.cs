using Qowaiv.CodeGeneration.Syntax;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Qowaiv.CodeGeneration;

/// <summary>A collection of data needed to create a <see cref="TypeBase"/>.</summary>
[Inheritable]
public record TypeInfo
{
    /// <summary>The type name.</summary>
    [Required]
    public TypeName? TypeName { get; init; }

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

    /// <summary>Collection of <see cref="AttributeInfo"/>s.</summary>
    public IReadOnlyCollection<AttributeInfo> Attributes { get; init; } = Array.Empty<AttributeInfo>();

    /// <summary>Collection of <see cref="ConstructorInfo"/>s.</summary>
    public IReadOnlyCollection<ConstructorInfo> Constructors { get; init; } = Array.Empty<ConstructorInfo>();

    /// <summary>Collection of <see cref="EventInfo"/>s.</summary>
    public IReadOnlyCollection<EventInfo> Events { get; init; } = Array.Empty<EventInfo>();

    /// <summary>Collection of <see cref="FieldInfo"/>s.</summary>
    public IReadOnlyCollection<FieldInfo> Fields { get; init; } = Array.Empty<FieldInfo>();

    /// <summary>Collection of interface <see cref="Type"/>s.</summary>
    public IReadOnlyCollection<Type> Interfaces { get; init; } = Array.Empty<Type>();

    /// <summary>Collection of <see cref="MethodInfo"/>s.</summary>
    public IReadOnlyCollection<MethodInfo> Methods { get; init; } = Array.Empty<MethodInfo>();

    /// <summary>Collection of <see cref="PropertyInfo"/>s.</summary>
    public IReadOnlyCollection<PropertyInfo> Properties { get; init; } = Array.Empty<PropertyInfo>();

    /// <summary>Collection of derived <see cref="Type"/>s.</summary>
    public IReadOnlyCollection<Type> DerivedTypes { get; init; } = Array.Empty<Type>();

    /// <summary>The (optional) documentation.</summary>
    public XmlDocumentation? Documentation { get; init; }
}
