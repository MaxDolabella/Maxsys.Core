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
    public TextColors Color { get; set; } = IconDefaults.Color;

    [HtmlAttributeName("custom-color")]
    public string? CustomColor { get; set; } = IconDefaults.CustomColor;

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

        if (!string.IsNullOrWhiteSpace(CustomColor))
        {
            output.AddStyle($"color:{CustomColor};", HtmlEncoder.Default);
            //output.Attributes.Add("style", $"color:{CustomColor};");
        }
        else if (Color is not TextColors.None)
        {
            output.AddClass(Color.ToFriendlyName(), HtmlEncoder.Default);
        }

        //context.AllAttributes.of

        output.Content.Clear();
    }
}

public static class IconDefaults
{
    public static TextColors Color { get; set; } = TextColors.None;
    public static string? CustomColor { get; set; } = null;
}