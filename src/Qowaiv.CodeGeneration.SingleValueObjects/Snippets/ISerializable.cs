﻿#if !NotISerializable // exec
public partial struct @TSvo : ISerializable
{
    /// <summary>Initializes a new instance of the @FullName based on the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    private @TSvo(SerializationInfo info, StreamingContext context)
    {
        Guard.NotNull(info, nameof(info));
        m_Value = info.GetValue("Value", typeof(@type)) is @type val ? val : default(@type);
    }

    /// <summary>Adds the underlying property of the @FullName to the serialization info.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        => Guard.NotNull(info, nameof(info)).AddValue("Value", m_Value);
}
#endif // exec
