using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.Interfaces;

public interface IBootstrapText : IBootstrapForeground//, IBootstrapTextAlign
{
    TextTransformations TextTransform { get; set; }
    FontWeights FontWeight { get; set; }
    bool IsItalic { get; set; }
    bool IsMonospace { get; set; }
    FontSizes TextSize { get; set; }

    public virtual void ApplyTextTransform(TagHelperContext context, TagHelperOutput output)
    {
        if (TextTransform is not TextTransformations.None)
        {
            output.AddClass(TextTransform.ToFriendlyName(), HtmlEncoder.Default);
        }
    }

    public virtual void ApplyFontWeight(TagHelperContext context, TagHelperOutput output)
    {
        if (FontWeight is not FontWeights.None)
        {
            output.AddClass(FontWeight.ToFriendlyName(), HtmlEncoder.Default);
        }
    }

    public virtual void ApplyIsItalic(TagHelperContext context, TagHelperOutput output)
    {
        if (IsItalic)
        {
            output.AddClass("fst-italic", HtmlEncoder.Default);
        }
    }

    public virtual void ApplyIsMonospace(TagHelperContext context, TagHelperOutput output)
    {
        if (IsMonospace)
        {
            output.AddClass("font-monospace", HtmlEncoder.Default);
        }
    }

    public virtual void ApplyTextSize(TagHelperContext context, TagHelperOutput output)
    {
        if (TextSize is not FontSizes.None)
        {
            output.AddClass(TextSize.ToFriendlyName(), HtmlEncoder.Default);
        }
    }

    public static void Apply(IBootstrapText me, TagHelperContext context, TagHelperOutput output)
    {
        IBootstrapForeground.Apply(me, context, output);

        me.ApplyTextTransform(context, output);
        me.ApplyFontWeight(context, output);
        me.ApplyIsItalic(context, output);
        me.ApplyIsMonospace(context, output);
        me.ApplyTextSize(context, output);
    }
}