using Qowaiv.CodeGeneration.Syntax;
using System.Reflection;

namespace Qowaiv.CodeGeneration;

/// <summary>Represents a base for types (classes, records, enums, Array).</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
public abstract class TypeBase : Type
{
    /// <summary>Collection of <see cref="AttributeInfo"/>s.</summary>
    protected readonly IReadOnlyCollection<AttributeInfo> AttributeInfos;

    /// <summary>Collection of <see cref="ConstructorInfo"/>s.</summary>
    protected readonly IReadOnlyCollection<ConstructorInfo> Constructors;

    /// <summary>Collection of <see cref="EventInfo"/>s.</summary>
    protected readonly IReadOnlyCollection<EventInfo> Events;

    /// <summary>Collection of <see cref="FieldInfo"/>s.</summary>
    protected readonly IReadOnlyCollection<FieldInfo> Fields;

    /// <summary>Collection of <see cref="MethodInfo"/>s.</summary>
    protected readonly IReadOnlyCollection<MethodInfo> Methods;

    /// <summary>Collection of <see cref="PropertyInfo"/>s.</summary>
    protected readonly IReadOnlyCollection<PropertyInfo> Properties;

    /// <summary>Collection of interface <see cref="Type"/>s.</summary>
    protected readonly IReadOnlyCollection<Type> Interfaces;

    /// <summary>The (optional) documentation.</summary>
    protected readonly XmlDocumentation? Documentation;

    private readonly IReadOnlyCollection<Type> DerivedTypes;

    private readonly TypeAttributes TypeAttributes = TypeAttributes.Public;

    /// <summary>Initializes a new instance of the <see cref="TypeBase"/> class.</summary>
    protected TypeBase(TypeInfo info)
    {
        Guard.NotNull(info);
        TypeName = Guard.NotNull(info.TypeName);
        BaseType = info.BaseType ?? typeof(object);
        AttributeInfos = info.Attributes ?? Array.Empty<AttributeInfo>();
        Constructors = info.Constructors ?? Array.Empty<ConstructorInfo>();
        Events = info.Events ?? Array.Empty<EventInfo>();
        Fields = info.Fields ?? Array.Empty<FieldInfo>();
        Methods = info.Methods ?? Array.Empty<MethodInfo>();
        Properties = info.Properties ?? Array.Empty<PropertyInfo>();
        Interfaces = info.Interfaces ?? Array.Empty<Type>();
        DerivedTypes = info.DerivedTypes ?? Array.Empty<Type>();
        Documentation = info.Documentation;
        Visibility = info.Visibility;

        IsPartial = info.IsPartial;
        TypeAttributes |= info.IsSealed ? TypeAttributes.Sealed : default;
        TypeAttributes |= info.IsAbstract ? TypeAttributes.Abstract : default;
        TypeAttributes |= info.IsStatic ? (TypeAttributes.Sealed | TypeAttributes.Abstract) : default;
    }

    /// <inheritdoc />
    public override Assembly Assembly => Assembly.GetCallingAssembly();

    /// <inheritdoc />
    public override string AssemblyQualifiedName => Assembly.GetName().FullName;

    /// <inheritdoc />
    public override Type BaseType { get; }

    /// <inheritdoc />
    public override string FullName => $"{Namespace}.{Name}";

    /// <inheritdoc />
    public override Guid GUID => Uuid.GenerateWithSHA1(Encoding.ASCII.GetBytes(FullName));

    /// <summary>Returns if the type sources are spread over multiple files.</summary>
    public bool IsPartial { get; }

    /// <inheritdoc />
    public override Module Module => Assembly.Modules.First();

    /// <inheritdoc />
    public override string Name => TypeName.Name;

    /// <summary>The namespace of the type.</summary>
    public Namespace NameSpace => TypeName.Namespace;

    /// <summary>The namespace of the type.</summary>
    /// <remarks>
    /// THis method exists to prevent unintended usage of the extension method
    /// on <see cref="System.Type"/>.
    /// </remarks>
#pragma warning disable S1133 // Deprecated code should be removed
    [Obsolete("Use NameSpace instead.")]
#pragma warning restore S1133 // Deprecated code should be removed
    [Pure]
    public Namespace NS() => NameSpace;

    /// <inheritdoc />
    public override string Namespace => NameSpace.ToString();

    /// <summary>Gets the type name (name space and name) of the type.</summary>
    public TypeName TypeName { get; }

    /// <inheritdoc />
    public override Type UnderlyingSystemType => this;

    /// <summary>Gets the visibility of the code.</summary>
    public CodeVisibility Visibility { get; }

    /// <inheritdoc />
    [Pure]
    public override bool IsDefined(Type attributeType, bool inherit)
        => GetCustomAttributes(attributeType, inherit).Any();

    /// <inheritdoc />
    [Pure]
    public override int GetArrayRank() => 0;

    /// <inheritdoc />
    [Pure]
    public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
        => Constructors.Where(c => (c.Bindings() & bindingAttr) != default).ToArray();

    /// <inheritdoc />
    [Pure]
    public override object[] GetCustomAttributes(bool inherit)
        => AttributeInfos.Cast<object>()
        .Concat(inherit ? BaseType.GetCustomAttributes(true) : Array.Empty<object>())
        .ToArray();

    /// <inheritdoc />
    [Pure]
    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        => GetCustomAttributes(inherit)
        .Where(a => (a is AttributeInfo d && d.AttributeType == attributeType) || a.GetType() == attributeType)
        .ToArray();

    /// <summary>
    /// Gets the <see cref="AttributeInfos"/> linked to the type.
    /// </summary>
    [Pure]
    public IReadOnlyCollection<AttributeInfo> GetAttributeInfos() => AttributeInfos;

    /// <summary>Gets the (explicitly added) derived types.</summary>
    [Pure]
    public IReadOnlyCollection<Type> GetDerivedTypes() => DerivedTypes;

    /// <inheritdoc />
    [Pure]
    public override EventInfo? GetEvent(string name, BindingFlags bindingAttr)
        => GetEvents(bindingAttr).Find(e => e.Name == name);

    /// <inheritdoc />
    [Pure]
    public override EventInfo[] GetEvents(BindingFlags bindingAttr)
        => Events.Where(e => (e.Bindings() & bindingAttr) != default).ToArray();

    /// <inheritdoc />
    [Pure]
    public override FieldInfo? GetField(string name, BindingFlags bindingAttr)
        => GetFields(bindingAttr).Find(f => f.Name == name);

    /// <inheritdoc />
    [Pure]
    public override FieldInfo[] GetFields(BindingFlags bindingAttr)
        => Fields.Where(f => (f.Bindings() & bindingAttr) != default).ToArray();

    /// <inheritdoc />
    [Pure]
    public override Type[] GetGenericArguments() => Array.Empty<Type>();

    /// <inheritdoc />
    [Pure]
    public override Type[] GetInterfaces() => Interfaces.ToArray();

    /// <inheritdoc />
    [Pure]
    public override Type? GetInterface(string name, bool ignoreCase)
        => Interfaces.FirstOrDefault(i => i.Name.Equals(name, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));

    /// <inheritdoc />
    [Pure]
    public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
        => GetProperties(bindingAttr).Cast<MemberInfo>()
        .Concat(GetFields(bindingAttr))
        .ToArray();

    /// <inheritdoc />
    [Pure]
    public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
        => Methods.Where(p => (p.Bindings() & bindingAttr) != default).ToArray();

    /// <inheritdoc />
    [Pure]
    public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        => Properties.Where(p => (p.Bindings() & bindingAttr) != default).ToArray();

    /// <inheritdoc />
    [Pure]
    public override object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, ParameterModifier[]? modifiers, CultureInfo? culture, string[]? namedParameters)
        => throw new NotSupportedException();

    /// <inheritdoc />
    [Pure]
    public override Type MakeArrayType() => MakeArrayType(1);

    /// <inheritdoc />
    [Pure]
    public override Type MakeArrayType(int rank) => new ArrayType(this, rank);

    /// <inheritdoc />
    [Pure]
    protected override TypeAttributes GetAttributeFlagsImpl()
        => DerivedTypes.Any()
            ? (TypeAttributes & ~TypeAttributes.Sealed)
            : TypeAttributes;

    /// <inheritdoc />
    [Pure]
    protected override ConstructorInfo? GetConstructorImpl(BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[]? modifiers)
        => GetConstructors(bindingAttr)
        .Find(c => c.GetParameters().Select(p => p.ParameterType).SequenceEqual(types));

    /// <inheritdoc />
    [Pure]
    protected override MethodInfo? GetMethodImpl(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers)
        => GetMethods(bindingAttr)
        .Find(m => m.Name == name);

    /// <inheritdoc />
    [Pure]
    protected override PropertyInfo? GetPropertyImpl(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[]? types, ParameterModifier[]? modifiers)
        => GetProperties(bindingAttr)
        .Find(p => p.Name == name && (returnType is null || p.PropertyType == returnType));

    /// <inheritdoc />
    [Pure]
    protected override bool HasElementTypeImpl() => IsArrayImpl();

    /// <inheritdoc />
    [Pure]
    protected override bool IsArrayImpl() => false;

    /// <inheritdoc />
    [Pure]
    protected sealed override bool IsByRefImpl() => false;

    /// <inheritdoc />
    [Pure]
    protected sealed override bool IsCOMObjectImpl() => false;

    /// <inheritdoc />
    [Pure]
    protected sealed override bool IsPointerImpl() => false;

    /// <inheritdoc />
    [Pure]
    protected sealed override bool IsPrimitiveImpl() => false;

    /// <inheritdoc />
    [Pure]
    public override Type? GetNestedType(string name, BindingFlags bindingAttr)
        => GetNestedTypes(bindingAttr)
        .Find(t => t.Name == name);

    /// <inheritdoc />
    [Pure]
    public override Type[] GetNestedTypes(BindingFlags bindingAttr) => Array.Empty<Type>();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.ToCSharpString(withNamespace: true);
}
