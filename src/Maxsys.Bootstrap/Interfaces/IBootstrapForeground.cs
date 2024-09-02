using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.Interfaces;

public interface IBootstrapForeground
{
    TextColors TextColor { get; set; }
    string? CustomTextColor { get; set; }

    public virtual void ApplyTextColor(TagHelperContext context, TagHelperOutput output)
    {
        if (!string.IsNullOrWhiteSpace(CustomTextColor))
        {
            output.AddStyle($"color:{CustomTextColor};", HtmlEncoder.Default);
        }
        else if (TextColor is not TextColors.None)
        {
            output.AddClass(TextColor.ToFriendlyName(), HtmlEncoder.Default);
        }
    }

    public static void Apply(IBootstrapForeground me, TagHelperContext context, TagHelperOutput output)
    {
        me.ApplyTextColor(context, output);
    }
}