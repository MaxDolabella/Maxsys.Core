using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Grupo de botões Bootstrap.<br/>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/button-group/">docs</see>
/// </summary>
[HtmlTargetElement("bs-button-group")]
public class ButtonGroupTagHelper : TagHelper
{
    #region Props

    public ButtonGroupSizes Size { get; set; } = ButtonGroupDefaults.Size;

    [HtmlAttributeName("vertical")]
    public bool IsVertical { get; set; } = ButtonGroupDefaults.IsVertical;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="btn-group" role="group" aria-label="Basic example">
          <button type="button" class="btn btn-primary">Left</button>
          ...
        </div>
        */
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("role", "group");

        output.AddClass(IsVertical ? "btn-group-vertical" : "btn-group", HtmlEncoder.Default);

        // Size
        if (Size is not ButtonGroupSizes.None)
        {
            output.AddClass(Size.ToFriendlyName(), HtmlEncoder.Default);
        }
    }
}

/// <summary>
/// Barra de ferramentas (toolbar) que combina grupos de botões Bootstrap.<br/>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/button-group/#button-toolbar">docs</see>
/// </summary>
[HtmlTargetElement("bs-button-toolbar")]
public class ButtonToolbarTagHelper : TagHelper
{
    #region Props

    [Range(0, 5)]
    public int Gap { get; set; } = ButtonToolbarDefaults.Gap;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="btn-toolbar gap-2" role="toolbar" aria-label="Toolbar with button groups">
          <div class="btn-group" role="group">...</div>
          ...
        </div>
        */
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("role", "toolbar");

        output.AddClass("btn-toolbar", HtmlEncoder.Default);

        var gap = Gap is < 0 or > 5 ? 0 : Gap;
        if (gap > 0)
        {
            output.AddClass($"gap-{gap}", HtmlEncoder.Default);
        }
    }
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Small"/></item>
/// <item>2.<see cref="Large"/></item>
/// </list>
/// </summary>
public enum ButtonGroupSizes : byte
{
    None = 0,

    [Description("btn-group-sm")]
    Small = 1,

    [Description("btn-group-lg")]
    Large = 2,
}

public static class ButtonGroupDefaults
{
    public static ButtonGroupSizes Size = ButtonGroupSizes.None;
    public static bool IsVertical = false;
}

public static class ButtonToolbarDefaults
{
    public static int Gap = 0;
}
