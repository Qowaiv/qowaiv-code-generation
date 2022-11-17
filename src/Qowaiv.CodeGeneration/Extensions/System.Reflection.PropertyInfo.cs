using Qowaiv.CodeGeneration;

namespace System.Reflection;

public static class QowaivPropertyInfoExtensions
{
    [Pure]  
    public static BindingFlags Bindings(this PropertyInfo property)
    {
        Guard.NotNull(property, nameof(property));
        BindingFlags bindings = default;

        bindings |= property.IsPublic() ? BindingFlags.Public : BindingFlags.NonPublic;
        bindings |= property.IsStatic() ? BindingFlags.Static : BindingFlags.Instance;

        return bindings;
    }

    [Pure]
    public static bool IsStatic(this PropertyInfo property)
        => Guard.NotNull(property, nameof(property)).GetAccessors(nonPublic: true).Any(m => m.IsStatic);

    [Pure]
    public static bool IsPublic(this PropertyInfo property)
        => Guard.NotNull(property, nameof(property)).GetAccessors(nonPublic: true).Any(m => m.IsPublic);
}
