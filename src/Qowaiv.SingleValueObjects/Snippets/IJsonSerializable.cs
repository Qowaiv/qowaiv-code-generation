public partial struct @TSvo
{
    /// <summary>Creates the @FullName from a JSON string.</summary>
    /// <param name="json">
    /// The JSON string to deserialize.
    /// </param>
    /// <returns>
    /// The deserialized @FullName.
    /// </returns>
#if !NotCultureDependent
    [Pure]
    public static @TSvo FromJson(string? json) => Parse(json, CultureInfo.InvariantCulture);
#else
    [Pure]
    public static @TSvo FromJson(string json) => Parse(json);
#endif
}
