using System.ComponentModel;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Painel lateral offcanvas, com <c>id</c> gerado quando ausente.
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/offcanvas/">docs</see>
/// </summary>
[HtmlTargetElement("bs-offcanvas")]
public class OffcanvasTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Lado da tela onde o offcanvas aparece (<c>offcanvas-{placement}</c>).
    /// </summary>
    [HtmlAttributeName("placement")]
    public OffcanvasPlacements Placement { get; set; } = OffcanvasDefaults.Placement;

    /// <summary>
    /// Backdrop estático: não fecha ao clicar fora (<c>data-bs-backdrop="static"</c>).
    /// </summary>
    [HtmlAttributeName("static-backdrop")]
    public bool StaticBackdrop { get; set; } = OffcanvasDefaults.StaticBackdrop;

    /// <summary>
    /// Permite rolagem do <c>&lt;body&gt;</c> com o offcanvas aberto (<c>data-bs-scroll="true"</c>).
    /// </summary>
    [HtmlAttributeName("body-scroll")]
    public bool BodyScroll { get; set; } = OffcanvasDefaults.BodyScroll;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="offcanvas offcanvas-start" tabindex="-1" id="myOffcanvas">
          ...
        </div>
        */
        var id = output.Attributes.TryGetAttribute("id", out var idAtt)
            ? idAtt.Value.ToString()!
            : $"ofc-{Utils.GenerateRandomId()}";

        output.TagName = "div";
        output.Attributes.SetAttribute("id", id);
        output.AddClass("offcanvas", HtmlEncoder.Default);
        output.AddClass(Placement.ToFriendlyName(), HtmlEncoder.Default);
        output.Attributes.SetAttribute("tabindex", "-1");

        if (StaticBackdrop)
        {
            output.Attributes.SetAttribute("data-bs-backdrop", "static");
        }

        if (BodyScroll)
        {
            output.Attributes.SetAttribute("data-bs-scroll", "true");
        }
    }
}

/// <summary>
/// Cabeçalho do offcanvas (<c>div.offcanvas-header</c>), com título opcional e botão de fechar.
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/offcanvas/">docs</see>
/// </summary>
[HtmlTargetElement("bs-offcanvas-header", ParentTag = "bs-offcanvas")]
public class OffcanvasHeaderTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Título opcional, renderizado como <c>h5.offcanvas-title</c>.
    /// </summary>
    [HtmlAttributeName("title")]
    public string? Title { get; set; } = OffcanvasHeaderDefaults.Title;

    /// <summary>
    /// Exibe o botão de fechar (<c>btn-close</c> com <c>data-bs-dismiss="offcanvas"</c>).
    /// </summary>
    [HtmlAttributeName("closeable")]
    public bool Closeable { get; set; } = OffcanvasHeaderDefaults.Closeable;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="offcanvas-header">
          <h5 class="offcanvas-title">Offcanvas</h5>
          <button type="button" class="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
        </div>
        */
        output.TagName = "div";
        output.AddClass("offcanvas-header", HtmlEncoder.Default);

        if (!string.IsNullOrWhiteSpace(Title))
        {
            output.PreContent
                .AppendHtml("<h5 class=\"offcanvas-title\">")
                .Append(Title)
                .AppendHtml("</h5>");
        }

        if (Closeable)
        {
            output.PostContent.AppendHtml("<button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"offcanvas\" aria-label=\"Close\"></button>");
        }
    }
}

/// <summary>
/// Corpo do offcanvas (<c>div.offcanvas-body</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/offcanvas/">docs</see>
/// </summary>
[HtmlTargetElement("bs-offcanvas-body", ParentTag = "bs-offcanvas")]
public class OffcanvasBodyTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.AddClass("offcanvas-body", HtmlEncoder.Default);
    }
}

/// <summary>
/// Botão que abre um offcanvas via <c>data-bs-toggle="offcanvas"</c>. <c>target</c> é o id do offcanvas (obrigatório).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/offcanvas/">docs</see>
/// </summary>
[HtmlTargetElement("bs-offcanvas-trigger")]
public class OffcanvasTriggerTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Id do offcanvas alvo (obrigatório). Aceita com ou sem <c>#</c>.
    /// </summary>
    [HtmlAttributeName("target")]
    public string Target { get; set; } = OffcanvasTriggerDefaults.Target;

    /// <summary>
    /// Aparência do botão (<c>btn-{color}</c>).
    /// </summary>
    [HtmlAttributeName("variant")]
    public ButtonVariants Variant { get; set; } = OffcanvasTriggerDefaults.Variant;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <button type="button" class="btn btn-primary" data-bs-toggle="offcanvas" data-bs-target="#myOffcanvas" aria-controls="myOffcanvas">
          Open offcanvas
        </button>
        */
        if (string.IsNullOrWhiteSpace(Target))
        {
            throw new InvalidOperationException("bs-offcanvas-trigger: o atributo 'target' é obrigatório (id do offcanvas).");
        }

        var targetId = Target.TrimStart('#');

        output.TagName = "button";
        output.Attributes.SetAttribute("type", "button");
        output.AddClass("btn", HtmlEncoder.Default);

        if (Variant is not ButtonVariants.None)
        {
            output.AddClass(Variant.ToFriendlyName(), HtmlEncoder.Default);
        }

        output.Attributes.SetAttribute("data-bs-toggle", "offcanvas");
        output.Attributes.SetAttribute("data-bs-target", $"#{targetId}");
        output.Attributes.SetAttribute("aria-controls", targetId);
    }
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Start"/></item>
/// <item>2.<see cref="End"/></item>
/// <item>3.<see cref="Top"/></item>
/// <item>4.<see cref="Bottom"/></item>
/// </list>
/// </summary>
public enum OffcanvasPlacements : byte
{
    [Description("offcanvas-start")]
    Start = 1,

    [Description("offcanvas-end")]
    End = 2,

    [Description("offcanvas-top")]
    Top = 3,

    [Description("offcanvas-bottom")]
    Bottom = 4,
}

public static class OffcanvasDefaults
{
    public static OffcanvasPlacements Placement = OffcanvasPlacements.Start;
    public static bool StaticBackdrop = false;
    public static bool BodyScroll = false;
}

public static class OffcanvasHeaderDefaults
{
    public static string? Title = null;
    public static bool Closeable = true;
}

public static class OffcanvasTriggerDefaults
{
    public static string Target = string.Empty;
    public static ButtonVariants Variant = ButtonVariants.Primary;
}
