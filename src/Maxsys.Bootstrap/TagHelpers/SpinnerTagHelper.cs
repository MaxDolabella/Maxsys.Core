using System.Text.Encodings.Web;
using Maxsys.Bootstrap.Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Indicador de carregamento (spinner) Bootstrap nas variações border (padrão) e grow.<br/>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/spinners/">docs</see>
/// </summary>
[HtmlTargetElement("bs-spinner")]
public class SpinnerTagHelper : TagHelper,
    IBootstrapForeground
{
    #region IBootstrapForeground

    public TextColors TextColor { get; set; } = SpinnerDefaults.TextColor;

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = SpinnerDefaults.CustomTextColor;

    #endregion IBootstrapForeground

    #region Props

    /// <summary>
    /// Usa a variação <c>spinner-grow</c> em vez de <c>spinner-border</c>.
    /// </summary>
    [HtmlAttributeName("grow")]
    public bool IsGrow { get; set; } = SpinnerDefaults.IsGrow;

    [HtmlAttributeName("small")]
    public bool IsSmall { get; set; } = SpinnerDefaults.IsSmall;

    /// <summary>
    /// Texto acessível (visually-hidden) do spinner. Ignorado quando <see cref="NoStatus"/> é <see langword="true"/>.
    /// </summary>
    [HtmlAttributeName("label")]
    public string Label { get; set; } = SpinnerDefaults.Label;

    /// <summary>
    /// Para uso inline (dentro de botões, por exemplo): não renderiza <c>role="status"</c>
    /// nem o span visually-hidden, e adiciona <c>aria-hidden="true"</c>.
    /// </summary>
    [HtmlAttributeName("no-status")]
    public bool NoStatus { get; set; } = SpinnerDefaults.NoStatus;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="spinner-border" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
        */
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var baseClass = IsGrow ? "spinner-grow" : "spinner-border";
        output.AddClass(baseClass, HtmlEncoder.Default);

        // Small
        if (IsSmall)
        {
            output.AddClass($"{baseClass}-sm", HtmlEncoder.Default);
        }

        if (NoStatus)
        {
            // <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
            output.TagName = "span";
            output.Attributes.SetAttribute("aria-hidden", "true");
        }
        else
        {
            output.Attributes.SetAttribute("role", "status");
            output.PreContent.AppendHtml($"<span class=\"visually-hidden\">{HtmlEncoder.Default.Encode(Label)}</span>");
        }

        IBootstrapForeground.Apply(this, context, output);
    }
}

public static class SpinnerDefaults
{
    public static bool IsGrow = false;
    public static bool IsSmall = false;
    public static string Label = "Loading...";
    public static bool NoStatus = false;
    public static TextColors TextColor = TextColors.None;
    public static string? CustomTextColor = null;
}
