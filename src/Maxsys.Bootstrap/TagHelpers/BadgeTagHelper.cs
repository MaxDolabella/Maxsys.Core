using System.ComponentModel;
using System.Text.Encodings.Web;
using Maxsys.SolutionScaffolder.MVC.Bootstrap.Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/badge/">docs</see>
/// </summary>
[HtmlTargetElement("bs-badge")]
public class BadgeTagHelper : TagHelper,
    IBootstrapBackground,
    IBootstrapText
{
    #region IBootstrapBackground

    [HtmlAttributeNotBound]
    public BackgroundColors BackgroundColor { get; set; } = BackgroundColors.None;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; }

    #endregion IBootstrapBackground

    #region IBootstrapText

    [HtmlAttributeNotBound]
    public TextTransformations TextTransform { get; set; } = TextTransformations.None;

    [HtmlAttributeNotBound]
    public FontWeights FontWeight { get; set; } = FontWeights.None;

    [HtmlAttributeName("size")]
    public FontSizes TextSize { get; set; } = FontSizes.None;

    [HtmlAttributeNotBound]
    public TextColors TextColor { get; set; } = TextColors.None;

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; }

    [HtmlAttributeName("small")]
    public bool IsSmall { get; set; } = false;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = false;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = false;

    #endregion IBootstrapText

    #region Props

    public BadgeAppearance Appearance { get; set; } = BadgeAppearance.Primary;

    [HtmlAttributeName("rounded-pill")]
    public bool IsRounded { get; set; } = false;

    #endregion Props

    private static bool IsMainAppearance(BadgeAppearance appearance)
    {
        return appearance
            is BadgeAppearance.Primary
            or BadgeAppearance.Secondary
            or BadgeAppearance.Success
            or BadgeAppearance.Danger
            or BadgeAppearance.Warning
            or BadgeAppearance.Info
            or BadgeAppearance.Light
            or BadgeAppearance.Dark;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // <span class="badge {textColor} {backgroundColor} rounded-pill">content</span>
        output.TagName = "span";
        output.AddClass("badge", HtmlEncoder.Default);

        // Appearance
        if (IsMainAppearance(Appearance))
        {
            output.AddClass(Appearance.ToFriendlyName(), HtmlEncoder.Default);
        }
        else
        {
            (TextColor, BackgroundColor) = Appearance switch
            {
                //BadgeStyle.Primary => (TextColors.Light, BackgroundColors.Primary),
                //BadgeStyle.Secondary => (TextColors.Light, BackgroundColors.Secondary),
                //BadgeStyle.Success => (TextColors.Light, BackgroundColors.Success),
                //BadgeStyle.Danger => (TextColors.Light, BackgroundColors.Danger),
                //BadgeStyle.Warning => (TextColors.Dark, BackgroundColors.Warning),
                //BadgeStyle.Info => (TextColors.Light, BackgroundColors.Info),
                //BadgeStyle.Light => (TextColors.Dark, BackgroundColors.Light),
                //BadgeStyle.Dark => (TextColors.Light, BackgroundColors.Dark),

                BadgeAppearance.None => (TextColors.None, BackgroundColors.None),
                BadgeAppearance.PrimarySubtle => (TextColors.PrimaryEmphasis, BackgroundColors.PrimarySubtle),
                BadgeAppearance.SecondarySubtle => (TextColors.SecondaryEmphasis, BackgroundColors.SecondarySubtle),
                BadgeAppearance.SuccessSubtle => (TextColors.SuccessEmphasis, BackgroundColors.SuccessSubtle),
                BadgeAppearance.DangerSubtle => (TextColors.DangerEmphasis, BackgroundColors.DangerSubtle),
                BadgeAppearance.WarningSubtle => (TextColors.WarningEmphasis, BackgroundColors.WarningSubtle),
                BadgeAppearance.InfoSubtle => (TextColors.InfoEmphasis, BackgroundColors.InfoSubtle),
                BadgeAppearance.LightSubtle => (TextColors.LightEmphasis, BackgroundColors.LightSubtle),
                BadgeAppearance.DarkSubtle => (TextColors.DarkEmphasis, BackgroundColors.DarkSubtle),
                BadgeAppearance.BodySecondary => (TextColors.Light, BackgroundColors.BodySecondary),
                BadgeAppearance.BodyTertiary => (TextColors.Light, BackgroundColors.BodyTertiary),
                BadgeAppearance.Body => (TextColors.Light, BackgroundColors.Body),
                BadgeAppearance.Black => (TextColors.Light, BackgroundColors.Black),
                BadgeAppearance.White => (TextColors.Dark, BackgroundColors.White),
                _ => throw new NotImplementedException(),
            };
        }

        // Rounded pill
        if (IsRounded)
        {
            output.AddClass("rounded-pill", HtmlEncoder.Default);
        }

        // Rounded pill
        if (IsSmall)
        {
            output.PreElement.AppendHtml("<small>");
            output.PostElement.AppendHtml("</small>");
        }

        IBootstrapText.Apply(this, context, output);
        IBootstrapBackground.Apply(this, context, output);
    }

}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Primary"/></item>
/// <item>2.<see cref="PrimarySubtle"/></item>
/// <item>3.<see cref="Secondary"/></item>
/// <item>4.<see cref="SecondarySubtle"/></item>
/// <item>5.<see cref="Success"/></item>
/// <item>6.<see cref="SuccessSubtle"/></item>
/// <item>7.<see cref="Danger"/></item>
/// <item>8.<see cref="DangerSubtle"/></item>
/// <item>9.<see cref="Warning"/></item>
/// <item>10.<see cref="WarningSubtle"/></item>
/// <item>11.<see cref="Info"/></item>
/// <item>12.<see cref="InfoSubtle"/></item>
/// <item>13.<see cref="Light"/></item>
/// <item>14.<see cref="LightSubtle"/></item>
/// <item>15.<see cref="Dark"/></item>
/// <item>16.<see cref="DarkSubtle"/></item>
/// <item>17.<see cref="BodySecondary"/></item>
/// <item>18.<see cref="BodyTertiary"/></item>
/// <item>19.<see cref="Body"/></item>
/// <item>20.<see cref="Black"/></item>
/// <item>21.<see cref="White"/></item>
/// </list>
/// </summary>
public enum BadgeAppearance : byte
{
    None = 0,

    [Description("text-bg-primary")]
    Primary = 1,

    PrimarySubtle = 2,

    [Description("text-bg-secondary")]
    Secondary = 3,

    SecondarySubtle = 4,

    [Description("text-bg-success")]
    Success = 5,

    SuccessSubtle = 6,

    [Description("text-bg-danger")]
    Danger = 7,

    DangerSubtle = 8,

    [Description("text-bg-warning")]
    Warning = 9,

    WarningSubtle = 10,

    [Description("text-bg-info")]
    Info = 11,

    InfoSubtle = 12,

    [Description("text-bg-light")]
    Light = 13,

    LightSubtle = 14,

    [Description("text-bg-dark")]
    Dark = 15,

    DarkSubtle = 16,
    BodySecondary = 17,
    BodyTertiary = 18,

    Body = 19,
    Black = 20,
    White = 21,
}