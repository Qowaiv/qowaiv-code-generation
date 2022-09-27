using System.Reflection;
using System.Text;

namespace Qowaiv.OpenApi;

public sealed partial class TypeInfo : Type
{
    public TypeInfo(
        Namespace @namespace,
        string name, 
        bool isArray = false,
        Type? baseType = null)
    {
        NS = @namespace;
        Name = Guard.NotNullOrEmpty(name, nameof(name));
        _IsArray = isArray;
        BaseType = baseType ?? typeof(object);
    }

    public override string Namespace => NS.ToString();

    public Namespace NS { get; }

    public override string Name { get; }

    public override Type? BaseType { get; }

    public override string FullName => $"{Namespace}.{Name}";

    public override Guid GUID => Qowaiv.Uuid.GenerateWithSHA1(Encoding.ASCII.GetBytes(FullName));

    public override Type? GetElementType()
    {
        throw new NotImplementedException();
    }

    public override Type? GetNestedType(string name, BindingFlags bindingAttr)
    {
        throw new NotImplementedException();
    }

    public override Type[] GetNestedTypes(BindingFlags bindingAttr)
    {
        throw new NotImplementedException();
    }

    public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
    {
        throw new NotImplementedException();
    }

    protected override PropertyInfo? GetPropertyImpl(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[]? types, ParameterModifier[]? modifiers)
    {
        throw new NotImplementedException();
    }

    protected override bool HasElementTypeImpl()
    {
        throw new NotImplementedException();
    }

    protected override bool IsArrayImpl() => _IsArray;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly bool _IsArray;

    #region Not available

    public override Assembly Assembly => throw new NotSupportedException();

    public override Module Module => throw new NotSupportedException();
    public override string? AssemblyQualifiedName => throw new NotSupportedException();
    public override Type UnderlyingSystemType => throw new NotSupportedException();

    public override object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, ParameterModifier[]? modifiers, CultureInfo? culture, string[]? namedParameters) => throw new NotSupportedException();
    protected override bool IsByRefImpl() => throw new NotSupportedException();
    protected override bool IsCOMObjectImpl() => throw new NotSupportedException();
    protected override bool IsPointerImpl() => throw new NotSupportedException();
    protected override bool IsPrimitiveImpl() => throw new NotSupportedException();

    protected override TypeAttributes GetAttributeFlagsImpl() => default;

    public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) => Array.Empty<ConstructorInfo>();

    protected override ConstructorInfo? GetConstructorImpl(BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[]? modifiers) => null;

    public override EventInfo? GetEvent(string name, BindingFlags bindingAttr) => null;

    public override EventInfo[] GetEvents(BindingFlags bindingAttr) => Array.Empty<EventInfo>();

    public override FieldInfo? GetField(string name, BindingFlags bindingAttr) => null;

    public override FieldInfo[] GetFields(BindingFlags bindingAttr) => Array.Empty<FieldInfo>();

    public override MemberInfo[] GetMembers(BindingFlags bindingAttr) => null;

    public override MethodInfo[] GetMethods(BindingFlags bindingAttr) => Array.Empty<MethodInfo>();

    protected override MethodInfo? GetMethodImpl(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers) => null;

    public override Type? GetInterface(string name, bool ignoreCase)
        => GetInterfaces()
        .FirstOrDefault(i => i.Name.Equals(name, ignoreCase 
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal));
    
    public override Type[] GetInterfaces() => Array.Empty<Type>();

    public override bool IsDefined(Type attributeType, bool inherit)=> GetCustomAttributes(attributeType, inherit).Any();
    
    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        => GetCustomAttributes(inherit)
        .Where(attr => attr.GetType() == attributeType)
        .ToArray();
    
    public override object[] GetCustomAttributes(bool inherit) => Array.Empty<object>();

    #endregion
}
