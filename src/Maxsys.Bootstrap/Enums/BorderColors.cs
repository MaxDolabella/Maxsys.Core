using System.ComponentModel;

namespace Maxsys.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/utilities/borders/#border-color">Documentation</see>
/// </summary>
public enum BorderColors : byte
{
    [Description("")]
    None = 0,

    /// <summary>
    /// border-primary
    /// </summary>
    [Description("border-primary")]
    Primary,

    /// <summary>
    /// border-primary-subtle
    /// </summary>
    [Description("border-primary-subtle")]
    PrimarySubtle,

    /// <summary>
    /// border-secondary
    /// </summary>
    [Description("border-secondary")]
    Secondary,

    /// <summary>
    /// border-secondary-subtle
    /// </summary>
    [Description("border-secondary-subtle")]
    SecondarySubtle,

    /// <summary>
    /// border-success
    /// </summary>
    [Description("border-success")]
    Success,

    /// <summary>
    /// border-success-subtle
    /// </summary>
    [Description("border-success-subtle")]
    SuccessSubtle,

    /// <summary>
    /// border-danger
    /// </summary>
    [Description("border-danger")]
    Danger,

    /// <summary>
    /// border-danger-subtle
    /// </summary>
    [Description("border-danger-subtle")]
    DangerSubtle,

    /// <summary>
    /// border-warning
    /// </summary>
    [Description("border-warning")]
    Warning,

    /// <summary>
    /// border-warning-subtle
    /// </summary>
    [Description("border-warning-subtle")]
    WarningSubtle,

    /// <summary>
    /// border-info
    /// </summary>
    [Description("border-info")]
    Info,

    /// <summary>
    /// border-info-subtle
    /// </summary>
    [Description("border-info-subtle")]
    InfoSubtle,

    /// <summary>
    /// border-light
    /// </summary>
    [Description("border-light")]
    Light,

    /// <summary>
    /// border-light-subtle
    /// </summary>
    [Description("border-light-subtle")]
    LightSubtle,

    /// <summary>
    /// border-dark
    /// </summary>
    [Description("border-dark")]
    Dark,

    /// <summary>
    /// border-dark-subtle
    /// </summary>
    [Description("border-dark-subtle")]
    DarkSubtle,

    /// <summary>
    /// border-black
    /// </summary>
    [Description("border-black")]
    Black,

    /// <summary>
    /// border-white
    /// </summary>
    [Description("border-white")]
    White,
}