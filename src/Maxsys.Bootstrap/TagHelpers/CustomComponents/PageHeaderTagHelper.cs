using System.Text.Encodings.Web;
using Maxsys.SolutionScaffolder.MVC.Bootstrap.Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

[HtmlTargetElement(TAG)]
public class PageHeaderTagHelper : TagHelper,
    IBootstrapText,
    IBootstrapBackground
{
    internal const string TAG = "bsc-page-header";

    #region IBootstrapText

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = null;

    public FontWeights FontWeight { get; set; } = FontWeights.Light;
    public TextTransformations TextTransform { get; set; } = TextTransformations.None;
    public FontSizes TextSize { get; set; } = FontSizes.Size4;
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

    #endregion IBootstrapBackground

    public bool NoDivisor { get; set; } = false;
    public PageTitleAlignments Alignment { get; set; } = PageTitleAlignments.Start;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "section";
        output.TagMode = TagMode.StartTagAndEndTag;

        var (textPosition, justifyContents) = Alignment switch
        {
            //PageTitleAlignments.Start => (TextPositions.Start, JustifyContents.Start),
            PageTitleAlignments.Center => (TextPositions.Center, JustifyContents.Center),
            PageTitleAlignments.End => (TextPositions.End, JustifyContents.End),
            _ => (TextPositions.Start, JustifyContents.Start),
        };

        output.AddClass("row", HtmlEncoder.Default);
        output.AddClass(justifyContents.ToFriendlyName(), HtmlEncoder.Default);

        IBootstrapText.Apply(this, context, output);
        IBootstrapBackground.Apply(this, context, output);

        output.PreContent.AppendHtml($"<div class=\"col col-auto {textPosition.ToFriendlyName()}\">");
        output.PostContent.AppendHtml("</div>");
        if (!NoDivisor)
        {
            output.PostElement.AppendHtml("<hr />");
        }
    }
}

[HtmlTargetElement(TAG, ParentTag = PageHeaderTagHelper.TAG)]
public class PageHeaderTitleTagHelper : TagHelper,
    IBootstrapText,
    IBootstrapBackground
{
    internal const string TAG = "bsc-title";

    #region IBootstrapText

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = null;

    public FontWeights FontWeight { get; set; } = FontWeights.Light;
    public TextTransformations TextTransform { get; set; } = TextTransformations.None;
    public FontSizes TextSize { get; set; } = FontSizes.Size4;
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

    #endregion IBootstrapBackground

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "h1";
        output.TagMode = TagMode.StartTagAndEndTag;

        IBootstrapText.Apply(this, context, output);
        IBootstrapBackground.Apply(this, context, output);
    }
}

[HtmlTargetElement(TAG, ParentTag = PageHeaderTagHelper.TAG)]
public class PageHeaderSubTitleTagHelper : TagHelper,
    IBootstrapText,
    IBootstrapBackground
{
    internal const string TAG = "bsc-sub-title";

    #region IBootstrapText

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = null;

    public FontWeights FontWeight { get; set; } = FontWeights.Lighter;
    public TextTransformations TextTransform { get; set; } = TextTransformations.None;
    public FontSizes TextSize { get; set; } = FontSizes.Size5;
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

    #endregion IBootstrapBackground

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "h2";
        output.TagMode = TagMode.StartTagAndEndTag;

        IBootstrapText.Apply(this, context, output);
        IBootstrapBackground.Apply(this, context, output);
    }
}

public enum PageTitleAlignments : byte
{
    Center = 0,
    Start,
    End,
}