using System.ComponentModel;
using System.Text.Encodings.Web;
using Maxsys.Bootstrap.Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/alerts/">docs</see>
/// </summary>
[HtmlTargetElement("bs-alert")]
public class AlertTagHelper : TagHelper,
    IBootstrapBackground,
    IBootstrapText
{
    #region IBootstrapBackground

    [HtmlAttributeNotBound]
    public BackgroundColors BackgroundColor { get; set; } = BackgroundColors.None;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; } = AlertDefaults.CustomBackgroundColor;

    #endregion IBootstrapBackground

    #region IBootstrapText

    public TextTransformations TextTransform { get; set; } = AlertDefaults.TextTransform;

    public FontWeights FontWeight { get; set; } = AlertDefaults.FontWeight;

    public FontSizes TextSize { get; set; } = AlertDefaults.TextSize;

    public TextColors TextColor { get; set; } = AlertDefaults.TextColor;

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = AlertDefaults.CustomTextColor;

    [HtmlAttributeName("small")]
    public bool IsSmall { get; set; } = AlertDefaults.IsSmall;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = AlertDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = AlertDefaults.IsMonospace;

    #endregion IBootstrapText

    #region Props

    public AlertTypes Type { get; set; } = AlertDefaults.Type;
    public BootstrapIcons Icon { get; set; } = AlertDefaults.Icon;

    #endregion Props

    private static bool IsMainAppearance(AlertTypes type)
    {
        return type
            is AlertTypes.Primary
            or AlertTypes.Secondary
            or AlertTypes.Success
            or AlertTypes.Danger
            or AlertTypes.Warning
            or AlertTypes.Info
            or AlertTypes.Light
            or AlertTypes.Dark;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="alert alert-{type}" role="alert">
          A simple {type} alert—check it out!
        </div>
        */
        output.TagName = "div";
        output.AddClass("alert", HtmlEncoder.Default);
        output.Attributes.Add("role", "alert");

        // Appearance
        if (IsMainAppearance(Type))
        {
            output.AddClass(Type.ToFriendlyName(), HtmlEncoder.Default);
        }
        else
        {
            (TextColor, BackgroundColor) = Type switch
            {
                //BadgeStyle.Primary => (TextColors.Light, BackgroundColors.Primary),
                //BadgeStyle.Secondary => (TextColors.Light, BackgroundColors.Secondary),
                //BadgeStyle.Success => (TextColors.Light, BackgroundColors.Success),
                //BadgeStyle.Danger => (TextColors.Light, BackgroundColors.Danger),
                //BadgeStyle.Warning => (TextColors.Dark, BackgroundColors.Warning),
                //BadgeStyle.Info => (TextColors.Light, BackgroundColors.Info),
                //BadgeStyle.Light => (TextColors.Dark, BackgroundColors.Light),
                //BadgeStyle.Dark => (TextColors.Light, BackgroundColors.Dark),

                AlertTypes.None => (TextColors.None, BackgroundColors.None),

                AlertTypes.BodySecondary => (TextColors.Light, BackgroundColors.BodySecondary),
                AlertTypes.BodyTertiary => (TextColors.Light, BackgroundColors.BodyTertiary),
                AlertTypes.Body => (TextColors.Light, BackgroundColors.Body),
                AlertTypes.Black => (TextColors.Light, BackgroundColors.Black),
                AlertTypes.White => (TextColors.Dark, BackgroundColors.White),
                _ => throw new NotImplementedException(),
            };

            output.AddClass("border", HtmlEncoder.Default);
            output.AddClass("border-1", HtmlEncoder.Default);
        }

        // IsSmall
        if (IsSmall)
        {
            output.PreContent.AppendHtml("<small>");
            output.PostContent.AppendHtml("</small>");
        }

        if (Icon is not BootstrapIcons.None)
        {
            //<i class="bi bi-0-circle-fill me-2"></i>
            var icon = $"<i class=\"{Icon.ToFriendlyName()} me-2\"></i>";
            output.PreContent.AppendHtml(icon);
        }

        IBootstrapText.Apply(this, context, output);
        IBootstrapBackground.Apply(this, context, output);
    }
}

[HtmlTargetElement("bs-alert-header", ParentTag = "bs-alert")]
public class AlertHeaderTagHelper : TagHelper,
    IBootstrapText
{
    #region IBootstrapText

    public TextTransformations TextTransform { get; set; } = AlertHeaderDefaults.TextTransform;

    public FontWeights FontWeight { get; set; } = AlertHeaderDefaults.FontWeight;

    public FontSizes TextSize { get; set; } = AlertHeaderDefaults.TextSize;

    public TextColors TextColor { get; set; } = AlertHeaderDefaults.TextColor;

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = AlertHeaderDefaults.CustomTextColor;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = AlertHeaderDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = AlertHeaderDefaults.IsMonospace;

    #endregion IBootstrapText

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <h4 class="alert-heading">Well done!</h4>
        */
        output.TagName = "h4";
        output.AddClass("alert-heading", HtmlEncoder.Default);

        IBootstrapText.Apply(this, context, output);
    }
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Primary"/></item>
/// <item>3.<see cref="Secondary"/></item>
/// <item>5.<see cref="Success"/></item>
/// <item>7.<see cref="Danger"/></item>
/// <item>9.<see cref="Warning"/></item>
/// <item>11.<see cref="Info"/></item>
/// <item>13.<see cref="Light"/></item>
/// <item>15.<see cref="Dark"/></item>
/// <item>17.<see cref="BodySecondary"/></item>
/// <item>18.<see cref="BodyTertiary"/></item>
/// <item>19.<see cref="Body"/></item>
/// <item>20.<see cref="Black"/></item>
/// <item>21.<see cref="White"/></item>
/// </list>
/// </summary>
public enum AlertTypes : byte
{
    None = 0,

    [Description("alert-primary")]
    Primary = 1,

    [Description("alert-secondary")]
    Secondary = 3,

    [Description("alert-success")]
    Success = 5,

    [Description("alert-danger")]
    Danger = 7,

    [Description("alert-warning")]
    Warning = 9,

    [Description("alert-info")]
    Info = 11,

    [Description("alert-light")]
    Light = 13,

    [Description("alert-dark")]
    Dark = 15,

    BodySecondary = 17,
    BodyTertiary = 18,

    Body = 19,
    Black = 20,
    White = 21,
}

public static class AlertDefaults
{
    public static AlertTypes Type = AlertTypes.Primary;
    public static BootstrapIcons Icon = BootstrapIcons.None;
    public static string? CustomBackgroundColor = null;
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontWeights FontWeight = FontWeights.None;
    public static FontSizes TextSize = FontSizes.None;
    public static TextColors TextColor = TextColors.None;
    public static string? CustomTextColor = null;
    public static bool IsSmall = false;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
}

public static class AlertHeaderDefaults
{
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontWeights FontWeight = FontWeights.None;
    public static FontSizes TextSize = FontSizes.None;
    public static TextColors TextColor = TextColors.None;
    public static string? CustomTextColor = null;
    public static bool IsSmall = false;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
}