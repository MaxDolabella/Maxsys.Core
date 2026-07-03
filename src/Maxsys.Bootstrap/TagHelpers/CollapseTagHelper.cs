using System.ComponentModel;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Conteúdo colapsável (<c>div.collapse</c>). Gera <c>id</c> aleatório quando não informado.
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/collapse/">docs</see>
/// </summary>
[HtmlTargetElement("bs-collapse")]
public class CollapseTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Quando <see langword="true"/>, colapsa horizontalmente (<c>collapse-horizontal</c>).
    /// O conteúdo filho deve ter largura definida (ex.: <c>style="width: 300px;"</c>).
    /// </summary>
    [HtmlAttributeName("horizontal")]
    public bool IsHorizontal { get; set; } = CollapseDefaults.IsHorizontal;

    /// <summary>
    /// Quando <see langword="true"/>, inicia expandido (<c>show</c>).
    /// Nesse caso, o trigger correspondente deve usar <c>expanded="true"</c>.
    /// </summary>
    [HtmlAttributeName("show")]
    public bool IsShow { get; set; } = CollapseDefaults.IsShow;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="collapse" id="collapseExample">
            ...
        </div>
        */
        var id = output.Attributes.TryGetAttribute("id", out var idAtt)
            ? idAtt.Value.ToString()!
            : $"clp-{Utils.GenerateRandomId()}";

        output.TagName = "div";
        output.Attributes.SetAttribute("id", id);

        output.AddClass("collapse", HtmlEncoder.Default);

        if (IsHorizontal)
        {
            output.AddClass("collapse-horizontal", HtmlEncoder.Default);
        }

        if (IsShow)
        {
            output.AddClass("show", HtmlEncoder.Default);
        }
    }
}

/// <summary>
/// Gatilho de collapse: <c>button</c> com <c>data-bs-toggle="collapse"</c> e <c>data-bs-target="#{target}"</c>;
/// quando <c>href</c> é informado, renderiza <c>a</c> com <c>href="#{id}"</c> e <c>role="button"</c>.
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/collapse/">docs</see>
/// </summary>
[HtmlTargetElement("bs-collapse-trigger")]
public class CollapseTriggerTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Id do <c>bs-collapse</c> alvo (sem <c>#</c>).
    /// </summary>
    [HtmlAttributeName("target")]
    public string? Target { get; set; }

    /// <summary>
    /// Quando informado, renderiza como <c>a</c> (âncora) em vez de <c>button</c>.
    /// Aceita o id com ou sem <c>#</c>.
    /// </summary>
    [HtmlAttributeName("href")]
    public string? Href { get; set; }

    /// <summary>
    /// Cor do botão (<c>btn-{color}</c>).
    /// </summary>
    public CollapseTriggerColors Color { get; set; } = CollapseTriggerDefaults.Color;

    /// <summary>
    /// Quando <see langword="true"/>, usa a variante outline (<c>btn-outline-{color}</c>).
    /// </summary>
    [HtmlAttributeName("outline")]
    public bool IsOutline { get; set; } = CollapseTriggerDefaults.IsOutline;

    /// <summary>
    /// Tamanho do botão (sm/lg).
    /// </summary>
    public CollapseTriggerSizes Size { get; set; } = CollapseTriggerDefaults.Size;

    /// <summary>
    /// Estado inicial de <c>aria-expanded</c>. Use <see langword="true"/> quando o collapse alvo tiver <c>show</c>.
    /// </summary>
    [HtmlAttributeName("expanded")]
    public bool IsExpanded { get; set; } = CollapseTriggerDefaults.IsExpanded;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <a class="btn btn-primary" data-bs-toggle="collapse" href="#collapseExample" role="button" aria-expanded="false" aria-controls="collapseExample">Link</a>
        <button class="btn btn-primary" type="button" data-bs-toggle="collapse" data-bs-target="#collapseExample" aria-expanded="false" aria-controls="collapseExample">Button</button>
        */
        var targetId = !string.IsNullOrWhiteSpace(Target)
            ? Target.TrimStart('#')
            : Href?.TrimStart('#');

        if (string.IsNullOrWhiteSpace(targetId))
        {
            throw new ArgumentException("Target (ou Href) é obrigatório em bs-collapse-trigger.");
        }

        if (!string.IsNullOrWhiteSpace(Href))
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", $"#{targetId}");
            output.Attributes.SetAttribute("role", "button");
        }
        else
        {
            output.TagName = "button";
            output.Attributes.SetAttribute("type", "button");
            output.Attributes.SetAttribute("data-bs-target", $"#{targetId}");
        }

        output.Attributes.SetAttribute("data-bs-toggle", "collapse");
        output.Attributes.SetAttribute("aria-expanded", IsExpanded ? "true" : "false");
        output.Attributes.SetAttribute("aria-controls", targetId);

        output.AddClass("btn", HtmlEncoder.Default);

        if (Color is not CollapseTriggerColors.None)
        {
            var color = Color.ToFriendlyName();
            output.AddClass(IsOutline ? $"btn-outline-{color}" : $"btn-{color}", HtmlEncoder.Default);
        }

        if (Size is not CollapseTriggerSizes.None)
        {
            output.AddClass(Size.ToFriendlyName(), HtmlEncoder.Default);
        }
    }
}

/// <summary>
/// Cores de botão para <see cref="CollapseTriggerTagHelper"/> (<c>btn-{color}</c> ou <c>btn-outline-{color}</c>).
/// </summary>
public enum CollapseTriggerColors : byte
{
    None = 0,

    [Description("primary")]
    Primary = 1,

    [Description("secondary")]
    Secondary = 2,

    [Description("success")]
    Success = 3,

    [Description("danger")]
    Danger = 4,

    [Description("warning")]
    Warning = 5,

    [Description("info")]
    Info = 6,

    [Description("light")]
    Light = 7,

    [Description("dark")]
    Dark = 8,

    [Description("link")]
    Link = 9,
}

/// <summary>
/// Tamanhos de botão para <see cref="CollapseTriggerTagHelper"/>.
/// </summary>
public enum CollapseTriggerSizes : byte
{
    None = 0,

    [Description("btn-sm")]
    Small = 1,

    [Description("btn-lg")]
    Large = 2,
}

public static class CollapseDefaults
{
    public static bool IsHorizontal = false;
    public static bool IsShow = false;
}

public static class CollapseTriggerDefaults
{
    public static CollapseTriggerColors Color = CollapseTriggerColors.Primary;
    public static bool IsOutline = false;
    public static CollapseTriggerSizes Size = CollapseTriggerSizes.None;
    public static bool IsExpanded = false;
}
