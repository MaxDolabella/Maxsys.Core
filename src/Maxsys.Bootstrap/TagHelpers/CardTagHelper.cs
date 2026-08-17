using System.ComponentModel;
using System.Text.Encodings.Web;
using Maxsys.Bootstrap.Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Cartão (container flexível de conteúdo).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/card/">docs</see>
/// </summary>
[HtmlTargetElement("bs-card")]
public class CardTagHelper : TagHelper,
    IBootstrapBackground,
    IBootstrapText
{
    #region IBootstrapBackground

    public BackgroundColors BackgroundColor { get; set; } = CardDefaults.BackgroundColor;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; } = CardDefaults.CustomBackgroundColor;

    #endregion IBootstrapBackground

    #region IBootstrapText

    public TextTransformations TextTransform { get; set; } = CardDefaults.TextTransform;

    public FontWeights FontWeight { get; set; } = CardDefaults.FontWeight;

    public FontSizes TextSize { get; set; } = CardDefaults.TextSize;

    public TextColors TextColor { get; set; } = CardDefaults.TextColor;

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = CardDefaults.CustomTextColor;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = CardDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = CardDefaults.IsMonospace;

    #endregion IBootstrapText

    #region Props

    /// <summary>
    /// Largura opcional do card (ex.: <c>18rem</c>, <c>300px</c>).
    /// </summary>
    [HtmlAttributeName("width")]
    public string? Width { get; set; } = CardDefaults.Width;

    /// <summary>
    /// Cor da borda do card (<c>border-{color}</c>).
    /// </summary>
    [HtmlAttributeName("border-color")]
    public BorderColors BorderColor { get; set; } = CardDefaults.BorderColor;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="card" style="width: 18rem;">
          ...
        </div>
        */
        output.TagName = "div";
        output.AddClass("card", HtmlEncoder.Default);

        if (!string.IsNullOrWhiteSpace(Width))
        {
            output.AddStyle($"width:{Width};", HtmlEncoder.Default);
        }

        if (BorderColor is not BorderColors.None)
        {
            output.AddClass(BorderColor.ToFriendlyName(), HtmlEncoder.Default);
        }

        IBootstrapText.Apply(this, context, output);
        IBootstrapBackground.Apply(this, context, output);
    }
}

/// <summary>
/// Cabeçalho do card (<c>div.card-header</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/card/#header-and-footer">docs</see>
/// </summary>
[HtmlTargetElement("bs-card-header", ParentTag = "bs-card")]
public class CardHeaderTagHelper : TagHelper,
    IBootstrapBackground,
    IBootstrapText
{
    #region IBootstrapBackground

    public BackgroundColors BackgroundColor { get; set; } = CardHeaderDefaults.BackgroundColor;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; } = CardHeaderDefaults.CustomBackgroundColor;

    #endregion IBootstrapBackground

    #region IBootstrapText

    public TextTransformations TextTransform { get; set; } = CardHeaderDefaults.TextTransform;

    public FontWeights FontWeight { get; set; } = CardHeaderDefaults.FontWeight;

    public FontSizes TextSize { get; set; } = CardHeaderDefaults.TextSize;

    public TextColors TextColor { get; set; } = CardHeaderDefaults.TextColor;

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = CardHeaderDefaults.CustomTextColor;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = CardHeaderDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = CardHeaderDefaults.IsMonospace;

    #endregion IBootstrapText

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.AddClass("card-header", HtmlEncoder.Default);

        IBootstrapText.Apply(this, context, output);
        IBootstrapBackground.Apply(this, context, output);
    }
}

/// <summary>
/// Corpo do card (<c>div.card-body</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/card/#body">docs</see>
/// </summary>
[HtmlTargetElement("bs-card-body", ParentTag = "bs-card")]
public class CardBodyTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.AddClass("card-body", HtmlEncoder.Default);
    }
}

/// <summary>
/// Rodapé do card (<c>div.card-footer</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/card/#header-and-footer">docs</see>
/// </summary>
[HtmlTargetElement("bs-card-footer", ParentTag = "bs-card")]
public class CardFooterTagHelper : TagHelper,
    IBootstrapBackground,
    IBootstrapText
{
    #region IBootstrapBackground

    public BackgroundColors BackgroundColor { get; set; } = CardFooterDefaults.BackgroundColor;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; } = CardFooterDefaults.CustomBackgroundColor;

    #endregion IBootstrapBackground

    #region IBootstrapText

    public TextTransformations TextTransform { get; set; } = CardFooterDefaults.TextTransform;

    public FontWeights FontWeight { get; set; } = CardFooterDefaults.FontWeight;

    public FontSizes TextSize { get; set; } = CardFooterDefaults.TextSize;

    public TextColors TextColor { get; set; } = CardFooterDefaults.TextColor;

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = CardFooterDefaults.CustomTextColor;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = CardFooterDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = CardFooterDefaults.IsMonospace;

    #endregion IBootstrapText

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.AddClass("card-footer", HtmlEncoder.Default);

        IBootstrapText.Apply(this, context, output);
        IBootstrapBackground.Apply(this, context, output);
    }
}

/// <summary>
/// Título do card (<c>h5.card-title</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/card/#titles-text-and-links">docs</see>
/// </summary>
[HtmlTargetElement("bs-card-title", ParentTag = "bs-card")]
[HtmlTargetElement("bs-card-title", ParentTag = "bs-card-body")]
public class CardTitleTagHelper : TagHelper,
    IBootstrapText
{
    #region IBootstrapText

    public TextTransformations TextTransform { get; set; } = CardTitleDefaults.TextTransform;

    public FontWeights FontWeight { get; set; } = CardTitleDefaults.FontWeight;

    public FontSizes TextSize { get; set; } = CardTitleDefaults.TextSize;

    public TextColors TextColor { get; set; } = CardTitleDefaults.TextColor;

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = CardTitleDefaults.CustomTextColor;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = CardTitleDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = CardTitleDefaults.IsMonospace;

    #endregion IBootstrapText

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "h5";
        output.AddClass("card-title", HtmlEncoder.Default);

        IBootstrapText.Apply(this, context, output);
    }
}

/// <summary>
/// Subtítulo do card (<c>h6.card-subtitle</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/card/#titles-text-and-links">docs</see>
/// </summary>
[HtmlTargetElement("bs-card-subtitle", ParentTag = "bs-card")]
[HtmlTargetElement("bs-card-subtitle", ParentTag = "bs-card-body")]
public class CardSubtitleTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <h6 class="card-subtitle mb-2 text-body-secondary">Card subtitle</h6>
        */
        output.TagName = "h6";
        output.AddClass("card-subtitle", HtmlEncoder.Default);
        output.AddClass("mb-2", HtmlEncoder.Default);
        output.AddClass("text-body-secondary", HtmlEncoder.Default);
    }
}

/// <summary>
/// Texto do card (<c>p.card-text</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/card/#titles-text-and-links">docs</see>
/// </summary>
[HtmlTargetElement("bs-card-text", ParentTag = "bs-card")]
[HtmlTargetElement("bs-card-text", ParentTag = "bs-card-body")]
public class CardTextTagHelper : TagHelper,
    IBootstrapText
{
    #region IBootstrapText

    public TextTransformations TextTransform { get; set; } = CardTextDefaults.TextTransform;

    public FontWeights FontWeight { get; set; } = CardTextDefaults.FontWeight;

    public FontSizes TextSize { get; set; } = CardTextDefaults.TextSize;

    public TextColors TextColor { get; set; } = CardTextDefaults.TextColor;

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = CardTextDefaults.CustomTextColor;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = CardTextDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = CardTextDefaults.IsMonospace;

    #endregion IBootstrapText

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "p";
        output.AddClass("card-text", HtmlEncoder.Default);

        IBootstrapText.Apply(this, context, output);
    }
}

/// <summary>
/// Imagem do card (<c>img.card-img-top</c>/<c>img.card-img-bottom</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/card/#images">docs</see>
/// </summary>
[HtmlTargetElement("bs-card-img", ParentTag = "bs-card")]
public class CardImageTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Posição da imagem no card (topo ou base).
    /// </summary>
    [HtmlAttributeName("position")]
    public CardImagePositions Position { get; set; } = CardImageDefaults.Position;

    /// <summary>
    /// Endereço da imagem (atributo <c>src</c>).
    /// </summary>
    [HtmlAttributeName("src")]
    public string? Source { get; set; } = CardImageDefaults.Source;

    /// <summary>
    /// Texto alternativo da imagem (atributo <c>alt</c>).
    /// </summary>
    [HtmlAttributeName("alt")]
    public string? AlternateText { get; set; } = CardImageDefaults.AlternateText;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <img src="..." class="card-img-top" alt="...">
        */
        output.TagName = "img";
        output.TagMode = TagMode.StartTagOnly;
        output.AddClass(Position.ToFriendlyName(), HtmlEncoder.Default);

        if (!string.IsNullOrWhiteSpace(Source))
        {
            output.Attributes.SetAttribute("src", Source);
        }

        if (!string.IsNullOrWhiteSpace(AlternateText))
        {
            output.Attributes.SetAttribute("alt", AlternateText);
        }
    }
}

/// <summary>
/// Link do card (<c>a.card-link</c>). O <c>href</c> é repassado ao elemento final.
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/card/#titles-text-and-links">docs</see>
/// </summary>
[HtmlTargetElement("bs-card-link", ParentTag = "bs-card")]
[HtmlTargetElement("bs-card-link", ParentTag = "bs-card-body")]
public class CardLinkTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";
        output.AddClass("card-link", HtmlEncoder.Default);
    }
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Top"/></item>
/// <item>2.<see cref="Bottom"/></item>
/// </list>
/// </summary>
public enum CardImagePositions : byte
{
    [Description("card-img-top")]
    Top = 1,

    [Description("card-img-bottom")]
    Bottom = 2,
}

public static class CardDefaults
{
    public static string? Width = null;
    public static BorderColors BorderColor = BorderColors.None;
    public static BackgroundColors BackgroundColor = BackgroundColors.None;
    public static string? CustomBackgroundColor = null;
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontWeights FontWeight = FontWeights.None;
    public static FontSizes TextSize = FontSizes.None;
    public static TextColors TextColor = TextColors.None;
    public static string? CustomTextColor = null;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
}

public static class CardHeaderDefaults
{
    public static BackgroundColors BackgroundColor = BackgroundColors.None;
    public static string? CustomBackgroundColor = null;
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontWeights FontWeight = FontWeights.None;
    public static FontSizes TextSize = FontSizes.None;
    public static TextColors TextColor = TextColors.None;
    public static string? CustomTextColor = null;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
}

public static class CardFooterDefaults
{
    public static BackgroundColors BackgroundColor = BackgroundColors.None;
    public static string? CustomBackgroundColor = null;
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontWeights FontWeight = FontWeights.None;
    public static FontSizes TextSize = FontSizes.None;
    public static TextColors TextColor = TextColors.None;
    public static string? CustomTextColor = null;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
}

public static class CardTitleDefaults
{
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontWeights FontWeight = FontWeights.None;
    public static FontSizes TextSize = FontSizes.None;
    public static TextColors TextColor = TextColors.None;
    public static string? CustomTextColor = null;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
}

public static class CardTextDefaults
{
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontWeights FontWeight = FontWeights.None;
    public static FontSizes TextSize = FontSizes.None;
    public static TextColors TextColor = TextColors.None;
    public static string? CustomTextColor = null;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
}

public static class CardImageDefaults
{
    public static CardImagePositions Position = CardImagePositions.Top;
    public static string? Source = null;
    public static string? AlternateText = null;
}
