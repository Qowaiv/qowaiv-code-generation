using Qowaiv.CodeGeneration.Syntax;
using System.Reflection;

namespace Qowaiv.CodeGeneration;

/// <summary>Represents a base for types (classes, records, enums, Array).</summary>
[DebuggerDisplay("{DebuggerDisplay}")]
public abstract class TypeBase : Type
{
    /// <summary>The type data.</summary>
    protected TypeData Data { get; }

    /// <summary>Initializes a new instance of the <see cref="TypeBase"/> class.</summary>
    protected TypeBase(TypeData info)
    {
        Data = Guard.NotNull(info);
        TypeName = Guard.NotNull(info.TypeName);
    }

    /// <inheritdoc />
    public override Assembly Assembly => Assembly.GetCallingAssembly();

    /// <inheritdoc />
    public override string AssemblyQualifiedName => Assembly.GetName().FullName;

    /// <inheritdoc />
    public override Type BaseType => Data.BaseType ?? typeof(object);

    /// <inheritdoc />
    public override string FullName => $"{Namespace}.{Name}";

    /// <inheritdoc />
    public override Guid GUID => Uuid.GenerateWithSHA1(Encoding.ASCII.GetBytes(FullName));

    /// <summary>Returns if the type sources are spread over multiple files.</summary>
    public bool IsPartial => Data.IsPartial;

    /// <inheritdoc />
    public override Module Module => Assembly.Modules.First();

    /// <inheritdoc />
    public override string Name => TypeName.Name;

    /// <summary>The namespace of the type.</summary>
    public Namespace NameSpace => TypeName.Namespace;

    /// <summary>The namespace of the type.</summary>
    /// <remarks>
    /// This method exists to prevent unintended usage of the extension method
    /// on <see cref="Type"/>.
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
    public CodeVisibility Visibility => Data.Visibility;

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
        => Data.Constructors.Where(c => (c.Bindings() & bindingAttr) != default).ToArray();

    /// <inheritdoc />
    [Pure]
    public override object[] GetCustomAttributes(bool inherit)
        => Data.Attributes.Cast<object>()
        .Concat(inherit ? BaseType.GetCustomAttributes(true) : [])
        .ToArray();

    /// <inheritdoc />
    [Pure]
    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        => GetCustomAttributes(inherit)
        .Where(a => (a is AttributeInfo d && d.AttributeType == attributeType) || a.GetType() == attributeType)
        .ToArray();

    /// <summary>Gets the <see cref="AttributeInfo"/>'s linked to the type.</summary>
    [Pure]
    public IReadOnlyCollection<AttributeInfo> GetAttributeInfos() => Data.Attributes;

    /// <summary>Gets the (explicitly added) derived types.</summary>
    [Pure]
    public IReadOnlyCollection<Type> GetDerivedTypes() => Data.DerivedTypes;

    /// <inheritdoc />
    [Pure]
    public override EventInfo? GetEvent(string name, BindingFlags bindingAttr)
        => GetEvents(bindingAttr).Find(e => e.Name == name);

    /// <inheritdoc />
    [Pure]
    public override EventInfo[] GetEvents(BindingFlags bindingAttr)
        => Data.Events.Where(e => (e.Bindings() & bindingAttr) != default).ToArray();

    /// <inheritdoc />
    [Pure]
    public override FieldInfo? GetField(string name, BindingFlags bindingAttr)
        => GetFields(bindingAttr).Find(f => f.Name == name);

    /// <inheritdoc />
    [Pure]
    public override FieldInfo[] GetFields(BindingFlags bindingAttr)
        => Data.Fields.Where(f => (f.Bindings() & bindingAttr) != default).ToArray();

    /// <inheritdoc />
    [Pure]
    public override Type[] GetGenericArguments() => [];

    /// <inheritdoc />
    [Pure]
    public override Type[] GetInterfaces() => Data.Interfaces.ToArray();

    /// <inheritdoc />
    [Pure]
    public override Type? GetInterface(string name, bool ignoreCase)
        => Data.Interfaces.FirstOrDefault(i => i.Name.Equals(name, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));

    /// <inheritdoc />
    [Pure]
    public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
        => GetProperties(bindingAttr).Cast<MemberInfo>()
        .Concat(GetFields(bindingAttr))
        .ToArray();

    /// <inheritdoc />
    [Pure]
    public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
        => Data.Methods.Where(p => (p.Bindings() & bindingAttr) != default).ToArray();

    /// <inheritdoc />
    [Pure]
    public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
    {
        var props = new Dictionary<string, PropertyInfo>();

        Type? @base = BaseType;

        while (@base is { })
        {
            foreach (var prop in @base.GetProperties(bindingAttr))
            {
                props.TryAdd(prop.Name, prop);
            }
            @base = @base.BaseType;
        }

        foreach (var prop in Data.Properties.Where(p => (p.Bindings() & bindingAttr) != default))
        {
            props.TryAdd(prop.Name, prop);
        }

        return [.. props.Values];
    }

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
    protected override TypeAttributes GetAttributeFlagsImpl() => Data.TypeAttributes;

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
    public override Type[] GetNestedTypes(BindingFlags bindingAttr) => [];

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => this.ToCSharpString(withNamespace: true);
}
