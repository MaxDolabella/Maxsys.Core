using System.ComponentModel;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Diálogo modal. Gera as camadas <c>modal-dialog</c>/<c>modal-content</c> automaticamente.
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/modal/">docs</see>
/// </summary>
[HtmlTargetElement("bs-modal")]
public class ModalTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Habilita a animação de fade (classe <c>fade</c>).
    /// </summary>
    [HtmlAttributeName("fade")]
    public bool Fade { get; set; } = ModalDefaults.Fade;

    /// <summary>
    /// Tamanho do modal (<c>modal-sm</c>/<c>modal-lg</c>/<c>modal-xl</c>).
    /// </summary>
    [HtmlAttributeName("size")]
    public ModalSizes Size { get; set; } = ModalDefaults.Size;

    /// <summary>
    /// Modo fullscreen (<c>modal-fullscreen[-{breakpoint}-down]</c>). Tem precedência sobre <see cref="Size"/>.
    /// </summary>
    [HtmlAttributeName("fullscreen")]
    public ModalFullscreenModes Fullscreen { get; set; } = ModalDefaults.Fullscreen;

    /// <summary>
    /// Centraliza verticalmente (<c>modal-dialog-centered</c>).
    /// </summary>
    [HtmlAttributeName("centered")]
    public bool IsCentered { get; set; } = ModalDefaults.IsCentered;

    /// <summary>
    /// Corpo rolável (<c>modal-dialog-scrollable</c>).
    /// </summary>
    [HtmlAttributeName("scrollable")]
    public bool IsScrollable { get; set; } = ModalDefaults.IsScrollable;

    /// <summary>
    /// Backdrop estático: não fecha ao clicar fora nem com Esc (<c>data-bs-backdrop="static"</c> + <c>data-bs-keyboard="false"</c>).
    /// </summary>
    [HtmlAttributeName("static-backdrop")]
    public bool StaticBackdrop { get; set; } = ModalDefaults.StaticBackdrop;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="modal fade" id="myModal" tabindex="-1" aria-hidden="true">
          <div class="modal-dialog">
            <div class="modal-content">
              ...
            </div>
          </div>
        </div>
        */
        var id = output.Attributes.TryGetAttribute("id", out var idAtt)
            ? idAtt.Value.ToString()!
            : $"mdl-{Utils.GenerateRandomId()}";

        output.TagName = "div";
        output.Attributes.SetAttribute("id", id);
        output.AddClass("modal", HtmlEncoder.Default);

        if (Fade)
        {
            output.AddClass("fade", HtmlEncoder.Default);
        }

        output.Attributes.SetAttribute("tabindex", "-1");
        output.Attributes.SetAttribute("aria-hidden", "true");

        if (StaticBackdrop)
        {
            output.Attributes.SetAttribute("data-bs-backdrop", "static");
            output.Attributes.SetAttribute("data-bs-keyboard", "false");
        }

        var dialogClasses = new List<string> { "modal-dialog" };

        if (Fullscreen is not ModalFullscreenModes.None)
        {
            dialogClasses.Add(Fullscreen.ToFriendlyName()!);
        }
        else if (Size is not ModalSizes.None)
        {
            dialogClasses.Add(Size.ToFriendlyName()!);
        }

        if (IsCentered)
        {
            dialogClasses.Add("modal-dialog-centered");
        }

        if (IsScrollable)
        {
            dialogClasses.Add("modal-dialog-scrollable");
        }

        output.PreContent.AppendHtml($"<div class=\"{string.Join(' ', dialogClasses)}\"><div class=\"modal-content\">");
        output.PostContent.AppendHtml("</div></div>");
    }
}

/// <summary>
/// Cabeçalho do modal (<c>div.modal-header</c>), com título opcional e botão de fechar.
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/modal/">docs</see>
/// </summary>
[HtmlTargetElement("bs-modal-header", ParentTag = "bs-modal")]
public class ModalHeaderTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Título opcional, renderizado como <c>h1.modal-title.fs-5</c>.
    /// </summary>
    [HtmlAttributeName("title")]
    public string? Title { get; set; } = ModalHeaderDefaults.Title;

    /// <summary>
    /// Exibe o botão de fechar (<c>btn-close</c> com <c>data-bs-dismiss="modal"</c>).
    /// </summary>
    [HtmlAttributeName("closeable")]
    public bool Closeable { get; set; } = ModalHeaderDefaults.Closeable;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="modal-header">
          <h1 class="modal-title fs-5">Modal title</h1>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        */
        output.TagName = "div";
        output.AddClass("modal-header", HtmlEncoder.Default);

        if (!string.IsNullOrWhiteSpace(Title))
        {
            output.PreContent
                .AppendHtml("<h1 class=\"modal-title fs-5\">")
                .Append(Title)
                .AppendHtml("</h1>");
        }

        if (Closeable)
        {
            output.PostContent.AppendHtml("<button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"modal\" aria-label=\"Close\"></button>");
        }
    }
}

/// <summary>
/// Corpo do modal (<c>div.modal-body</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/modal/">docs</see>
/// </summary>
[HtmlTargetElement("bs-modal-body", ParentTag = "bs-modal")]
public class ModalBodyTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.AddClass("modal-body", HtmlEncoder.Default);
    }
}

/// <summary>
/// Rodapé do modal (<c>div.modal-footer</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/modal/">docs</see>
/// </summary>
[HtmlTargetElement("bs-modal-footer", ParentTag = "bs-modal")]
public class ModalFooterTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.AddClass("modal-footer", HtmlEncoder.Default);
    }
}

/// <summary>
/// Botão que abre um modal via <c>data-bs-toggle="modal"</c>. <c>target</c> é o id do modal (obrigatório).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/modal/">docs</see>
/// </summary>
[HtmlTargetElement("bs-modal-trigger")]
public class ModalTriggerTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Id do modal alvo (obrigatório). Aceita com ou sem <c>#</c>.
    /// </summary>
    [HtmlAttributeName("target")]
    public string Target { get; set; } = ModalTriggerDefaults.Target;

    /// <summary>
    /// Aparência do botão (<c>btn-{color}</c>).
    /// </summary>
    [HtmlAttributeName("variant")]
    public ButtonVariants Variant { get; set; } = ModalTriggerDefaults.Variant;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#myModal">
          Launch modal
        </button>
        */
        if (string.IsNullOrWhiteSpace(Target))
        {
            throw new InvalidOperationException("bs-modal-trigger: o atributo 'target' é obrigatório (id do modal).");
        }

        output.TagName = "button";
        output.Attributes.SetAttribute("type", "button");
        output.AddClass("btn", HtmlEncoder.Default);

        if (Variant is not ButtonVariants.None)
        {
            output.AddClass(Variant.ToFriendlyName(), HtmlEncoder.Default);
        }

        output.Attributes.SetAttribute("data-bs-toggle", "modal");
        output.Attributes.SetAttribute("data-bs-target", $"#{Target.TrimStart('#')}");
    }
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Small"/></item>
/// <item>2.<see cref="Large"/></item>
/// <item>3.<see cref="ExtraLarge"/></item>
/// </list>
/// </summary>
public enum ModalSizes : byte
{
    None = 0,

    [Description("modal-sm")]
    Small = 1,

    [Description("modal-lg")]
    Large = 2,

    [Description("modal-xl")]
    ExtraLarge = 3,
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Always"/></item>
/// <item>2.<see cref="SmallDown"/></item>
/// <item>3.<see cref="MediumDown"/></item>
/// <item>4.<see cref="LargeDown"/></item>
/// <item>5.<see cref="ExtraLargeDown"/></item>
/// <item>6.<see cref="ExtraExtraLargeDown"/></item>
/// </list>
/// </summary>
public enum ModalFullscreenModes : byte
{
    None = 0,

    [Description("modal-fullscreen")]
    Always = 1,

    [Description("modal-fullscreen-sm-down")]
    SmallDown = 2,

    [Description("modal-fullscreen-md-down")]
    MediumDown = 3,

    [Description("modal-fullscreen-lg-down")]
    LargeDown = 4,

    [Description("modal-fullscreen-xl-down")]
    ExtraLargeDown = 5,

    [Description("modal-fullscreen-xxl-down")]
    ExtraExtraLargeDown = 6,
}

public static class ModalDefaults
{
    public static bool Fade = true;
    public static ModalSizes Size = ModalSizes.None;
    public static ModalFullscreenModes Fullscreen = ModalFullscreenModes.None;
    public static bool IsCentered = false;
    public static bool IsScrollable = false;
    public static bool StaticBackdrop = false;
}

public static class ModalHeaderDefaults
{
    public static string? Title = null;
    public static bool Closeable = true;
}

public static class ModalTriggerDefaults
{
    public static string Target = string.Empty;
    public static ButtonVariants Variant = ButtonVariants.Primary;
}
