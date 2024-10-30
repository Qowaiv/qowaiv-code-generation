namespace Qowaiv.CodeGeneration.OpenApi;

/// <summary>The data type of a Open API schema.</summary>
public enum OpenApiDataType
{
    /// <summary>None (default).</summary>
    None = 0,

    /// <summary>Array (<see cref="Array"/>.</summary>
    array,

    /// <summary>Boolean (true/false).</summary>
    boolean,

    /// <summary>Integer (<see cref="int"/>, <see cref="long"/>, etc...).</summary>
    integer,

    /// <summary>Number (<see cref="double"/>, <see cref="decimal"/>, <see cref="int"/>, etc...).</summary>
    number,

    /// <summary>Object (<see cref="object"/>).</summary>
    @object,

    /// <summary>String (<see cref="string"/>).</summary>
    @string,
}
