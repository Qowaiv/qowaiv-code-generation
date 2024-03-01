using Qowaiv.CodeGeneration;

namespace System.Reflection;

/// <summary>Extensions on <see cref="ConstructorInfo"/>.</summary>
public static class QowaivConstructorInfoExtensions
{
    /// <summary>Resolves the <see cref="BindingFlags"/> of a constructor.</summary>
    [Pure]
    public static BindingFlags Bindings(this ConstructorInfo ctor)
    {
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
        // Only to represent the state, not to access private code.
        Guard.NotNull(ctor);
        var bindings = BindingFlags.Default;
        bindings |= ctor.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic;
        bindings |= ctor.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
        return bindings;

#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
    }
}
