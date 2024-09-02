using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.Interfaces;

public interface IBootstrapBackground
{
    BackgroundColors BackgroundColor { get; set; }
    string? CustomBackgroundColor { get; set; }

    public virtual void ApplyBackgroundColor(TagHelperContext context, TagHelperOutput output)
    {
        if (!string.IsNullOrWhiteSpace(CustomBackgroundColor))
        {
            output.AddStyle($"background-color:{CustomBackgroundColor};", HtmlEncoder.Default);
        }
        else if (BackgroundColor is not BackgroundColors.None)
        {
            output.AddClass(BackgroundColor.ToFriendlyName(), HtmlEncoder.Default);
        }
    }

    public static void Apply(IBootstrapBackground me, TagHelperContext context, TagHelperOutput output)
    {
        me.ApplyBackgroundColor(context, output);
    }
}