using System.ComponentModel;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Container posicionador de toasts (<c>div.toast-container</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/toasts/">docs</see>
/// </summary>
[HtmlTargetElement("bs-toast-container")]
public class ToastContainerTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Posição fixa do container na tela (<c>position-fixed</c> + utilitários de posicionamento).
    /// </summary>
    [HtmlAttributeName("position")]
    public ToastContainerPositions Position { get; set; } = ToastContainerDefaults.Position;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="toast-container position-fixed bottom-0 end-0 p-3">
          ...
        </div>
        */
        output.TagName = "div";
        output.AddClass("toast-container", HtmlEncoder.Default);
        output.AddClass("p-3", HtmlEncoder.Default);

        if (Position is not ToastContainerPositions.None)
        {
            output.AddClass("position-fixed", HtmlEncoder.Default);

            foreach (var cssClass in Position.ToFriendlyName()!.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                output.AddClass(cssClass, HtmlEncoder.Default);
            }
        }
    }
}

/// <summary>
/// Notificação toast (<c>div.toast</c>), com <c>id</c> gerado quando ausente.
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/toasts/">docs</see>
/// </summary>
[HtmlTargetElement("bs-toast")]
public class ToastTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Oculta o toast automaticamente após o delay (<c>data-bs-autohide</c>).
    /// </summary>
    [HtmlAttributeName("autohide")]
    public bool Autohide { get; set; } = ToastDefaults.Autohide;

    /// <summary>
    /// Tempo em milissegundos até ocultar o toast (<c>data-bs-delay</c>).
    /// </summary>
    [HtmlAttributeName("delay")]
    public int? Delay { get; set; } = ToastDefaults.Delay;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="toast" role="alert" aria-live="assertive" aria-atomic="true">
          ...
        </div>
        */
        var id = output.Attributes.TryGetAttribute("id", out var idAtt)
            ? idAtt.Value.ToString()!
            : $"tst-{Utils.GenerateRandomId()}";

        output.TagName = "div";
        output.Attributes.SetAttribute("id", id);
        output.AddClass("toast", HtmlEncoder.Default);
        output.Attributes.SetAttribute("role", "alert");
        output.Attributes.SetAttribute("aria-live", "assertive");
        output.Attributes.SetAttribute("aria-atomic", "true");

        if (Autohide != ToastDefaults.AUTOHIDE_DEFAULT)
        {
            output.Attributes.SetAttribute("data-bs-autohide", Autohide ? "true" : "false");
        }

        if (Delay.HasValue)
        {
            output.Attributes.SetAttribute("data-bs-delay", Delay.Value.ToString());
        }
    }
}

/// <summary>
/// Cabeçalho do toast (<c>div.toast-header</c>), com título opcional e botão de fechar.
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/toasts/">docs</see>
/// </summary>
[HtmlTargetElement("bs-toast-header", ParentTag = "bs-toast")]
public class ToastHeaderTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Título opcional, renderizado como <c>strong.me-auto</c>.
    /// </summary>
    [HtmlAttributeName("title")]
    public string? Title { get; set; } = ToastHeaderDefaults.Title;

    /// <summary>
    /// Exibe o botão de fechar (<c>btn-close</c> com <c>data-bs-dismiss="toast"</c>).
    /// </summary>
    [HtmlAttributeName("closeable")]
    public bool Closeable { get; set; } = ToastHeaderDefaults.Closeable;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="toast-header">
          <strong class="me-auto">Bootstrap</strong>
          <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
        </div>
        */
        output.TagName = "div";
        output.AddClass("toast-header", HtmlEncoder.Default);

        if (!string.IsNullOrWhiteSpace(Title))
        {
            output.PreContent
                .AppendHtml("<strong class=\"me-auto\">")
                .Append(Title)
                .AppendHtml("</strong>");
        }

        if (Closeable)
        {
            output.PostContent.AppendHtml("<button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"toast\" aria-label=\"Close\"></button>");
        }
    }
}

/// <summary>
/// Corpo do toast (<c>div.toast-body</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/toasts/">docs</see>
/// </summary>
[HtmlTargetElement("bs-toast-body", ParentTag = "bs-toast")]
public class ToastBodyTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.AddClass("toast-body", HtmlEncoder.Default);
    }
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="TopLeft"/></item>
/// <item>2.<see cref="TopCenter"/></item>
/// <item>3.<see cref="TopRight"/></item>
/// <item>4.<see cref="MiddleLeft"/></item>
/// <item>5.<see cref="MiddleCenter"/></item>
/// <item>6.<see cref="MiddleRight"/></item>
/// <item>7.<see cref="BottomLeft"/></item>
/// <item>8.<see cref="BottomCenter"/></item>
/// <item>9.<see cref="BottomRight"/></item>
/// </list>
/// </summary>
public enum ToastContainerPositions : byte
{
    None = 0,

    [Description("top-0 start-0")]
    TopLeft = 1,

    [Description("top-0 start-50 translate-middle-x")]
    TopCenter = 2,

    [Description("top-0 end-0")]
    TopRight = 3,

    [Description("top-50 start-0 translate-middle-y")]
    MiddleLeft = 4,

    [Description("top-50 start-50 translate-middle")]
    MiddleCenter = 5,

    [Description("top-50 end-0 translate-middle-y")]
    MiddleRight = 6,

    [Description("bottom-0 start-0")]
    BottomLeft = 7,

    [Description("bottom-0 start-50 translate-middle-x")]
    BottomCenter = 8,

    [Description("bottom-0 end-0")]
    BottomRight = 9,
}

public static class ToastContainerDefaults
{
    public static ToastContainerPositions Position = ToastContainerPositions.None;
}

public static class ToastDefaults
{
    /// <summary>
    /// Default do Bootstrap para <c>data-bs-autohide</c>; o atributo só é emitido quando difere deste valor.
    /// </summary>
    public const bool AUTOHIDE_DEFAULT = true;

    public static bool Autohide = AUTOHIDE_DEFAULT;
    public static int? Delay = null;
}

public static class ToastHeaderDefaults
{
    public static string? Title = null;
    public static bool Closeable = true;
}
