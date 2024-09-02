using System.Text.Encodings.Web;
using Maxsys.Bootstrap.Interfaces;
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
    public string? CustomTextColor { get; set; } = PageHeaderDefaults.CustomTextColor;

    public FontWeights FontWeight { get; set; } = PageHeaderDefaults.FontWeight;
    public TextTransformations TextTransform { get; set; } = PageHeaderDefaults.TextTransform;
    public FontSizes TextSize { get; set; } = PageHeaderDefaults.TextSize;
    public TextColors TextColor { get; set; } = PageHeaderDefaults.TextColor;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = PageHeaderDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = PageHeaderDefaults.IsMonospace;

    #endregion IBootstrapText

    #region IBootstrapBackground

    public BackgroundColors BackgroundColor { get; set; } = PageHeaderDefaults.BackgroundColor;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; } = PageHeaderDefaults.CustomBackgroundColor;

    #endregion IBootstrapBackground

    public PageTitleAlignments Alignment { get; set; } = PageHeaderDefaults.Alignment;

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
    public string? CustomTextColor { get; set; } = PageHeaderTitleDefaults.CustomTextColor;

    public FontWeights FontWeight { get; set; } = PageHeaderTitleDefaults.FontWeight;
    public TextTransformations TextTransform { get; set; } = PageHeaderTitleDefaults.TextTransform;
    public FontSizes TextSize { get; set; } = PageHeaderTitleDefaults.TextSize;
    public TextColors TextColor { get; set; } = PageHeaderTitleDefaults.TextColor;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = PageHeaderTitleDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = PageHeaderTitleDefaults.IsMonospace;

    #endregion IBootstrapText

    #region IBootstrapBackground

    public BackgroundColors BackgroundColor { get; set; } = PageHeaderTitleDefaults.BackgroundColor;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; } = PageHeaderTitleDefaults.CustomBackgroundColor;

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
    public string? CustomTextColor { get; set; } = PageHeaderSubTitleDefaults.CustomTextColor;

    public FontWeights FontWeight { get; set; } = PageHeaderSubTitleDefaults.FontWeight;
    public TextTransformations TextTransform { get; set; } = PageHeaderSubTitleDefaults.TextTransform;
    public FontSizes TextSize { get; set; } = PageHeaderSubTitleDefaults.TextSize;
    public TextColors TextColor { get; set; } = PageHeaderSubTitleDefaults.TextColor;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = PageHeaderSubTitleDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = PageHeaderSubTitleDefaults.IsMonospace;

    #endregion IBootstrapText

    #region IBootstrapBackground

    public BackgroundColors BackgroundColor { get; set; } = PageHeaderSubTitleDefaults.BackgroundColor;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; } = PageHeaderSubTitleDefaults.CustomBackgroundColor;

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

public static class PageHeaderDefaults
{
    public static bool NoDivisor = false;
    public static PageTitleAlignments Alignment = PageTitleAlignments.Start;
    public static string? CustomTextColor = null;
    public static FontWeights FontWeight = FontWeights.Light;
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontSizes TextSize = FontSizes.Size4;
    public static TextColors TextColor = TextColors.None;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
    public static BackgroundColors BackgroundColor = BackgroundColors.None;
    public static string? CustomBackgroundColor = null;
}

public static class PageHeaderTitleDefaults
{
    public static string? CustomTextColor = null;
    public static FontWeights FontWeight = FontWeights.Light;
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontSizes TextSize = FontSizes.Size4;
    public static TextColors TextColor = TextColors.None;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
    public static BackgroundColors BackgroundColor = BackgroundColors.None;
    public static string? CustomBackgroundColor = null;
}

public static class PageHeaderSubTitleDefaults
{
    public static string? CustomTextColor = null;
    public static FontWeights FontWeight = FontWeights.Light;
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontSizes TextSize = FontSizes.Size5;
    public static TextColors TextColor = TextColors.None;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
    public static BackgroundColors BackgroundColor = BackgroundColors.None;
    public static string? CustomBackgroundColor = null;
}