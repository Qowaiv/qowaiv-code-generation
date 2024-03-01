using Qowaiv.CodeGeneration;

namespace System.Reflection;

/// <summary>Extensions on <see cref="MethodInfo"/>. </summary>
public static class QowaivMethodInfoExtensions
{
    /// <summary>Resolves the <see cref="BindingFlags"/> of a method.</summary>
    [Pure]
    public static BindingFlags Bindings(this MethodInfo method)
    {
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
        // Only to represent the state, not to access private code.
        Guard.NotNull(method);
        var bindings = BindingFlags.Default;
        bindings |= method.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic;
        bindings |= method.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
        return bindings;

#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
    }
}
