using System.Reflection;

namespace Qowaiv.CodeGeneration;

public abstract partial class TypeInfo : Type
{
    protected TypeInfo(
        TypeName nameType,
        bool isArray = false,
        Type? baseType = null)
    {
        NS = Guard.NotNull(nameType, nameof(nameType)).Namespace;
        Name = nameType.Name;
        _IsArray = isArray;
        BaseType = baseType ?? typeof(object);
    }

    /// <inheritdoc />
    public override string Namespace => NS.ToString();

    public Namespace NS { get; }

    /// <inheritdoc />
    public override string Name { get; }

    /// <inheritdoc />
    public override Type? BaseType { get; }

    /// <inheritdoc />
    public override string FullName => $"{Namespace}.{Name}";



    /// <inheritdoc />
    public override Guid GUID => Qowaiv.Uuid.GenerateWithSHA1(Encoding.ASCII.GetBytes(FullName));

    /// <inheritdoc />
    [Pure]
    public override Type? GetNestedType(string name, BindingFlags bindingAttr) => null;

    /// <inheritdoc />
    [Pure]
    public override Type[] GetNestedTypes(BindingFlags bindingAttr) => Array.Empty<Type>();

    /// <inheritdoc />
    [Pure]
    protected override PropertyInfo? GetPropertyImpl(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[]? types, ParameterModifier[]? modifiers)
    => GetProperties(bindingAttr)
        .SingleOrDefault(p 
            => p.Name == name
            && (returnType is null || p.PropertyType == returnType));

    /// <inheritdoc />
    [Pure]
    protected override bool IsArrayImpl() => _IsArray;
    
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly bool _IsArray;

    /// <inheritdoc />
    [Pure]
    protected override bool HasElementTypeImpl() => IsArrayImpl();

    /// <inheritdoc />
    [Pure]
    protected sealed override bool IsPointerImpl() => false;

    /// <inheritdoc />
    [Pure]
    protected sealed override bool IsPrimitiveImpl() => false;

    /// <inheritdoc />
    [Pure]
    protected sealed override bool IsByRefImpl() => false;

    /// <inheritdoc />
    [Pure]
    protected sealed  override bool IsCOMObjectImpl() => false;

    #region Not available

    public override Assembly Assembly => throw new NotSupportedException();

    public override Module Module => throw new NotSupportedException();
    public override string? AssemblyQualifiedName => throw new NotSupportedException();
    public override Type UnderlyingSystemType => throw new NotSupportedException();

    /// <inheritdoc />
    [Pure]
    public override object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, ParameterModifier[]? modifiers, CultureInfo? culture, string[]? namedParameters) => throw new NotSupportedException();
    


    /// <inheritdoc />
    [Pure]
    protected override TypeAttributes GetAttributeFlagsImpl() => default;

    /// <inheritdoc />
    [Pure]
    public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) => Array.Empty<ConstructorInfo>();

    /// <inheritdoc />
    [Pure]
    protected override ConstructorInfo? GetConstructorImpl(BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[]? modifiers) => null;

    /// <inheritdoc />
    [Pure]
    public override EventInfo? GetEvent(string name, BindingFlags bindingAttr) => null;

    /// <inheritdoc />
    [Pure]
    public override EventInfo[] GetEvents(BindingFlags bindingAttr) => Array.Empty<EventInfo>();

    /// <inheritdoc />
    [Pure]
    public override FieldInfo? GetField(string name, BindingFlags bindingAttr) => GetFields(bindingAttr).SingleOrDefault(f => f.Name == name);

    /// <inheritdoc />
    [Pure]
    public override FieldInfo[] GetFields(BindingFlags bindingAttr) => Array.Empty<FieldInfo>();

    /// <inheritdoc />
    [Pure]
    public override MemberInfo[] GetMembers(BindingFlags bindingAttr) => Array.Empty<MemberInfo>();

    /// <inheritdoc />
    [Pure]
    public override MethodInfo[] GetMethods(BindingFlags bindingAttr) => Array.Empty<MethodInfo>();

    /// <inheritdoc />
    [Pure]
    protected override MethodInfo? GetMethodImpl(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers) => null;

    /// <inheritdoc />
    [Pure]
    public override Type? GetInterface(string name, bool ignoreCase)
        => GetInterfaces()
        .FirstOrDefault(i => i.Name.Equals(name, ignoreCase 
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal));

    /// <inheritdoc />
    [Pure]
    public override Type[] GetInterfaces() => Array.Empty<Type>();

    /// <inheritdoc />
    [Pure]
    public override bool IsDefined(Type attributeType, bool inherit)=> GetCustomAttributes(attributeType, inherit).Any();

    /// <inheritdoc />
    [Pure]
    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        => GetCustomAttributes(inherit)
        .Where(attr => attr.GetType() == attributeType)
        .ToArray();

    /// <inheritdoc />
    [Pure]
    public override object[] GetCustomAttributes(bool inherit) => Array.Empty<object>();

    #endregion

    /// <summary>Returns "[]" for arrays, otherwise <see cref="string.Empty"/>.</summary>
    [Pure]
    protected string ArraySuffix() => IsArray ? "[]" : string.Empty;
}
