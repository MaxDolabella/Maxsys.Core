using System.ComponentModel;
using System.Text.Encodings.Web;
using Maxsys.Bootstrap.Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Botão Bootstrap. Renderiza <c>&lt;button&gt;</c> ou <c>&lt;a role="button"&gt;</c> quando <c>href</c> é informado.<br/>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/buttons/">docs</see>
/// </summary>
[HtmlTargetElement("bs-button")]
public class ButtonTagHelper : TagHelper,
    IBootstrapBackground,
    IBootstrapText
{
    #region IBootstrapBackground

    [HtmlAttributeNotBound]
    public BackgroundColors BackgroundColor { get; set; } = BackgroundColors.None;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; } = ButtonDefaults.CustomBackgroundColor;

    #endregion IBootstrapBackground

    #region IBootstrapText

    public TextTransformations TextTransform { get; set; } = ButtonDefaults.TextTransform;

    public FontWeights FontWeight { get; set; } = ButtonDefaults.FontWeight;

    public FontSizes TextSize { get; set; } = ButtonDefaults.TextSize;

    public TextColors TextColor { get; set; } = ButtonDefaults.TextColor;

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = ButtonDefaults.CustomTextColor;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = ButtonDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = ButtonDefaults.IsMonospace;

    #endregion IBootstrapText

    #region Props

    public ButtonVariants Variant { get; set; } = ButtonDefaults.Variant;

    [HtmlAttributeName("outline")]
    public bool IsOutline { get; set; } = ButtonDefaults.IsOutline;

    public ButtonSizes Size { get; set; } = ButtonDefaults.Size;

    [HtmlAttributeName("disabled")]
    public bool IsDisabled { get; set; } = ButtonDefaults.IsDisabled;

    /// <summary>
    /// Quando informado, o componente é renderizado como <c>&lt;a role="button"&gt;</c>.
    /// </summary>
    [HtmlAttributeName("href")]
    public string? Href { get; set; }

    /// <summary>
    /// Tipo do botão (button/submit/reset). Ignorado quando <see cref="Href"/> é informado.
    /// </summary>
    [HtmlAttributeName("type")]
    public ButtonTypes Type { get; set; } = ButtonDefaults.Type;

    public BootstrapIcons Icon { get; set; } = ButtonDefaults.Icon;

    [HtmlAttributeName("no-wrap")]
    public bool IsNoWrap { get; set; } = ButtonDefaults.IsNoWrap;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <button type="button" class="btn btn-primary">Primary</button>
        <a class="btn btn-primary" href="#" role="button">Link</a>
        */
        var isAnchor = !string.IsNullOrWhiteSpace(Href);

        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("btn", HtmlEncoder.Default);

        // Variant (+ outline)
        if (Variant is not ButtonVariants.None)
        {
            var variantClass = Variant.ToFriendlyName()!;
            if (IsOutline)
            {
                variantClass = variantClass.Replace("btn-", "btn-outline-");
            }
            output.AddClass(variantClass, HtmlEncoder.Default);
        }

        // Size
        if (Size is not ButtonSizes.None)
        {
            output.AddClass(Size.ToFriendlyName(), HtmlEncoder.Default);
        }

        // NoWrap
        if (IsNoWrap)
        {
            output.AddClass("text-nowrap", HtmlEncoder.Default);
        }

        if (isAnchor)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", Href);
            output.Attributes.SetAttribute("role", "button");

            if (IsDisabled)
            {
                output.AddClass("disabled", HtmlEncoder.Default);
                output.Attributes.SetAttribute("aria-disabled", "true");
                output.Attributes.SetAttribute("tabindex", "-1");
            }
        }
        else
        {
            output.TagName = "button";
            output.Attributes.SetAttribute("type", Type.ToFriendlyName());

            if (IsDisabled)
            {
                output.Attributes.SetAttribute("disabled", "disabled");
            }
        }

        // Icon
        if (Icon is not BootstrapIcons.None)
        {
            //<i class="bi bi-0-circle-fill me-2"></i>
            var icon = $"<i class=\"{Icon.ToFriendlyName()} me-2\"></i>";
            output.PreContent.AppendHtml(icon);
        }

        IBootstrapText.Apply(this, context, output);
        IBootstrapBackground.Apply(this, context, output);
    }
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
/// <item>9.<see cref="Link"/></item>
/// </list>
/// </summary>
public enum ButtonVariants : byte
{
    None = 0,

    [Description("btn-primary")]
    Primary = 1,

    [Description("btn-secondary")]
    Secondary = 2,

    [Description("btn-success")]
    Success = 3,

    [Description("btn-danger")]
    Danger = 4,

    [Description("btn-warning")]
    Warning = 5,

    [Description("btn-info")]
    Info = 6,

    [Description("btn-light")]
    Light = 7,

    [Description("btn-dark")]
    Dark = 8,

    [Description("btn-link")]
    Link = 9,
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Small"/></item>
/// <item>2.<see cref="Large"/></item>
/// </list>
/// </summary>
public enum ButtonSizes : byte
{
    None = 0,

    [Description("btn-sm")]
    Small = 1,

    [Description("btn-lg")]
    Large = 2,
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Button"/></item>
/// <item>2.<see cref="Submit"/></item>
/// <item>3.<see cref="Reset"/></item>
/// </list>
/// </summary>
public enum ButtonTypes : byte
{
    [Description("button")]
    Button = 1,

    [Description("submit")]
    Submit = 2,

    [Description("reset")]
    Reset = 3,
}

public static class ButtonDefaults
{
    public static ButtonVariants Variant = ButtonVariants.Primary;
    public static bool IsOutline = false;
    public static ButtonSizes Size = ButtonSizes.None;
    public static bool IsDisabled = false;
    public static ButtonTypes Type = ButtonTypes.Button;
    public static BootstrapIcons Icon = BootstrapIcons.None;
    public static bool IsNoWrap = false;
    public static string? CustomBackgroundColor = null;
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontWeights FontWeight = FontWeights.None;
    public static FontSizes TextSize = FontSizes.None;
    public static TextColors TextColor = TextColors.None;
    public static string? CustomTextColor = null;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
}
