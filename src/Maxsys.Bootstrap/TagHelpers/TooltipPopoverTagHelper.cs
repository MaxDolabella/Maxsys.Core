using System.ComponentModel;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Tooltip via atributo global <c>bs-tooltip="texto"</c> em qualquer elemento — adiciona
/// <c>data-bs-toggle="tooltip"</c> e <c>data-bs-title</c>.
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/tooltips/">docs</see>
/// <para>
/// ATENÇÃO: tooltips exigem inicialização manual via JS (são opt-in por performance):
/// <code>
/// const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
/// [...tooltipTriggerList].map(el => new bootstrap.Tooltip(el));
/// </code>
/// </para>
/// </summary>
[HtmlTargetElement("*", Attributes = "bs-tooltip")]
public class TooltipTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Texto do tooltip (<c>data-bs-title</c>).
    /// </summary>
    [HtmlAttributeName("bs-tooltip")]
    public string? Title { get; set; }

    /// <summary>
    /// Posicionamento do tooltip (<c>data-bs-placement</c>).
    /// </summary>
    [HtmlAttributeName("bs-tooltip-placement")]
    public TooltipPlacements Placement { get; set; } = TooltipDefaults.Placement;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <button type="button" class="btn btn-secondary" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Tooltip on top">...</button>
        */
        output.Attributes.SetAttribute("data-bs-toggle", "tooltip");
        output.Attributes.SetAttribute("data-bs-title", Title ?? string.Empty);

        if (Placement is not TooltipPlacements.None)
        {
            output.Attributes.SetAttribute("data-bs-placement", Placement.ToFriendlyName());
        }
    }
}

/// <summary>
/// Popover via atributo global <c>bs-popover="conteúdo"</c> em qualquer elemento — adiciona
/// <c>data-bs-toggle="popover"</c>, <c>data-bs-content</c> e, opcionalmente,
/// <c>data-bs-title</c> (via <c>bs-popover-title</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/popovers/">docs</see>
/// <para>
/// ATENÇÃO: popovers exigem inicialização manual via JS (são opt-in por performance):
/// <code>
/// const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]');
/// [...popoverTriggerList].map(el => new bootstrap.Popover(el));
/// </code>
/// </para>
/// </summary>
[HtmlTargetElement("*", Attributes = "bs-popover")]
public class PopoverTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Conteúdo do popover (<c>data-bs-content</c>).
    /// </summary>
    [HtmlAttributeName("bs-popover")]
    public string? Content { get; set; }

    /// <summary>
    /// Título do popover (<c>data-bs-title</c>).
    /// </summary>
    [HtmlAttributeName("bs-popover-title")]
    public string? Title { get; set; } = PopoverDefaults.Title;

    /// <summary>
    /// Posicionamento do popover (<c>data-bs-placement</c>).
    /// </summary>
    [HtmlAttributeName("bs-popover-placement")]
    public PopoverPlacements Placement { get; set; } = PopoverDefaults.Placement;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <button type="button" class="btn btn-lg btn-danger" data-bs-toggle="popover" data-bs-title="Popover title" data-bs-content="And here's some amazing content.">...</button>
        */
        output.Attributes.SetAttribute("data-bs-toggle", "popover");
        output.Attributes.SetAttribute("data-bs-content", Content ?? string.Empty);

        if (!string.IsNullOrWhiteSpace(Title))
        {
            output.Attributes.SetAttribute("data-bs-title", Title);
        }

        if (Placement is not PopoverPlacements.None)
        {
            output.Attributes.SetAttribute("data-bs-placement", Placement.ToFriendlyName());
        }
    }
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Top"/></item>
/// <item>2.<see cref="Bottom"/></item>
/// <item>3.<see cref="Left"/></item>
/// <item>4.<see cref="Right"/></item>
/// </list>
/// </summary>
public enum TooltipPlacements : byte
{
    None = 0,

    [Description("top")]
    Top = 1,

    [Description("bottom")]
    Bottom = 2,

    [Description("left")]
    Left = 3,

    [Description("right")]
    Right = 4,
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Top"/></item>
/// <item>2.<see cref="Bottom"/></item>
/// <item>3.<see cref="Left"/></item>
/// <item>4.<see cref="Right"/></item>
/// </list>
/// </summary>
public enum PopoverPlacements : byte
{
    None = 0,

    [Description("top")]
    Top = 1,

    [Description("bottom")]
    Bottom = 2,

    [Description("left")]
    Left = 3,

    [Description("right")]
    Right = 4,
}

public static class TooltipDefaults
{
    public static TooltipPlacements Placement = TooltipPlacements.None;
}

public static class PopoverDefaults
{
    public static string? Title = null;
    public static PopoverPlacements Placement = PopoverPlacements.None;
}
