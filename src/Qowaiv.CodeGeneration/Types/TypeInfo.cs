using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Qowaiv.CodeGeneration;

[Inheritable]
public record TypeInfo
{
    [Required]
    public TypeName? TypeName { get; init; }
    public Type? BaseType { get; init; } = typeof(object);
    public bool IsStatic { get; init; }
    public bool IsAbstract { get; init; }
    public bool IsSealed { get; init; }
    public bool IsPartial { get; init; }
    public IReadOnlyCollection<AttributeInfo> Attributes { get; init; } = Array.Empty<AttributeInfo>();
    public IReadOnlyCollection<ConstructorInfo> Constructors { get; init; } = Array.Empty<ConstructorInfo>();
    public IReadOnlyCollection<EventInfo> Events { get; init; } = Array.Empty<EventInfo>();
    public IReadOnlyCollection<FieldInfo> Fields { get; init; } = Array.Empty<FieldInfo>();
    public IReadOnlyCollection<Type> Interfaces { get; init; } = Array.Empty<Type>();
    public IReadOnlyCollection<MethodInfo> Methods { get; init; } = Array.Empty<MethodInfo>();
    public IReadOnlyCollection<PropertyInfo> Properties { get; init; } = Array.Empty<PropertyInfo>();
    public IReadOnlyCollection<Type> DerivedTypes { get; init; } = Array.Empty<Type>();
}
