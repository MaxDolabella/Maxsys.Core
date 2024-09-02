using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

[HtmlTargetElement(COMMON_LABEL, Attributes = "asp-for")]
[HtmlTargetElement(CHECK_LABEL, Attributes = "asp-for")]
public class FormLabelTagHelper : Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper
{
    #region Consts

    private const string COMMON_LABEL = "bs-form-label";
    private const string CHECK_LABEL = "bs-form-check-label";

    #endregion Consts

    #region Props

    public FontSizes Size { get; set; } = FormLabelDefaults.Size;
    public TextColors Color { get; set; } = FormLabelDefaults.Color;

    [HtmlAttributeName("custom-color")]
    public string? CustomColor { get; set; } = FormLabelDefaults.CustomColor;

    [HtmlAttributeName("small")]
    public bool IsSmall { get; set; } = FormLabelDefaults.IsSmall;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = FormLabelDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = FormLabelDefaults.IsMonospace;

    #endregion Props

    public FormLabelTagHelper(IHtmlGenerator generator) : base(generator)
    { }

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

        if (!string.IsNullOrWhiteSpace(CustomColor))
        {
            output.Attributes.Add("style", $"color:{CustomColor};");
        }
        else if (Color is not TextColors.None)
        {
            output.AddClass(Color.ToFriendlyName(), HtmlEncoder.Default);
        }

        if (Size is not FontSizes.None)
        {
            output.AddClass(Size.ToFriendlyName(), HtmlEncoder.Default);
        }

        // small
        if (IsSmall)
        {
            output.AddClass("small", HtmlEncoder.Default);
        }

        // italic
        if (IsItalic)
        {
            output.AddClass("fst-italic", HtmlEncoder.Default);
        }

        // monospace
        if (IsMonospace)
        {
            output.AddClass("font-monospace", HtmlEncoder.Default);
        }

        return base.ProcessAsync(context, output);
    }
}

public static class FormLabelDefaults
{
    public static FontSizes Size = FontSizes.None;
    public static TextColors Color = TextColors.None;
    public static string? CustomColor = null;
    public static bool IsSmall = false;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
}