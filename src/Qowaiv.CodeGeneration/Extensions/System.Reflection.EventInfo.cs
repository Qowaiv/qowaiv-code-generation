using Qowaiv.CodeGeneration;

namespace System.Reflection;

/// <summary>Extensions on <see cref="EventInfo"/>.</summary>
public static class QowaivEventInfoExtensions
{
    /// <summary>Resolves the <see cref="BindingFlags"/> of a event.</summary>
    [Pure]
    public static BindingFlags Bindings(this EventInfo @event)
    {
        Guard.NotNull(@event);
        BindingFlags bindings = BindingFlags.Public | BindingFlags.Static;
        return bindings;
    }
}
