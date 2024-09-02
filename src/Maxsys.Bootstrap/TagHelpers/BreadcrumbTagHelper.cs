using System.Text.Encodings.Web;
using Maxsys.Bootstrap.Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

[HtmlTargetElement("bs-breadcrumb")]
public class BreadcrumbTagHelper : TagHelper
    , IBootstrapText
{
    #region Props

    public string Divider { get; set; } = BreadcrumbDefaults.Divider;

    #region IBootstrapText

    public TextTransformations TextTransform { get; set; } = BreadcrumbDefaults.TextTransform;
    public FontSizes TextSize { get; set; } = BreadcrumbDefaults.TextSize;
    public FontWeights FontWeight { get; set; } = BreadcrumbDefaults.FontWeight;
    public TextColors TextColor { get; set; } = BreadcrumbDefaults.TextColor;
    [HtmlAttributeName("custom-color")] public string? CustomTextColor { get; set; } = BreadcrumbDefaults.CustomTextColor;
    [HtmlAttributeName("small")] public bool IsSmall { get; set; } = BreadcrumbDefaults.IsSmall;
    [HtmlAttributeName("italic")] public bool IsItalic { get; set; } = BreadcrumbDefaults.IsItalic;
    [HtmlAttributeName("monospace")] public bool IsMonospace { get; set; } = BreadcrumbDefaults.IsMonospace;

    #endregion IBootstrapText

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <nav class="fw-light fs-5" style="--bs-breadcrumb-divider: '>';" >
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="#">Project</a></li>
                <li class="breadcrumb-item"><a href="#">Enum</a></li>
                <li class="breadcrumb-item active">List</li>
            </ol>
        </nav>
        */

        output.TagName = "nav";

        output.PreContent.AppendHtml("<ol class=\"breadcrumb\">");
        output.PostContent.AppendHtml("</ol>");

        if (Divider != BreadcrumbDefaults.DEFAULT_DIVIDER)
        {
            output.Attributes.SetAttribute("style", $"--bs-breadcrumb-divider: '{Divider}';");
        }

        IBootstrapText.Apply(this, context, output);
    }
}

[HtmlTargetElement("bs-breadcrumb-item", ParentTag = "bs-breadcrumb")]
public class BreadcrumbItemTagHelper : TagHelper
    , IBootstrapText
{
    #region Props

    [HtmlAttributeName("active")]
    public bool IsActive { get; set; } = BreadcrumbItemDefaults.IsActive;

    #region IBootstrapText

    public TextTransformations TextTransform { get; set; } = BreadcrumbItemDefaults.TextTransform;
    public FontSizes TextSize { get; set; } = BreadcrumbItemDefaults.TextSize;
    public FontWeights FontWeight { get; set; } = BreadcrumbItemDefaults.FontWeight;
    public TextColors TextColor { get; set; } = BreadcrumbItemDefaults.TextColor;
    [HtmlAttributeName("custom-color")] public string? CustomTextColor { get; set; } = BreadcrumbItemDefaults.CustomTextColor;
    [HtmlAttributeName("small")] public bool IsSmall { get; set; } = BreadcrumbItemDefaults.IsSmall;
    [HtmlAttributeName("italic")] public bool IsItalic { get; set; } = BreadcrumbItemDefaults.IsItalic;
    [HtmlAttributeName("monospace")] public bool IsMonospace { get; set; } = BreadcrumbItemDefaults.IsMonospace;

    #endregion IBootstrapText

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
            <li class="breadcrumb-item"><a href="#">Project</a></li>
            <li class="breadcrumb-item active">List</li>
        */

        output.TagName = "li";
        output.AddClass("breadcrumb-item", HtmlEncoder.Default);

        // active
        if (IsActive)
        {
            output.AddClass("active", HtmlEncoder.Default);
        }

        IBootstrapText.Apply(this, context, output);
    }
}

public static class BreadcrumbDefaults
{
    public const string DEFAULT_DIVIDER = "/";
    
    public static string Divider = DEFAULT_DIVIDER;
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontSizes TextSize = FontSizes.None;
    public static FontWeights FontWeight = FontWeights.None;
    public static TextColors TextColor = TextColors.None;
    public static string? CustomTextColor = null;
    public static bool IsSmall = false;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
}

public static class BreadcrumbItemDefaults
{
    public static bool IsActive = false;
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontSizes TextSize = FontSizes.None;
    public static FontWeights FontWeight = FontWeights.None;
    public static TextColors TextColor = TextColors.None;
    public static string? CustomTextColor = null;
    public static bool IsSmall = false;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
}
