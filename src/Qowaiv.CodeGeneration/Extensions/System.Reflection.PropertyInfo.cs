using Qowaiv.CodeGeneration;

namespace System.Reflection;

/// <summary>Extensions on <see cref="FieldInfo"/>. </summary>
public static class QowaivPropertyInfoExtensions
{
    /// <summary>Resolves the <see cref="BindingFlags"/> of a property.</summary>
    [Pure]
    public static BindingFlags Bindings(this PropertyInfo property)
    {
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
        // Only to represent the state, not to access private code.
        Guard.NotNull(property);
        var bindings = BindingFlags.Default;
        bindings |= property.IsPublic() ? BindingFlags.Public : BindingFlags.NonPublic;
        bindings |= property.IsStatic() ? BindingFlags.Static : BindingFlags.Instance;
        return bindings;

#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
    }

    [Pure]
    public static bool IsStatic(this PropertyInfo property)
        => Guard.NotNull(property).GetAccessors(nonPublic: true).Exists(m => m.IsStatic);

    [Pure]
    public static bool IsPublic(this PropertyInfo property)
        => Guard.NotNull(property).GetAccessors(nonPublic: true).Exists(m => m.IsPublic);

    [Pure]
    public static bool IsNullable(this PropertyInfo property)
        => Guard.NotNull(property)
        .GetCustomAttributes().Any(a => a.GetType().FullName == "System.Runtime.CompilerServices.NullableAttribute");
}
