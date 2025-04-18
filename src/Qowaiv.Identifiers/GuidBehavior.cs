namespace Qowaiv.Identifiers;

public abstract class GuidBehavior : IdBehavior<Guid>
{
    public virtual Guid Next() => Guid.NewGuid();
}
