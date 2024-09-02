using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

[HtmlTargetElement(FORM_FLOATING)]
[HtmlTargetElement(FORM_INPUT_GROUP)]
[HtmlTargetElement(FORM_INPUT_GROUP_SM)]
[HtmlTargetElement(FORM_INPUT_GROUP_LG)]
[HtmlTargetElement(FORM_CHECK)]
[HtmlTargetElement(FORM_SWITCH)]
public class FormGroupTagHelper : TagHelper
{
    #region Consts

    private const string FORM_FLOATING = "bs-form-floating"; // form-floating
    private const string FORM_INPUT_GROUP = "bs-form-input-group"; // input-group
    private const string FORM_INPUT_GROUP_SM = "bs-form-input-group-sm"; // input-group input-group-sm
    private const string FORM_INPUT_GROUP_LG = "bs-form-input-group-lg"; // input-group input-group-lg
    private const string FORM_CHECK = "bs-form-check"; // form-check
    private const string FORM_SWITCH = "bs-form-switch"; // form-check form-switch

    #endregion Consts

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";

        if (context.TagName == FORM_FLOATING)
        {
            output.AddClass("form-floating", HtmlEncoder.Default);
        }
        else if (context.TagName.StartsWith(FORM_INPUT_GROUP))
        {
            output.AddClass("input-group", HtmlEncoder.Default);

            if (context.TagName == FORM_INPUT_GROUP_SM)
            {
                output.AddClass("input-group-sm", HtmlEncoder.Default);
            }
            else if (context.TagName == FORM_INPUT_GROUP_LG)
            {
                output.AddClass("input-group-lg", HtmlEncoder.Default);
            }
        }
        else if (context.TagName == FORM_CHECK)
        {
            output.AddClass("form-check", HtmlEncoder.Default);
        }
        else if (context.TagName == FORM_SWITCH)
        {
            output.AddClass("form-check", HtmlEncoder.Default);
            output.AddClass("form-switch", HtmlEncoder.Default);
        }
    }
}