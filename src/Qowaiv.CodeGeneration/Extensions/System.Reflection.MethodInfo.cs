using Qowaiv.CodeGeneration;

namespace System.Reflection;

public static class QowaivMethodInfoExtensions
{
    [Pure]  
    public static BindingFlags Bindings(this MethodInfo method)
    {
        Guard.NotNull(method, nameof(method));
        var bindings = BindingFlags.Default;
        bindings |= method.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic;
        bindings |= method.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
        return bindings;
    }
}
