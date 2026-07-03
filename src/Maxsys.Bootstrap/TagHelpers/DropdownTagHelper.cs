using System.ComponentModel;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Container de dropdown (menu suspenso).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/dropdowns/">docs</see>
/// </summary>
[HtmlTargetElement("bs-dropdown")]
public class DropdownTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Direção de abertura do menu (dropup, dropend, dropstart, centered).
    /// </summary>
    public DropdownDirections Direction { get; set; } = DropdownDefaults.Direction;

    /// <summary>
    /// Quando <see langword="true"/>, aplica <c>btn-group</c> (uso em grupos de botões/split buttons).
    /// </summary>
    [HtmlAttributeName("btn-group")]
    public bool IsButtonGroup { get; set; } = DropdownDefaults.IsButtonGroup;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="dropdown">
          <button class="btn btn-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">...</button>
          <ul class="dropdown-menu">...</ul>
        </div>
        */
        output.TagName = "div";

        if (IsButtonGroup)
        {
            output.AddClass("btn-group", HtmlEncoder.Default);
        }

        if (Direction is not DropdownDirections.None)
        {
            foreach (var item in Direction.ToFriendlyName()!.Split(' '))
            {
                output.AddClass(item, HtmlEncoder.Default);
            }
        }
        else if (!IsButtonGroup)
        {
            output.AddClass("dropdown", HtmlEncoder.Default);
        }
    }
}

/// <summary>
/// Botão que abre o dropdown (via <c>data-bs-toggle="dropdown"</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/dropdowns/">docs</see>
/// </summary>
[HtmlTargetElement("bs-dropdown-toggle", ParentTag = "bs-dropdown")]
public class DropdownToggleTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Cor do botão (<c>btn-{color}</c>).
    /// </summary>
    public DropdownToggleColors Color { get; set; } = DropdownToggleDefaults.Color;

    /// <summary>
    /// Quando <see langword="true"/>, usa a variante outline (<c>btn-outline-{color}</c>).
    /// </summary>
    [HtmlAttributeName("outline")]
    public bool IsOutline { get; set; } = DropdownToggleDefaults.IsOutline;

    /// <summary>
    /// Tamanho do botão (sm/lg).
    /// </summary>
    public DropdownToggleSizes Size { get; set; } = DropdownToggleDefaults.Size;

    /// <summary>
    /// Quando <see langword="true"/>, renderiza um split toggle (<c>dropdown-toggle-split</c>)
    /// com <c>span.visually-hidden</c> interno.
    /// </summary>
    [HtmlAttributeName("split")]
    public bool IsSplit { get; set; } = DropdownToggleDefaults.IsSplit;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <button class="btn btn-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
            Dropdown button
        </button>
        */
        output.TagName = "button";
        output.Attributes.SetAttribute("type", "button");
        output.Attributes.SetAttribute("data-bs-toggle", "dropdown");
        output.Attributes.SetAttribute("aria-expanded", "false");

        output.AddClass("btn", HtmlEncoder.Default);
        output.AddClass("dropdown-toggle", HtmlEncoder.Default);

        if (Color is not DropdownToggleColors.None)
        {
            var color = Color.ToFriendlyName();
            output.AddClass(IsOutline ? $"btn-outline-{color}" : $"btn-{color}", HtmlEncoder.Default);
        }

        if (Size is not DropdownToggleSizes.None)
        {
            output.AddClass(Size.ToFriendlyName(), HtmlEncoder.Default);
        }

        if (IsSplit)
        {
            output.AddClass("dropdown-toggle-split", HtmlEncoder.Default);
            output.PostContent.AppendHtml("<span class=\"visually-hidden\">Toggle Dropdown</span>");
        }
    }
}

/// <summary>
/// Menu do dropdown (<c>ul.dropdown-menu</c>). No Bootstrap 5.3, <c>dropdown-menu-dark</c> foi removido:
/// o modo escuro é aplicado via <c>data-bs-theme="dark"</c> (flag <c>dark</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/dropdowns/">docs</see>
/// </summary>
[HtmlTargetElement("bs-dropdown-menu", ParentTag = "bs-dropdown")]
public class DropdownMenuTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Quando <see langword="true"/>, aplica <c>data-bs-theme="dark"</c> ao menu.
    /// </summary>
    [HtmlAttributeName("dark")]
    public bool IsDark { get; set; } = DropdownMenuDefaults.IsDark;

    /// <summary>
    /// Quando <see langword="true"/>, alinha o menu à direita (<c>dropdown-menu-end</c>).
    /// </summary>
    [HtmlAttributeName("end")]
    public bool IsEnd { get; set; } = DropdownMenuDefaults.IsEnd;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <ul class="dropdown-menu">
            ...
        </ul>
        */
        output.TagName = "ul";
        output.AddClass("dropdown-menu", HtmlEncoder.Default);

        if (IsEnd)
        {
            output.AddClass("dropdown-menu-end", HtmlEncoder.Default);
        }

        if (IsDark)
        {
            output.Attributes.SetAttribute("data-bs-theme", "dark");
        }
    }
}

/// <summary>
/// Item do dropdown: renderiza <c>li &gt; a.dropdown-item</c> quando <c>href</c> é informado,
/// ou <c>li &gt; button.dropdown-item</c> caso contrário.
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/dropdowns/">docs</see>
/// </summary>
[HtmlTargetElement("bs-dropdown-item", ParentTag = "bs-dropdown-menu")]
public class DropdownItemTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Endereço do link. Quando ausente, o item é renderizado como <c>button</c>.
    /// </summary>
    [HtmlAttributeName("href")]
    public string? Href { get; set; }

    /// <summary>
    /// Quando <see langword="true"/>, marca o item como ativo (<c>active</c> + <c>aria-current="true"</c>).
    /// </summary>
    [HtmlAttributeName("active")]
    public bool IsActive { get; set; } = DropdownItemDefaults.IsActive;

    /// <summary>
    /// Quando <see langword="true"/>, desabilita o item.
    /// </summary>
    [HtmlAttributeName("disabled")]
    public bool IsDisabled { get; set; } = DropdownItemDefaults.IsDisabled;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <li><a class="dropdown-item" href="#">Action</a></li>
        <li><button class="dropdown-item" type="button">Action</button></li>
        */
        output.TagName = "li";

        var classes = "dropdown-item";
        if (IsActive)
        {
            classes += " active";
        }
        if (IsDisabled)
        {
            classes += " disabled";
        }

        var ariaCurrent = IsActive ? " aria-current=\"true\"" : string.Empty;

        if (!string.IsNullOrWhiteSpace(Href))
        {
            var ariaDisabled = IsDisabled ? " aria-disabled=\"true\" tabindex=\"-1\"" : string.Empty;
            output.PreContent.AppendHtml($"<a class=\"{classes}\" href=\"{HtmlEncoder.Default.Encode(Href)}\"{ariaCurrent}{ariaDisabled}>");
            output.PostContent.AppendHtml("</a>");
        }
        else
        {
            var disabled = IsDisabled ? " disabled" : string.Empty;
            output.PreContent.AppendHtml($"<button class=\"{classes}\" type=\"button\"{ariaCurrent}{disabled}>");
            output.PostContent.AppendHtml("</button>");
        }
    }
}

/// <summary>
/// Divisor do dropdown (<c>li &gt; hr.dropdown-divider</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/dropdowns/">docs</see>
/// </summary>
[HtmlTargetElement("bs-dropdown-divider", ParentTag = "bs-dropdown-menu", TagStructure = TagStructure.WithoutEndTag)]
public class DropdownDividerTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <li><hr class="dropdown-divider"></li>
        */
        output.TagName = "li";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Content.SetHtmlContent("<hr class=\"dropdown-divider\">");
    }
}

/// <summary>
/// Cabeçalho de seção do dropdown (<c>li &gt; h6.dropdown-header</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/dropdowns/">docs</see>
/// </summary>
[HtmlTargetElement("bs-dropdown-header", ParentTag = "bs-dropdown-menu")]
public class DropdownHeaderTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <li><h6 class="dropdown-header">Dropdown header</h6></li>
        */
        output.TagName = "li";
        output.PreContent.AppendHtml("<h6 class=\"dropdown-header\">");
        output.PostContent.AppendHtml("</h6>");
    }
}

/// <summary>
/// Texto não interativo dentro do dropdown (<c>li &gt; span.dropdown-item-text</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/dropdowns/">docs</see>
/// </summary>
[HtmlTargetElement("bs-dropdown-text", ParentTag = "bs-dropdown-menu")]
public class DropdownTextTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <li><span class="dropdown-item-text">Dropdown item text</span></li>
        */
        output.TagName = "li";
        output.PreContent.AppendHtml("<span class=\"dropdown-item-text\">");
        output.PostContent.AppendHtml("</span>");
    }
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Dropup"/></item>
/// <item>2.<see cref="Dropend"/></item>
/// <item>3.<see cref="Dropstart"/></item>
/// <item>4.<see cref="DropupCenter"/></item>
/// <item>5.<see cref="DropdownCenter"/></item>
/// </list>
/// </summary>
public enum DropdownDirections : byte
{
    None = 0,

    [Description("dropup")]
    Dropup = 1,

    [Description("dropend")]
    Dropend = 2,

    [Description("dropstart")]
    Dropstart = 3,

    [Description("dropup-center dropup")]
    DropupCenter = 4,

    [Description("dropdown-center")]
    DropdownCenter = 5,
}

/// <summary>
/// Cores de botão para <see cref="DropdownToggleTagHelper"/> (<c>btn-{color}</c> ou <c>btn-outline-{color}</c>).
/// </summary>
public enum DropdownToggleColors : byte
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
/// Tamanhos de botão para <see cref="DropdownToggleTagHelper"/>.
/// </summary>
public enum DropdownToggleSizes : byte
{
    None = 0,

    [Description("btn-sm")]
    Small = 1,

    [Description("btn-lg")]
    Large = 2,
}

public static class DropdownDefaults
{
    public static DropdownDirections Direction = DropdownDirections.None;
    public static bool IsButtonGroup = false;
}

public static class DropdownToggleDefaults
{
    public static DropdownToggleColors Color = DropdownToggleColors.Secondary;
    public static bool IsOutline = false;
    public static DropdownToggleSizes Size = DropdownToggleSizes.None;
    public static bool IsSplit = false;
}

public static class DropdownMenuDefaults
{
    public static bool IsDark = false;
    public static bool IsEnd = false;
}

public static class DropdownItemDefaults
{
    public static bool IsActive = false;
    public static bool IsDisabled = false;
}
