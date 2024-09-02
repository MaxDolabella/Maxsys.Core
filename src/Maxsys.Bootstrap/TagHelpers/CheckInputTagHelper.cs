using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

[HtmlTargetElement("bs-input-check", Attributes = "asp-for")]
[HtmlTargetElement("bs-input-switch", Attributes = "asp-for")]
public class CheckInputTagHelper : InputTagHelper
{
    public CheckInputTagHelper(IHtmlGenerator generator) : base(generator)
    { }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "input";
        output.TagMode = TagMode.SelfClosing;

        output.AddClass("form-check-input", HtmlEncoder.Default);

        base.Process(context, output);

        output.PostElement.Clear();
    }
}