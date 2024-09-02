using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Bootstrap <see href="https://icons.getbootstrap.com/">docs</see>
/// </summary>
[HtmlTargetElement("bs-icon", Attributes = "icon")]
public class IconTagHelper : TagHelper
{
    #region Props

    public BootstrapIcons Icon { get; set; }
    public TextColors Color { get; set; } = TextColors.None;

    [HtmlAttributeName("custom-color")]
    public string? CustomForeground { get; set; }

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // <i class="bi bi-0-circle"></i>
        output.TagName = "i";
        output.TagMode = TagMode.StartTagAndEndTag;

        if (Icon is BootstrapIcons.None)
        {
            throw new ArgumentException("Icon is required.");
        }

        foreach (var item in Icon.ToFriendlyName()!.Split(' '))
        {
            output.AddClass(item, HtmlEncoder.Default);
        }

        if (!string.IsNullOrWhiteSpace(CustomForeground))
        {
            output.Attributes.Add("style", $"color:{CustomForeground};");
        }
        else if (Color is not TextColors.None)
        {
            output.AddClass(Color.ToFriendlyName(), HtmlEncoder.Default);
        }

        //context.AllAttributes.of

        output.Content.Clear();
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
public enum IconAppearance : byte
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