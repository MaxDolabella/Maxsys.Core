using System.ComponentModel;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Maxsys.Bootstrap.ViewComponents;

/// <summary>
/// Paginação <i>data-driven</i>: recebe página atual/total e um template de URL e renderiza
/// a paginação completa (anterior/próxima, janela de páginas com reticências).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/pagination/">docs</see>
/// </summary>
/// <remarks>
/// Uso (requer <c>AddMaxsysBootstrap()</c> no MVC):
/// <code>
/// &lt;vc:bs-pagination current-page="3" total-pages="12" url-format="?page={0}" /&gt;
/// </code>
/// Para markup manual, use o TagHelper <c>&lt;bs-pagination&gt;</c>.
/// </remarks>
public class BsPaginationViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(
        int currentPage,
        int totalPages,
        string urlFormat,
        string? label = null,
        PaginationViewSizes? size = null,
        JustifyContents? justify = null,
        int? maxVisiblePages = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(urlFormat, nameof(urlFormat));

        var encoder = HtmlEncoder.Default;

        label ??= BsPaginationViewDefaults.Label;
        var sizeValue = size ?? BsPaginationViewDefaults.Size;
        var justifyValue = justify ?? BsPaginationViewDefaults.Justify;
        var window = Math.Max(3, maxVisiblePages ?? BsPaginationViewDefaults.MaxVisiblePages);

        currentPage = Math.Clamp(currentPage, 1, Math.Max(1, totalPages));

        var ulClasses = "pagination";
        if (sizeValue is not PaginationViewSizes.None)
            ulClasses += $" {sizeValue.ToFriendlyName()}";
        if (justifyValue is not JustifyContents.None)
            ulClasses += $" {justifyValue.ToFriendlyName()}";

        var sb = new StringBuilder();
        sb.Append($"<nav aria-label=\"{encoder.Encode(label)}\">");
        sb.Append($"<ul class=\"{ulClasses}\">");

        // « anterior
        AppendPageItem(sb, encoder, urlFormat, currentPage - 1, "&laquo;", isDisabled: currentPage <= 1, isActive: false, ariaLabel: "Anterior");

        // janela de páginas com reticências
        foreach (var page in GetWindow(currentPage, totalPages, window))
        {
            if (page is null)
                sb.Append("<li class=\"page-item disabled\"><span class=\"page-link\">&hellip;</span></li>");
            else
                AppendPageItem(sb, encoder, urlFormat, page.Value, page.Value.ToString(), isDisabled: false, isActive: page.Value == currentPage, ariaLabel: null);
        }

        // próxima »
        AppendPageItem(sb, encoder, urlFormat, currentPage + 1, "&raquo;", isDisabled: currentPage >= totalPages, isActive: false, ariaLabel: "Próxima");

        sb.Append("</ul></nav>");

        return new HtmlContentViewComponentResult(new HtmlString(sb.ToString()));
    }

    private static void AppendPageItem(StringBuilder sb, HtmlEncoder encoder, string urlFormat, int page, string content, bool isDisabled, bool isActive, string? ariaLabel)
    {
        var liClass = "page-item" + (isActive ? " active" : string.Empty) + (isDisabled ? " disabled" : string.Empty);
        var aria = (isActive ? " aria-current=\"page\"" : string.Empty)
                 + (ariaLabel is not null ? $" aria-label=\"{encoder.Encode(ariaLabel)}\"" : string.Empty);

        sb.Append($"<li class=\"{liClass}\">");

        if (isDisabled)
        {
            sb.Append($"<span class=\"page-link\"{aria}>{content}</span>");
        }
        else
        {
            var url = encoder.Encode(string.Format(urlFormat, page));
            sb.Append($"<a class=\"page-link\" href=\"{url}\"{aria}>{content}</a>");
        }

        sb.Append("</li>");
    }

    /// <summary>
    /// Retorna a janela de páginas a exibir. <see langword="null"/> representa reticências.
    /// Sempre inclui a primeira e a última página.
    /// </summary>
    private static IEnumerable<int?> GetWindow(int current, int total, int maxVisible)
    {
        if (total <= maxVisible)
        {
            for (var i = 1; i <= total; i++)
                yield return i;
            yield break;
        }

        // núcleo da janela em torno da página atual (descontando primeira, última e possíveis reticências)
        var innerSize = maxVisible - 2;
        var start = Math.Max(2, current - innerSize / 2);
        var end = Math.Min(total - 1, start + innerSize - 1);
        start = Math.Max(2, end - innerSize + 1);

        yield return 1;

        if (start > 2)
            yield return null;

        for (var i = start; i <= end; i++)
            yield return i;

        if (end < total - 1)
            yield return null;

        yield return total;
    }
}

/// <summary>
/// Tamanhos da paginação.
/// </summary>
public enum PaginationViewSizes : byte
{
    None = 0,

    [Description("pagination-sm")]
    Small = 1,

    [Description("pagination-lg")]
    Large = 2,
}

public static class BsPaginationViewDefaults
{
    public static string Label = "Navegação de páginas";
    public static PaginationViewSizes Size = PaginationViewSizes.None;
    public static JustifyContents Justify = JustifyContents.None;
    public static int MaxVisiblePages = 7;
}
