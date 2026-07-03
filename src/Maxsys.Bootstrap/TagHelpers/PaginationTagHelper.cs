using System.ComponentModel;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Paginação (<c>nav &gt; ul.pagination</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/pagination/">docs</see>
/// </summary>
[HtmlTargetElement("bs-pagination")]
public class PaginationTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Texto do <c>aria-label</c> do <c>nav</c> (acessibilidade).
    /// </summary>
    [HtmlAttributeName("label")]
    public string Label { get; set; } = PaginationDefaults.Label;

    /// <summary>
    /// Tamanho da paginação (sm/lg).
    /// </summary>
    public PaginationSizes Size { get; set; } = PaginationDefaults.Size;

    /// <summary>
    /// Alinhamento horizontal (<c>justify-content-*</c>).
    /// </summary>
    public PaginationJustifications Justify { get; set; } = PaginationDefaults.Justify;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <nav aria-label="Page navigation">
          <ul class="pagination">
            <li class="page-item"><a class="page-link" href="#">1</a></li>
          </ul>
        </nav>
        */
        output.TagName = "nav";
        output.Attributes.SetAttribute("aria-label", Label);

        var classes = "pagination";

        if (Size is not PaginationSizes.None)
        {
            classes += $" {Size.ToFriendlyName()}";
        }

        if (Justify is not PaginationJustifications.None)
        {
            classes += $" {Justify.ToFriendlyName()}";
        }

        output.PreContent.AppendHtml($"<ul class=\"{classes}\">");
        output.PostContent.AppendHtml("</ul>");
    }
}

/// <summary>
/// Item da paginação: <c>li.page-item &gt; a.page-link</c> quando há <c>href</c> e não está desabilitado;
/// caso contrário, <c>span.page-link</c>.
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/pagination/">docs</see>
/// </summary>
[HtmlTargetElement("bs-page-item", ParentTag = "bs-pagination")]
public class PageItemTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Endereço do link. Quando ausente (ou desabilitado), renderiza <c>span.page-link</c>.
    /// </summary>
    [HtmlAttributeName("href")]
    public string? Href { get; set; }

    /// <summary>
    /// Quando <see langword="true"/>, marca a página atual (<c>active</c> + <c>aria-current="page"</c>).
    /// </summary>
    [HtmlAttributeName("active")]
    public bool IsActive { get; set; } = PageItemDefaults.IsActive;

    /// <summary>
    /// Quando <see langword="true"/>, desabilita o item.
    /// </summary>
    [HtmlAttributeName("disabled")]
    public bool IsDisabled { get; set; } = PageItemDefaults.IsDisabled;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <li class="page-item active" aria-current="page"><a class="page-link" href="#">1</a></li>
        <li class="page-item disabled"><span class="page-link">Previous</span></li>
        */
        output.TagName = "li";
        output.AddClass("page-item", HtmlEncoder.Default);

        if (IsActive)
        {
            output.AddClass("active", HtmlEncoder.Default);
            output.Attributes.SetAttribute("aria-current", "page");
        }

        if (IsDisabled)
        {
            output.AddClass("disabled", HtmlEncoder.Default);
        }

        if (!IsDisabled && !string.IsNullOrWhiteSpace(Href))
        {
            output.PreContent.AppendHtml($"<a class=\"page-link\" href=\"{HtmlEncoder.Default.Encode(Href)}\">");
            output.PostContent.AppendHtml("</a>");
        }
        else
        {
            output.PreContent.AppendHtml("<span class=\"page-link\">");
            output.PostContent.AppendHtml("</span>");
        }
    }
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Small"/></item>
/// <item>2.<see cref="Large"/></item>
/// </list>
/// </summary>
public enum PaginationSizes : byte
{
    None = 0,

    [Description("pagination-sm")]
    Small = 1,

    [Description("pagination-lg")]
    Large = 2,
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Start"/></item>
/// <item>2.<see cref="Center"/></item>
/// <item>3.<see cref="End"/></item>
/// </list>
/// </summary>
public enum PaginationJustifications : byte
{
    None = 0,

    [Description("justify-content-start")]
    Start = 1,

    [Description("justify-content-center")]
    Center = 2,

    [Description("justify-content-end")]
    End = 3,
}

public static class PaginationDefaults
{
    public static string Label = "Page navigation";
    public static PaginationSizes Size = PaginationSizes.None;
    public static PaginationJustifications Justify = PaginationJustifications.None;
}

public static class PageItemDefaults
{
    public static bool IsActive = false;
    public static bool IsDisabled = false;
}
