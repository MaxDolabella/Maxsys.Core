using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

// https://www.red-gate.com/simple-talk/development/dotnet-development/asp-net-core-tag-helpers-bootstrap/
// https://stackoverflow.com/questions/44888510/getting-all-of-the-children-tag-helpers-in-a-parent-tag-helper-asp-net-core

#region Context

internal class AccordionTagContext
{
    public required string Id { get; set; }
    public required bool IsAwayOpen { get; set; }
}

internal class AccordionItemTagContext
{
    public required AccordionTagContext AccordionContext { get; set; }
    public required string Id { get; set; }

    public string GetTargetId() => $"{Id}-collapse";
}

#endregion Context

/// <summary>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/accordion/">docs</see>
/// </summary>
[HtmlTargetElement("bs-accordion")]
public sealed class AccordionTagHelper : TagHelper
{
    #region Props

    [HtmlAttributeName("flush")]
    public bool IsFlush { get; set; } = AccordionDefaults.IsFlush;

    [HtmlAttributeName("always-open")]
    public bool IsAwaysOpen { get; set; } = AccordionDefaults.IsAwaysOpen;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // Create the context to pass to child elements
        var accordionContext = new AccordionTagContext
        {
            Id = output.Attributes.TryGetAttribute("id", out var idAtt)
                ? idAtt.Value.ToString()!
                : $"acd-{Utils.GenerateRandomId()}",
            IsAwayOpen = IsAwaysOpen
        };
        context.Items[typeof(AccordionTagContext)] = accordionContext;

        if (idAtt != null)
        {
            output.Attributes.Remove(context.AllAttributes["id"]);
        }

        output.TagName = "div";
        output.Attributes.SetAttribute("id", accordionContext.Id);

        output.AddClass("accordion", HtmlEncoder.Default);

        if (IsFlush)
        {
            output.AddClass("accordion-flush", HtmlEncoder.Default);
        }
    }
}

[HtmlTargetElement("bs-accordion-item", ParentTag = "bs-accordion")]
public sealed class AccordionItemTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var accordionContext = context.Items[typeof(AccordionTagContext)] as AccordionTagContext;
        ArgumentNullException.ThrowIfNull(accordionContext);

        var accordionItemTagContext = new AccordionItemTagContext
        {
            AccordionContext = accordionContext,
            Id = output.Attributes.TryGetAttribute("id", out var idAtt)
                ? idAtt.Value.ToString()!
                : $"{accordionContext.Id}-aci-{Utils.GenerateRandomId()}"
        };
        context.Items[typeof(AccordionItemTagContext)] = accordionItemTagContext;

        output.TagName = "div";
        output.AddClass("accordion-item", HtmlEncoder.Default);
    }
}

[HtmlTargetElement("bs-accordion-header", ParentTag = "bs-accordion-item")]
public sealed class AccordionHeaderTagHelper : TagHelper
{
    /*
    <div class="accordion-header">
        <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#_accordioId-itemId">
            Accordion Item #1
        </button>
    </div>
    */

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var accordionItemTagContext = context.Items[typeof(AccordionItemTagContext)] as AccordionItemTagContext;
        ArgumentNullException.ThrowIfNull(accordionItemTagContext);

        output.TagName = string.Empty;

        string @class = string.Empty;
        string style = string.Empty;
        if (context.AllAttributes.ContainsName("class"))
        {
            @class = context.AllAttributes["class"]?.Value.ToString()!;
            output.Attributes.Remove(context.AllAttributes["class"]);
        }
        if (context.AllAttributes.ContainsName("style"))
        {
            style = $"style=\"{context.AllAttributes["style"]?.Value.ToString()!}\"";
            output.Attributes.Remove(context.AllAttributes["style"]);
        }

        var sb = new StringBuilder();
        sb.AppendFormat("<div class=\"accordion-header\">\r\n");
        sb.AppendFormat($"  <button class=\"accordion-button collapsed {@class}\" {style} type=\"button\" data-bs-toggle=\"collapse\" data-bs-target=\"#{accordionItemTagContext.GetTargetId()}\">\r\n");
        output.PreContent.SetHtmlContent(sb.ToString());

        sb = new StringBuilder();
        sb.AppendFormat("   </button>\r\n");
        sb.AppendFormat("</div>\r\n");
        output.PostContent.SetHtmlContent(sb.ToString());
    }
}

[HtmlTargetElement("bs-accordion-body", ParentTag = "bs-accordion-item")]
public sealed class AccordionBodyTagHelper : TagHelper
{
    /*
    <div id="_accordioId-itemId" class="accordion-collapse collapse show" data-bs-parent="#_accordioId">
        <div class="accordion-body @class">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
        </div>
    </div>
    */

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var itemContext = context.Items[typeof(AccordionItemTagContext)] as AccordionItemTagContext;
        ArgumentNullException.ThrowIfNull(itemContext);

        output.TagName = string.Empty;

        string @class = string.Empty;
        string style = string.Empty;
        if (context.AllAttributes.ContainsName("class"))
        {
            @class = context.AllAttributes["class"]?.Value.ToString()!;
            output.Attributes.Remove(context.AllAttributes["class"]);
        }
        if (context.AllAttributes.ContainsName("style"))
        {
            style = $"style=\"{context.AllAttributes["style"]?.Value.ToString()!}\"";
            output.Attributes.Remove(context.AllAttributes["style"]);
        }

        var dataBsParent = itemContext.AccordionContext.IsAwayOpen ? string.Empty : $"data-bs-parent=\"#{itemContext.AccordionContext.Id}\"";

        var sb = new StringBuilder();
        sb.AppendFormat($"<div id=\"{itemContext.GetTargetId()}\" class=\"accordion-collapse collapse\" {dataBsParent}>\r\n");
        sb.AppendFormat($"  <div class=\"accordion-body {@class}\" {style}>\r\n");
        output.PreContent.SetHtmlContent(sb.ToString());

        sb = new StringBuilder();
        sb.AppendFormat("   </div>\r\n");
        sb.AppendFormat("</div>\r\n");
        output.PostContent.SetHtmlContent(sb.ToString());
    }
}

public static class AccordionDefaults
{
    public static bool IsFlush { get; set; } = false;
    public static bool IsAwaysOpen { get; set; } = false;
}