using System.Text.Encodings.Web;
using Maxsys.SolutionScaffolder.MVC.Bootstrap.Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

[HtmlTargetElement("bs-breadcrumb")]
public class BreadcrumbTagHelper : TagHelper
    , IBootstrapText
{
    #region Props

    private const string DEFAULT_DIVIDER = "/";
    public string Divider { get; set; } = DEFAULT_DIVIDER;

    #region IBootstrapText

    [HtmlAttributeName("transform")] public TextTransformations TextTransform { get; set; } = TextTransformations.None;
    [HtmlAttributeName("size")] public FontSizes TextSize { get; set; } = FontSizes.None;
    [HtmlAttributeName("weight")] public FontWeights FontWeight { get; set; } = FontWeights.None;
    [HtmlAttributeName("color")] public TextColors TextColor { get; set; } = TextColors.None;
    [HtmlAttributeName("custom-color")] public string? CustomTextColor { get; set; }
    [HtmlAttributeName("small")] public bool IsSmall { get; set; } = false;
    [HtmlAttributeName("italic")] public bool IsItalic { get; set; } = false;
    [HtmlAttributeName("monospace")] public bool IsMonospace { get; set; } = false;

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

        if (Divider != DEFAULT_DIVIDER)
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
    public bool IsActive { get; set; } = false;

    #region IBootstrapText

    [HtmlAttributeName("transform")] public TextTransformations TextTransform { get; set; } = TextTransformations.None;
    [HtmlAttributeName("size")] public FontSizes TextSize { get; set; } = FontSizes.None;
    [HtmlAttributeName("weight")] public FontWeights FontWeight { get; set; } = FontWeights.None;
    [HtmlAttributeName("color")] public TextColors TextColor { get; set; } = TextColors.None;
    [HtmlAttributeName("custom-color")] public string? CustomTextColor { get; set; }
    [HtmlAttributeName("small")] public bool IsSmall { get; set; } = false;
    [HtmlAttributeName("italic")] public bool IsItalic { get; set; } = false;
    [HtmlAttributeName("monospace")] public bool IsMonospace { get; set; } = false;

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