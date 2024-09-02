using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

[HtmlTargetElement(BS_VR)]
[HtmlTargetElement(BS_HR)]
public class DividerTagHelper : TagHelper
{
    #region Consts

    private const string BS_VR = "bs-vr";
    private const string BS_HR = "bs-hr";

    #endregion Consts

    #region Props

    public TextColors Color { get; set; } = DividerDefaults.Color;

    [HtmlAttributeName("custom-color")]
    public string? CustomForeground { get; set; } = DividerDefaults.CustomForeground;

    public string? Thickness { get; set; } = DividerDefaults.Thickness;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // <div class="vr"></div>
        // <hr>

        if (context.TagName == BS_VR)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.AddClass("vr", HtmlEncoder.Default);

            if (!string.IsNullOrWhiteSpace(CustomForeground))
            {
                output.AddStyle($"color:{CustomForeground};", HtmlEncoder.Default);
            }
            else if (Color is not TextColors.None)
            {
                output.AddClass(Color.ToFriendlyName(), HtmlEncoder.Default);
            }

            if (!string.IsNullOrWhiteSpace(Thickness))
            {
                //style = "width:5px;"
                output.AddStyle($"width:{Thickness};", HtmlEncoder.Default);
            }
        }
        else
        {
            output.TagName = "hr";
            output.TagMode = TagMode.SelfClosing;

            if (!string.IsNullOrWhiteSpace(CustomForeground))
            {
                output.AddStyle($"border-color:{CustomForeground};", HtmlEncoder.Default);
            }
            else if (Color is not TextColors.None)
            {
                output.AddClass(Color.ToFriendlyName(), HtmlEncoder.Default);
            }

            if (!string.IsNullOrWhiteSpace(Thickness))
            {
                output.AddStyle($"border-width:{Thickness};", HtmlEncoder.Default);
            }
        }
    }
}

public static class DividerDefaults
{
    public static TextColors Color = TextColors.None;
    public static string? CustomForeground = null;
    public static string? Thickness = null;
}