using Qowaiv.CodeGeneration;

namespace System.Reflection;

public static class QowaivConstructorInfoExtensions
{
    [Pure]
    public static BindingFlags Bindings(this ConstructorInfo ctor)
    {
        Guard.NotNull(ctor, nameof(ctor));
        var bindings = BindingFlags.Default;
        bindings |= ctor.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic;
        bindings |= ctor.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
        return bindings;
    }
}
