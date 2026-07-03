using System.ComponentModel;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Lista de itens (list group), com suporte a flush, numeração e orientação horizontal.
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/list-group/">docs</see>
/// </summary>
[HtmlTargetElement("bs-list-group")]
public class ListGroupTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Remove bordas externas e cantos arredondados (<c>list-group-flush</c>).
    /// </summary>
    [HtmlAttributeName("flush")]
    public bool IsFlush { get; set; } = ListGroupDefaults.IsFlush;

    /// <summary>
    /// Lista numerada (<c>ol.list-group-numbered</c>).
    /// </summary>
    [HtmlAttributeName("numbered")]
    public bool IsNumbered { get; set; } = ListGroupDefaults.IsNumbered;

    /// <summary>
    /// Orientação horizontal (<c>list-group-horizontal</c>).
    /// </summary>
    [HtmlAttributeName("horizontal")]
    public bool IsHorizontal { get; set; } = ListGroupDefaults.IsHorizontal;

    /// <summary>
    /// Breakpoint a partir do qual a lista fica horizontal (<c>list-group-horizontal-{breakpoint}</c>). Implica horizontal.
    /// </summary>
    [HtmlAttributeName("breakpoint")]
    public ListGroupBreakpoints Breakpoint { get; set; } = ListGroupDefaults.Breakpoint;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <ul class="list-group">
          <li class="list-group-item">An item</li>
        </ul>
        */
        output.TagName = IsNumbered ? "ol" : "ul";
        output.AddClass("list-group", HtmlEncoder.Default);

        if (IsNumbered)
        {
            output.AddClass("list-group-numbered", HtmlEncoder.Default);
        }

        if (IsFlush)
        {
            output.AddClass("list-group-flush", HtmlEncoder.Default);
        }

        if (Breakpoint is not ListGroupBreakpoints.None)
        {
            output.AddClass(Breakpoint.ToFriendlyName(), HtmlEncoder.Default);
        }
        else if (IsHorizontal)
        {
            output.AddClass("list-group-horizontal", HtmlEncoder.Default);
        }
    }
}

/// <summary>
/// Item de list group. Com <c>action</c>, renderiza <c>&lt;a&gt;</c> (se houver <c>href</c>) ou <c>&lt;button&gt;</c>.
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/list-group/">docs</see>
/// </summary>
[HtmlTargetElement("bs-list-group-item", ParentTag = "bs-list-group")]
public class ListGroupItemTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Item ativo (classe <c>active</c> + <c>aria-current</c>).
    /// </summary>
    [HtmlAttributeName("active")]
    public bool IsActive { get; set; } = ListGroupItemDefaults.IsActive;

    /// <summary>
    /// Item desabilitado (classe <c>disabled</c> + <c>aria-disabled</c>).
    /// </summary>
    [HtmlAttributeName("disabled")]
    public bool IsDisabled { get; set; } = ListGroupItemDefaults.IsDisabled;

    /// <summary>
    /// Cor contextual do item (<c>list-group-item-{color}</c>).
    /// </summary>
    [HtmlAttributeName("variant")]
    public ListGroupItemVariants Variant { get; set; } = ListGroupItemDefaults.Variant;

    /// <summary>
    /// Item acionável (<c>list-group-item-action</c>): vira <c>&lt;a&gt;</c> quando há <c>href</c>, senão <c>&lt;button&gt;</c>.
    /// </summary>
    [HtmlAttributeName("action")]
    public bool IsAction { get; set; } = ListGroupItemDefaults.IsAction;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <li class="list-group-item">An item</li>
        <a href="#" class="list-group-item list-group-item-action">A link</a>
        <button type="button" class="list-group-item list-group-item-action">A button</button>
        */
        var hasHref = output.Attributes.ContainsName("href");
        var isButton = IsAction && !hasHref;

        output.TagName = IsAction
            ? (hasHref ? "a" : "button")
            : "li";

        output.AddClass("list-group-item", HtmlEncoder.Default);

        if (IsAction)
        {
            output.AddClass("list-group-item-action", HtmlEncoder.Default);
        }

        if (isButton)
        {
            output.Attributes.SetAttribute("type", "button");
        }

        if (IsActive)
        {
            output.AddClass("active", HtmlEncoder.Default);
            output.Attributes.SetAttribute("aria-current", "true");
        }

        if (IsDisabled)
        {
            if (isButton)
            {
                // Botões usam o atributo nativo disabled.
                output.Attributes.SetAttribute("disabled", "disabled");
            }
            else
            {
                output.AddClass("disabled", HtmlEncoder.Default);
            }

            output.Attributes.SetAttribute("aria-disabled", "true");
        }

        if (Variant is not ListGroupItemVariants.None)
        {
            output.AddClass(Variant.ToFriendlyName(), HtmlEncoder.Default);
        }
    }
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Small"/></item>
/// <item>2.<see cref="Medium"/></item>
/// <item>3.<see cref="Large"/></item>
/// <item>4.<see cref="ExtraLarge"/></item>
/// <item>5.<see cref="ExtraExtraLarge"/></item>
/// </list>
/// </summary>
public enum ListGroupBreakpoints : byte
{
    None = 0,

    [Description("list-group-horizontal-sm")]
    Small = 1,

    [Description("list-group-horizontal-md")]
    Medium = 2,

    [Description("list-group-horizontal-lg")]
    Large = 3,

    [Description("list-group-horizontal-xl")]
    ExtraLarge = 4,

    [Description("list-group-horizontal-xxl")]
    ExtraExtraLarge = 5,
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Primary"/></item>
/// <item>2.<see cref="Secondary"/></item>
/// <item>3.<see cref="Success"/></item>
/// <item>4.<see cref="Danger"/></item>
/// <item>5.<see cref="Warning"/></item>
/// <item>6.<see cref="Info"/></item>
/// <item>7.<see cref="Light"/></item>
/// <item>8.<see cref="Dark"/></item>
/// </list>
/// </summary>
public enum ListGroupItemVariants : byte
{
    None = 0,

    [Description("list-group-item-primary")]
    Primary = 1,

    [Description("list-group-item-secondary")]
    Secondary = 2,

    [Description("list-group-item-success")]
    Success = 3,

    [Description("list-group-item-danger")]
    Danger = 4,

    [Description("list-group-item-warning")]
    Warning = 5,

    [Description("list-group-item-info")]
    Info = 6,

    [Description("list-group-item-light")]
    Light = 7,

    [Description("list-group-item-dark")]
    Dark = 8,
}

public static class ListGroupDefaults
{
    public static bool IsFlush = false;
    public static bool IsNumbered = false;
    public static bool IsHorizontal = false;
    public static ListGroupBreakpoints Breakpoint = ListGroupBreakpoints.None;
}

public static class ListGroupItemDefaults
{
    public static bool IsActive = false;
    public static bool IsDisabled = false;
    public static ListGroupItemVariants Variant = ListGroupItemVariants.None;
    public static bool IsAction = false;
}
