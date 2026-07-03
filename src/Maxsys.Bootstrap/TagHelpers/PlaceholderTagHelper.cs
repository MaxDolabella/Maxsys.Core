using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using Maxsys.Bootstrap.Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Placeholder (esqueleto de carregamento) Bootstrap.<br/>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/placeholders/">docs</see>
/// </summary>
[HtmlTargetElement("bs-placeholder")]
public class PlaceholderTagHelper : TagHelper,
    IBootstrapBackground
{
    #region IBootstrapBackground

    [HtmlAttributeName("bg")]
    public BackgroundColors BackgroundColor { get; set; } = PlaceholderDefaults.BackgroundColor;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; } = PlaceholderDefaults.CustomBackgroundColor;

    #endregion IBootstrapBackground

    #region Props

    /// <summary>
    /// Largura em colunas do grid (1 a 12).
    /// </summary>
    [HtmlAttributeName("col")]
    [Range(1, 12)]
    public int? Col { get; set; } = PlaceholderDefaults.Col;

    public PlaceholderSizes Size { get; set; } = PlaceholderDefaults.Size;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <span class="placeholder col-6"></span>
        */
        output.TagName = "span";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("placeholder", HtmlEncoder.Default);

        // Col
        if (Col is >= 1 and <= 12)
        {
            output.AddClass($"col-{Col}", HtmlEncoder.Default);
        }

        // Size
        if (Size is not PlaceholderSizes.None)
        {
            output.AddClass(Size.ToFriendlyName(), HtmlEncoder.Default);
        }

        IBootstrapBackground.Apply(this, context, output);
    }
}

/// <summary>
/// Contêiner de animação para placeholders Bootstrap (glow ou wave).<br/>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/placeholders/#animation">docs</see>
/// </summary>
[HtmlTargetElement(GLOW)]
[HtmlTargetElement(WAVE)]
public class PlaceholderAnimationTagHelper : TagHelper
{
    #region Consts

    private const string GLOW = "bs-placeholder-glow";
    private const string WAVE = "bs-placeholder-wave";

    #endregion Consts

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <p class="placeholder-glow"><span class="placeholder col-12"></span></p>
        */
        output.TagName = "p";
        output.TagMode = TagMode.StartTagAndEndTag;

        var @class = context.TagName == GLOW ? "placeholder-glow" : "placeholder-wave";
        output.AddClass(@class, HtmlEncoder.Default);
    }
}

/// <summary>
/// Placeholder Bootstrap com aparência de botão.<br/>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/placeholders/">docs</see>
/// </summary>
[HtmlTargetElement("bs-placeholder-button")]
public class PlaceholderButtonTagHelper : TagHelper
{
    #region Props

    public ButtonVariants Variant { get; set; } = PlaceholderButtonDefaults.Variant;

    /// <summary>
    /// Largura em colunas do grid (1 a 12).
    /// </summary>
    [HtmlAttributeName("col")]
    [Range(1, 12)]
    public int? Col { get; set; } = PlaceholderButtonDefaults.Col;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <a class="btn btn-primary disabled placeholder col-4" aria-disabled="true"></a>
        */
        output.TagName = "a";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("aria-disabled", "true");

        output.AddClass("btn", HtmlEncoder.Default);

        // Variant
        if (Variant is not ButtonVariants.None)
        {
            output.AddClass(Variant.ToFriendlyName(), HtmlEncoder.Default);
        }

        output.AddClass("disabled", HtmlEncoder.Default);
        output.AddClass("placeholder", HtmlEncoder.Default);

        // Col
        if (Col is >= 1 and <= 12)
        {
            output.AddClass($"col-{Col}", HtmlEncoder.Default);
        }
    }
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="ExtraSmall"/></item>
/// <item>2.<see cref="Small"/></item>
/// <item>3.<see cref="Large"/></item>
/// </list>
/// </summary>
public enum PlaceholderSizes : byte
{
    None = 0,

    [Description("placeholder-xs")]
    ExtraSmall = 1,

    [Description("placeholder-sm")]
    Small = 2,

    [Description("placeholder-lg")]
    Large = 3,
}

public static class PlaceholderDefaults
{
    public static BackgroundColors BackgroundColor = BackgroundColors.None;
    public static string? CustomBackgroundColor = null;
    public static int? Col = null;
    public static PlaceholderSizes Size = PlaceholderSizes.None;
}

public static class PlaceholderButtonDefaults
{
    public static ButtonVariants Variant = ButtonVariants.Primary;
    public static int? Col = null;
}
