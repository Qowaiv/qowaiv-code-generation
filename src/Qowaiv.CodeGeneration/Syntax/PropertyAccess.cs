namespace Qowaiv.CodeGeneration.Syntax;

public enum PropertyAccess
{
    /// <summary>Init only `{ get; init; }`.</summary>
    InitOnly = 0,

    /// <summary>Public set `{ get; set; }`.</summary>
    PublicSet,

    /// <summary>Protected set `{ get; protected set; }`.</summary>
    ProtectedSet,

    /// <summary>Internal set `{ get; internal set; }`.</summary>
    InternalSet,

    /// <summary>Get only `{ get; }`.</summary>
    GetOnly,
}

