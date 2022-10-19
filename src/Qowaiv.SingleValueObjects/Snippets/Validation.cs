﻿public partial struct @TSvo
{
#if !NotCultureDependent // exec
    /// <summary>Returns true if the value represents a valid @FullName.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    [Pure]
    public static bool IsValid(string? val) => IsValid(val, (IFormatProvider?)null);

    /// <summary>Returns true if the value represents a valid @FullName.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    /// <param name="formatProvider">
    /// The <see cref="IFormatProvider"/> to interpret the <see cref="string"/> value with.
    /// </param>
    [Pure]
    public static bool IsValid(string? val, IFormatProvider? formatProvider)
        => !string.IsNullOrWhiteSpace(val)
        && TryParse(val, formatProvider, out _);
#else // exec
    /// <summary>Returns true if the value represents a valid @FullName.</summary>
    /// <param name="val">
    /// The <see cref="string"/> to validate.
    /// </param>
    [Pure]
    public static bool IsValid(string val)
        => !string.IsNullOrWhiteSpace(val)
        && TryParse(val, out _);
#endif // exec
}
