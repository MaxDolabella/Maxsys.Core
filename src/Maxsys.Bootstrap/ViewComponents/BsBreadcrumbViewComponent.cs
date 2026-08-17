using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Maxsys.Bootstrap.ViewComponents;

/// <summary>
/// Breadcrumb <i>data-driven</i>: recebe a lista de itens e renderiza a trilha completa
/// (último item marcado como ativo/<c>aria-current</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/breadcrumb/">docs</see>
/// </summary>
/// <remarks>
/// Uso (requer <c>AddMaxsysBootstrap()</c> no MVC):
/// <code>
/// &lt;vc:bs-breadcrumb items="Model.Trail" /&gt;
/// // onde Trail é IEnumerable&lt;BreadcrumbItem&gt;:
/// new BreadcrumbItem[] { new("Home", "/"), new("Biblioteca", "/lib"), new("Dados") }
/// </code>
/// Para markup manual, use o TagHelper <c>&lt;bs-breadcrumb&gt;</c>.
/// </remarks>
public class BsBreadcrumbViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IEnumerable<BreadcrumbItem> items, string? divider = null)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        var encoder = HtmlEncoder.Default;
        var itemList = items.ToList();

        var sb = new StringBuilder();

        // divider customizado via CSS var (ex.: ">")
        divider ??= BsBreadcrumbViewDefaults.Divider;
        var style = divider is not null
            ? $" style=\"--bs-breadcrumb-divider: '{encoder.Encode(divider)}';\""
            : string.Empty;

        sb.Append($"<nav aria-label=\"breadcrumb\"{style}>");
        sb.Append("<ol class=\"breadcrumb\">");

        for (var i = 0; i < itemList.Count; i++)
        {
            var item = itemList[i];
            var isLast = i == itemList.Count - 1;
            var text = encoder.Encode(item.Text);

            if (isLast || string.IsNullOrWhiteSpace(item.Url))
            {
                var ariaCurrent = isLast ? " aria-current=\"page\"" : string.Empty;
                sb.Append($"<li class=\"breadcrumb-item active\"{ariaCurrent}>{text}</li>");
            }
            else
            {
                sb.Append($"<li class=\"breadcrumb-item\"><a href=\"{encoder.Encode(item.Url!)}\">{text}</a></li>");
            }
        }

        sb.Append("</ol></nav>");

        return new HtmlContentViewComponentResult(new HtmlString(sb.ToString()));
    }
}

/// <summary>
/// Item de breadcrumb: texto e URL opcional (itens sem URL — tipicamente o último — são renderizados como ativos).
/// </summary>
/// <param name="Text">Texto exibido.</param>
/// <param name="Url">URL do link. <see langword="null"/> para item sem link (ativo).</param>
public sealed record BreadcrumbItem(string Text, string? Url = null);

public static class BsBreadcrumbViewDefaults
{
    /// <summary>Divider customizado (CSS var <c>--bs-breadcrumb-divider</c>). <see langword="null"/> usa o padrão do Bootstrap (/).</summary>
    public static string? Divider = null;
}
