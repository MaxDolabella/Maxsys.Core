using System.Text.Encodings.Web;
using Maxsys.Bootstrap.Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

[HtmlTargetElement(COMMON_LABEL)]
[HtmlTargetElement(CHECK_LABEL)]
public class LabelTagHelper : TagHelper,
    IBootstrapText,
    IBootstrapBackground
{
    #region Consts

    private const string COMMON_LABEL = "bs-label";
    private const string CHECK_LABEL = "bs-check-label";

    #endregion Consts

    #region IBootstrapText

    [HtmlAttributeName("custom-color")]
    public string? CustomTextColor { get; set; } = null;

    public FontWeights FontWeight { get; set; } = FontWeights.None;
    public TextTransformations TextTransform { get; set; } = TextTransformations.None;
    public FontSizes TextSize { get; set; } = FontSizes.None;
    public TextColors TextColor { get; set; } = TextColors.None;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = false;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = false;

    #endregion IBootstrapText

    #region IBootstrapBackground

    public BackgroundColors BackgroundColor { get; set; } = BackgroundColors.None;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; } = null;

    void IBootstrapBackground.ApplyBackgroundColor(TagHelperContext context, TagHelperOutput output)
    {
        if (!string.IsNullOrWhiteSpace(CustomBackgroundColor))
        {
            output.AddStyle($"--bs-table-bg:{CustomBackgroundColor};", HtmlEncoder.Default);
        }
        else if (BackgroundColor is not BackgroundColors.None)
        {
            output.AddClass(BackgroundColor.ToFriendlyName(), HtmlEncoder.Default);
        }
    }

    #endregion IBootstrapBackground

    [HtmlAttributeName("small")]
    public bool IsSmall { get; set; } = false;

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "label";
        output.TagMode = TagMode.StartTagAndEndTag;

        if (context.TagName == CHECK_LABEL)
        {
            output.AddClass("form-check-label", HtmlEncoder.Default);
        }
        else //if (context.TagName == COMMON_LABEL)
        {
            output.AddClass("control-label", HtmlEncoder.Default);
        }

        IBootstrapText.Apply(this, context, output);
        IBootstrapBackground.Apply(this, context, output);

        // small
        if (IsSmall)
        {
            output.AddClass("small", HtmlEncoder.Default);
        }

        return base.ProcessAsync(context, output);
    }
}