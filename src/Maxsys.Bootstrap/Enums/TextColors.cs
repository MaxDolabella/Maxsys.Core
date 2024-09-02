using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Maxsys.Bootstrap;

/// <summary>
/// <list type="bullet">
/// <item>
///     <term>1.<see cref="Primary"/></term>
///     <description>text-primary</description>
/// </item>
/// <item>
///     <term>2.<see cref="PrimaryEmphasis"/></term>
///     <description>text-primary-emphasis</description>
/// </item>
/// <item>
///     <term>3.<see cref="Secondary"/></term>
///     <description>text-secondary</description>
/// </item>
/// <item>
///     <term>4.<see cref="SecondaryEmphasis"/></term>
///     <description>text-secondary-emphasis</description>
/// </item>
/// <item>
///     <term>5.<see cref="Success"/></term>
///     <description>text-success</description>
/// </item>
/// <item>
///     <term>6.<see cref="SuccessEmphasis"/></term>
///     <description>text-success-emphasis</description>
/// </item>
/// <item>
///     <term>7.<see cref="Danger"/></term>
///     <description>text-danger</description>
/// </item>
/// <item>
///     <term>8.<see cref="DangerEmphasis"/></term>
///     <description>text-danger-emphasis</description>
/// </item>
/// <item>
///     <term>9.<see cref="Warning"/></term>
///     <description>text-warning</description>
/// </item>
/// <item>
///     <term>10.<see cref="WarningEmphasis"/></term>
///     <description>text-warning-emphasis</description>
/// </item>
/// <item>
///     <term>11.<see cref="Info"/></term>
///     <description>text-info</description>
/// </item>
/// <item>
///     <term>12.<see cref="InfoEmphasis"/></term>
///     <description>text-info-emphasis</description>
/// </item>
/// <item>
///     <term>13.<see cref="Light"/></term>
///     <description>text-light</description>
/// </item>
/// <item>
///     <term>14.<see cref="LightEmphasis"/></term>
///     <description>text-light-emphasis</description>
/// </item>
/// <item>
///     <term>15.<see cref="Dark"/></term>
///     <description>text-dark</description>
/// </item>
/// <item>
///     <term>16.<see cref="DarkEmphasis"/></term>
///     <description>text-dark-emphasis</description>
/// </item>
/// <item>
///     <term>17.<see cref="Body"/></term>
///     <description>text-body</description>
/// </item>
/// <item>
///     <term>18.<see cref="BodyEmphasis"/></term>
///     <description>text-body-emphasis</description>
/// </item>
/// <item>
///     <term>19.<see cref="BodySecondary"/></term>
///     <description>text-body-secondary</description>
/// </item>
/// <item>
///     <term>20.<see cref="BodyTertiary"/></term>
///     <description>text-body-tertiary</description>
/// </item>
/// <item>
///     <term>21.<see cref="Black"/></term>
///     <description>text-black</description>
/// </item>
/// <item>
///     <term>22.<see cref="White"/></term>
///     <description>text-white</description>
/// </item>
/// <item>
///     <term>23.<see cref="Black50"/></term>
///     <description>text-black-50</description>
/// </item>
/// <item>
///     <term>24.<see cref="White50"/></term>
///     <description>text-white-50</description>
/// </item>
/// </list>
/// </summary>
public enum TextColors : byte
{
    [Description("")]
    None = 0,

    /// <summary>
    /// text-primary
    /// </summary>
    [Description("text-primary"), Display(Name = "text-primary")]
    Primary = 1,

    /// <summary>
    /// text-primary-emphasis
    /// </summary>
    [Description("text-primary-emphasis"), Display(Name = "text-primary-emphasis")]
    PrimaryEmphasis = 2,

    /// <summary>
    /// text-secondary
    /// </summary>
    [Description("text-secondary"), Display(Name = "text-secondary")]
    Secondary = 3,

    /// <summary>
    /// text-secondary-emphasis
    /// </summary>
    [Description("text-secondary-emphasis"), Display(Name = "text-secondary-emphasis")]
    SecondaryEmphasis = 4,

    /// <summary>
    /// text-success
    /// </summary>
    [Description("text-success"), Display(Name = "text-success")]
    Success = 5,

    /// <summary>
    /// text-success-emphasis
    /// </summary>
    [Description("text-success-emphasis"), Display(Name = "text-success-emphasis")]
    SuccessEmphasis = 6,

    /// <summary>
    /// text-danger
    /// </summary>
    [Description("text-danger"), Display(Name = "text-danger")]
    Danger = 7,

    /// <summary>
    /// text-danger-emphasis
    /// </summary>
    [Description("text-danger-emphasis"), Display(Name = "text-danger-emphasis")]
    DangerEmphasis = 8,

    /// <summary>
    /// text-warning
    /// </summary>
    [Description("text-warning"), Display(Name = "text-warning")]
    Warning = 9,

    /// <summary>
    /// text-warning-emphasis
    /// </summary>
    [Description("text-warning-emphasis"), Display(Name = "text-warning-emphasis")]
    WarningEmphasis = 10,

    /// <summary>
    /// text-info
    /// </summary>
    [Description("text-info"), Display(Name = "text-info")]
    Info = 11,

    /// <summary>
    /// text-info-emphasis
    /// </summary>
    [Description("text-info-emphasis"), Display(Name = "text-info-emphasis")]
    InfoEmphasis = 12,

    /// <summary>
    /// text-light
    /// </summary>
    [Description("text-light"), Display(Name = "text-light")]
    Light = 13,

    /// <summary>
    /// text-light-emphasis
    /// </summary>
    [Description("text-light-emphasis"), Display(Name = "text-light-emphasis")]
    LightEmphasis = 14,

    /// <summary>
    /// text-dark
    /// </summary>
    [Description("text-dark"), Display(Name = "text-dark")]
    Dark = 15,

    /// <summary>
    /// text-dark-emphasis
    /// </summary>
    [Description("text-dark-emphasis"), Display(Name = "text-dark-emphasis")]
    DarkEmphasis = 16,

    /// <summary>
    /// text-body
    /// </summary>
    [Description("text-body"), Display(Name = "text-body")]
    Body = 17,

    /// <summary>
    /// text-body-emphasis
    /// </summary>
    [Description("text-body-emphasis"), Display(Name = "text-body-emphasis")]
    BodyEmphasis = 18,

    /// <summary>
    /// text-body-secondary
    /// </summary>
    [Description("text-body-secondary"), Display(Name = "text-body-secondary")]
    BodySecondary = 19,

    /// <summary>
    /// text-body-tertiary
    /// </summary>
    [Description("text-body-tertiary"), Display(Name = "text-body-tertiary")]
    BodyTertiary = 20,

    /// <summary>
    /// text-black
    /// </summary>
    [Description("text-black"), Display(Name = "text-black")]
    Black = 21,

    /// <summary>
    /// text-white
    /// </summary>
    [Description("text-white"), Display(Name = "text-white")]
    White = 22,

    /// <summary>
    /// text-black-50
    /// </summary>
    [Description("text-black-50"), Display(Name = "text-black-50")]
    Black50 = 23,

    /// <summary>
    /// text-white-50
    /// </summary>
    [Description("text-white-50"), Display(Name = "text-white-50")]
    White50 = 24,
}