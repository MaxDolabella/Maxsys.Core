using System.ComponentModel;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Botão de fechar (close button) Bootstrap para dispensar modais, toasts, offcanvas e alerts.<br/>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/close-button/">docs</see>
/// </summary>
[HtmlTargetElement("bs-close-button")]
public class CloseButtonTagHelper : TagHelper
{
    #region Props

    [HtmlAttributeName("white")]
    public bool IsWhite { get; set; } = CloseButtonDefaults.IsWhite;

    [HtmlAttributeName("disabled")]
    public bool IsDisabled { get; set; } = CloseButtonDefaults.IsDisabled;

    /// <summary>
    /// Alvo do <c>data-bs-dismiss</c> (modal/toast/offcanvas/alert).
    /// </summary>
    [HtmlAttributeName("dismiss")]
    public CloseButtonDismissTargets Dismiss { get; set; } = CloseButtonDefaults.Dismiss;

    /// <summary>
    /// Texto do <c>aria-label</c>.
    /// </summary>
    [HtmlAttributeName("label")]
    public string AriaLabel { get; set; } = CloseButtonDefaults.AriaLabel;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        */
        output.TagName = "button";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("type", "button");
        output.Attributes.SetAttribute("aria-label", AriaLabel);

        output.AddClass("btn-close", HtmlEncoder.Default);

        // White
        if (IsWhite)
        {
            output.AddClass("btn-close-white", HtmlEncoder.Default);
        }

        // Disabled
        if (IsDisabled)
        {
            output.Attributes.SetAttribute("disabled", "disabled");
        }

        // Dismiss
        if (Dismiss is not CloseButtonDismissTargets.None)
        {
            output.Attributes.SetAttribute("data-bs-dismiss", Dismiss.ToFriendlyName());
        }
    }
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Modal"/></item>
/// <item>2.<see cref="Toast"/></item>
/// <item>3.<see cref="Offcanvas"/></item>
/// <item>4.<see cref="Alert"/></item>
/// </list>
/// </summary>
public enum CloseButtonDismissTargets : byte
{
    None = 0,

    [Description("modal")]
    Modal = 1,

    [Description("toast")]
    Toast = 2,

    [Description("offcanvas")]
    Offcanvas = 3,

    [Description("alert")]
    Alert = 4,
}

public static class CloseButtonDefaults
{
    public static bool IsWhite = false;
    public static bool IsDisabled = false;
    public static CloseButtonDismissTargets Dismiss = CloseButtonDismissTargets.None;
    public static string AriaLabel = "Close";
}
