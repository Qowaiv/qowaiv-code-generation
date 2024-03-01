using Qowaiv.CodeGeneration;

namespace System.Reflection;

/// <summary>Extensions on <see cref="FieldInfo"/>. </summary>
public static class QowaivFieldInfoExtensions
{
    /// <summary>Resolves the <see cref="BindingFlags"/> of a field.</summary>
    [Pure]
    public static BindingFlags Bindings(this FieldInfo field)
    {
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
        // Only to represent the state, not to access private code.
        Guard.NotNull(field);
        var bindings = BindingFlags.Default;
        bindings |= field.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic;
        bindings |= field.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
        return bindings;

#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
    }
}
