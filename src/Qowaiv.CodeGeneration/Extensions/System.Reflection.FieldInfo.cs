using Qowaiv.CodeGeneration;

namespace System.Reflection;

public static class QowaivFieldInfoExtensions
{
    [Pure]  
    public static BindingFlags Bindings(this FieldInfo field)
    {
        Guard.NotNull(field, nameof(field));
        var bindings = BindingFlags.Default;
        bindings |= field.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic;
        bindings |= field.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
        return bindings;
    }
}
