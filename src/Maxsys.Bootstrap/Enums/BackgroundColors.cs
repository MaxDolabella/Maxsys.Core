using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Maxsys.Bootstrap;

/// <summary>
/// <list type="bullet">
/// <item>
///     <term>1.<see cref="Primary"/></term>
///     <description>bg-primary</description>
/// </item>
/// <item>
///     <term>2.<see cref="PrimarySubtle"/></term>
///     <description>bg-primary-subtle</description>
/// </item>
/// <item>
///     <term>3.<see cref="Secondary"/></term>
///     <description>bg-secondary</description>
/// </item>
/// <item>
///     <term>4.<see cref="SecondarySubtle"/></term>
///     <description>bg-secondary-subtle</description>
/// </item>
/// <item>
///     <term>5.<see cref="Success"/></term>
///     <description>bg-success</description>
/// </item>
/// <item>
///     <term>6.<see cref="SuccessSubtle"/></term>
///     <description>bg-success-subtle</description>
/// </item>
/// <item>
///     <term>7.<see cref="Danger"/></term>
///     <description>bg-danger</description>
/// </item>
/// <item>
///     <term>8.<see cref="DangerSubtle"/></term>
///     <description>bg-danger-subtle</description>
/// </item>
/// <item>
///     <term>9.<see cref="Warning"/></term>
///     <description>bg-warning</description>
/// </item>
/// <item>
///     <term>10.<see cref="WarningSubtle"/></term>
///     <description>bg-warning-subtle</description>
/// </item>
/// <item>
///     <term>11.<see cref="Info"/></term>
///     <description>bg-info</description>
/// </item>
/// <item>
///     <term>12.<see cref="InfoSubtle"/></term>
///     <description>bg-info-subtle</description>
/// </item>
/// <item>
///     <term>13.<see cref="Light"/></term>
///     <description>bg-light</description>
/// </item>
/// <item>
///     <term>14.<see cref="LightSubtle"/></term>
///     <description>bg-light-subtle</description>
/// </item>
/// <item>
///     <term>15.<see cref="Dark"/></term>
///     <description>bg-dark</description>
/// </item>
/// <item>
///     <term>16.<see cref="DarkSubtle"/></term>
///     <description>bg-dark-subtle</description>
/// </item>
/// <item>
///     <term>17.<see cref="BodySecondary"/></term>
///     <description>bg-body-secondary</description>
/// </item>
/// <item>
///     <term>18.<see cref="BodyTertiary"/></term>
///     <description>bg-body-tertiary</description>
/// </item>
/// <item>
///     <term>19.<see cref="Body"/></term>
///     <description>bg-body</description>
/// </item>
/// <item>
///     <term>20.<see cref="Black"/></term>
///     <description>bg-black</description>
/// </item>
/// <item>
///     <term>21.<see cref="White"/></term>
///     <description>bg-white</description>
/// </item>
/// <item>
///     <term>22.<see cref="Transparent"/></term>
///     <description>bg-transparent</description>
/// </item>
/// </list>
/// </summary>
public enum BackgroundColors : byte
{
    [Description("")]
    None = 0,

    /// <summary>
    /// bg-primary
    /// </summary>
    [Description("bg-primary"), Display(Name = "bg-primary")]
    Primary = 1,

    /// <summary>
    /// bg-primary-subtle
    /// </summary>
    [Description("bg-primary-subtle"), Display(Name = "bg-primary-subtle")]
    PrimarySubtle = 2,

    /// <summary>
    /// bg-secondary
    /// </summary>
    [Description("bg-secondary"), Display(Name = "bg-secondary")]
    Secondary = 3,

    /// <summary>
    /// bg-secondary-subtle
    /// </summary>
    [Description("bg-secondary-subtle"), Display(Name = "bg-secondary-subtle")]
    SecondarySubtle = 4,

    /// <summary>
    /// bg-success
    /// </summary>
    [Description("bg-success"), Display(Name = "bg-success")]
    Success = 5,

    /// <summary>
    /// bg-success-subtle
    /// </summary>
    [Description("bg-success-subtle"), Display(Name = "bg-success-subtle")]
    SuccessSubtle = 6,

    /// <summary>
    /// bg-danger
    /// </summary>
    [Description("bg-danger"), Display(Name = "bg-danger")]
    Danger = 7,

    /// <summary>
    /// bg-danger-subtle
    /// </summary>
    [Description("bg-danger-subtle"), Display(Name = "bg-danger-subtle")]
    DangerSubtle = 8,

    /// <summary>
    /// bg-warning
    /// </summary>
    [Description("bg-warning"), Display(Name = "bg-warning")]
    Warning = 9,

    /// <summary>
    /// bg-warning-subtle
    /// </summary>
    [Description("bg-warning-subtle"), Display(Name = "bg-warning-subtle")]
    WarningSubtle = 10,

    /// <summary>
    /// bg-info
    /// </summary>
    [Description("bg-info"), Display(Name = "bg-info")]
    Info = 11,

    /// <summary>
    /// bg-info-subtle
    /// </summary>
    [Description("bg-info-subtle"), Display(Name = "bg-info-subtle")]
    InfoSubtle = 12,

    /// <summary>
    /// bg-light
    /// </summary>
    [Description("bg-light"), Display(Name = "bg-light")]
    Light = 13,

    /// <summary>
    /// bg-light-subtle
    /// </summary>
    [Description("bg-light-subtle"), Display(Name = "bg-light-subtle")]
    LightSubtle = 14,

    /// <summary>
    /// bg-dark
    /// </summary>
    [Description("bg-dark"), Display(Name = "bg-dark")]
    Dark = 15,

    /// <summary>
    /// bg-dark-subtle
    /// </summary>
    [Description("bg-dark-subtle"), Display(Name = "bg-dark-subtle")]
    DarkSubtle = 16,

    /// <summary>
    /// bg-body-secondary
    /// </summary>
    [Description("bg-body-secondary"), Display(Name = "bg-body-secondary")]
    BodySecondary = 17,

    /// <summary>
    /// bg-body-tertiary
    /// </summary>
    [Description("bg-body-tertiary"), Display(Name = "bg-body-tertiary")]
    BodyTertiary = 18,

    /// <summary>
    /// bg-body
    /// </summary>
    [Description("bg-body"), Display(Name = "bg-body")]
    Body = 19,

    /// <summary>
    /// bg-black
    /// </summary>
    [Description("bg-black"), Display(Name = "bg-black")]
    Black = 20,

    /// <summary>
    /// bg-white
    /// </summary>
    [Description("bg-white"), Display(Name = "bg-white")]
    White = 21,

    /// <summary>
    /// bg-transparent
    /// </summary>
    [Description("bg-transparent"), Display(Name = "bg-transparent")]
    Transparent = 22,
}