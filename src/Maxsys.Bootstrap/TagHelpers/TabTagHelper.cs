using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

// https://www.red-gate.com/simple-talk/development/dotnet-development/asp-net-core-tag-helpers-bootstrap/
// https://stackoverflow.com/questions/44888510/getting-all-of-the-children-tag-helpers-in-a-parent-tag-helper-asp-net-core

#region Context

internal class TabTagContext
{
    public required string Id { get; set; }

    public List<TabItemTagContext> Items { get; set; } = [];
}

internal class TabItemTagContext
{
    public required int Order { get; set; }
    public required Guid ContextId { get; set; }
    public IHtmlContent Header { get; set; }
    public IHtmlContent Content { get; set; }
}

#endregion Context

/// <summary>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/navs-tabs/">docs</see>
/// </summary>
[HtmlTargetElement("bs-tab")]
public sealed class TabTagHelper : TagHelper
{
    private const string TAG = "bs-tab";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // Create the context to pass to child elements
        var tabContext = new TabTagContext
        {
            Id = output.Attributes.TryGetAttribute("id", out var idAtt)
                ? idAtt.Value.ToString()!
                : $"tab{Utils.GenerateRandomId()}"
        };
        context.Items[typeof(TabTagContext)] = tabContext;

        //if (idAtt != null)
        //{
        //    output.Attributes.Remove(context.AllAttributes["id"]);
        //}

        _ = output.GetChildContentAsync().Result;

        output.TagName = string.Empty;

        var sb = new StringBuilder();
        sb.AppendLine($"<ul id=\"{tabContext.Id}\" class=\"nav nav-tabs\" role=\"tablist\">");

        foreach (var item in tabContext.Items)
        {
            var clss = item.Order == 0 ? " active" : string.Empty;

            sb.AppendLine("<li class=\"nav-item\">");
            sb.AppendLine($"    <button class=\"nav-link{clss}\" id=\"tab-header-{tabContext.Id}-{item.Order:00}\" data-bs-toggle=\"tab\" data-bs-target=\"#tab-content-{tabContext.Id}-{item.Order:00}\" type=\"button\" role=\"tab\">");
            sb.Append(item.Header.GetContent());
            sb.AppendLine($"    </button>");
            sb.AppendLine("</li>");
        }

        sb.AppendLine("</ul>");

        sb.AppendLine($"<div class=\"tab-content\" id=\"tab-contents-{tabContext.Id}\">");

        foreach (var item in tabContext.Items)
        {
            var clss = item.Order == 0 ? " show active" : string.Empty;

            sb.AppendLine($"<div class=\"tab-pane{clss}\" id=\"tab-content-{tabContext.Id}-{item.Order:00}\" role=\"tabpanel\" tabindex=\"0\">");
            sb.Append(item.Content.GetContent());
            sb.AppendLine("</div>");
        }

        sb.AppendLine("</div>");

        output.Content.AppendHtml(sb.ToString());
    }
}

[HtmlTargetElement("bs-tab-item", ParentTag = "bs-tab")]
public sealed class TabItemTagHelper : TagHelper
{
    private const string TAG = "bs-tab-item";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var tabContext = context.Items[typeof(TabTagContext)] as TabTagContext;
        ArgumentNullException.ThrowIfNull(tabContext);

        var currentContext = new TabItemTagContext
        {
            Order = tabContext.Items.Count,
            ContextId = Guid.NewGuid()
        };
        context.Items[typeof(TabItemTagContext)] = currentContext;

        tabContext.Items.Add(currentContext);

        output.TagName = string.Empty;
    }
}

[HtmlTargetElement("bs-tab-item-header", ParentTag = "bs-tab-item")]
public sealed class TabItemHeaderTagHelper : TagHelper
{
    private const string TAG = "bs-tab-item-header";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var tabContext = context.Items[typeof(TabTagContext)] as TabTagContext;
        ArgumentNullException.ThrowIfNull(tabContext);

        var tabItemContext = context.Items[typeof(TabItemTagContext)] as TabItemTagContext;
        ArgumentNullException.ThrowIfNull(tabItemContext);

        var sb = new StringBuilder("<div");

        if (context.AllAttributes.TryGetAttributes("class", out var attributes))
        {
            var classes = attributes.Select(x => x.Value);
            sb.AppendFormat(" class=\"{0}\"", string.Join(' ', classes));
        }

        sb.AppendLine(">");

        sb.AppendLine(output.GetChildContentAsync().Result.GetContent().Trim());
        sb.AppendLine("</div>");

        tabItemContext.Header = new DefaultTagHelperContent().AppendHtml(sb.ToString());

        output.TagName = string.Empty;
    }
}

[HtmlTargetElement("bs-tab-item-content", ParentTag = "bs-tab-item")]
public sealed class TabItemContentTagHelper : TagHelper
{
    private const string TAG = "bs-tab-item-content";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var tabContext = context.Items[typeof(TabTagContext)] as TabTagContext;
        ArgumentNullException.ThrowIfNull(tabContext);

        var tabItemContext = context.Items[typeof(TabItemTagContext)] as TabItemTagContext;
        ArgumentNullException.ThrowIfNull(tabItemContext);

        var sb = new StringBuilder("<div");

        if (context.AllAttributes.TryGetAttributes("class", out var attributes))
        {
            var classes = attributes.Select(x => x.Value);
            sb.AppendFormat(" class=\"{0}\"", string.Join(' ', classes));
        }

        sb.AppendLine(">");

        sb.AppendLine(output.GetChildContentAsync().Result.GetContent().Trim());
        sb.AppendLine("</div>");

        tabItemContext.Content = new DefaultTagHelperContent().AppendHtml(sb.ToString());

        output.TagName = string.Empty;
    }
}