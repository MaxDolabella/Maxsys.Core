using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/helpers/stacks/">docs</see>
/// </summary>
[HtmlTargetElement(V_STACK)]
[HtmlTargetElement(H_STACK)]
public class StackTagHelper : TagHelper
{
    #region Consts

    private const string V_STACK = "bs-vstack";
    private const string H_STACK = "bs-hstack";

    #endregion Consts

    #region Props

    [Range(0, 5)]
    public int Gap { get; set; } = 0;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // <div class="vstack gap-3">
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var @class = context.TagName == V_STACK ? "vstack" : "hstack";
        output.AddClass(@class, HtmlEncoder.Default);

        var gap = Gap is < 0 or > 5 ? 0 : Gap;
        output.AddClass($"gap-{gap}", HtmlEncoder.Default);
    }
}