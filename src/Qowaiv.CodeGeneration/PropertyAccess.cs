using System.ComponentModel.DataAnnotations;

namespace Qowaiv.CodeGeneration;

public enum PropertyAccess
{
    /// <summary>{ get; init; }</summary>
    [Display(Name = "{ get; init; }")]
    InitOnly = 0,

    /// <summary>{ get; set; }</summary>
    [Display(Name = "{ get; set; }")]
    PublicSet,

    /// <summary>{ get; protected set; }</summary>
    [Display(Name = "{ get; protected set; }")]
    ProtectedSet,

    /// <summary>{ get; internal set; }</summary>
    [Display(Name = "{ get; internal set; }")]
    InternalSet,

    /// <summary>{ get; }</summary>
    [Display(Name = "{ get; }")]
    GetOnly,
}
