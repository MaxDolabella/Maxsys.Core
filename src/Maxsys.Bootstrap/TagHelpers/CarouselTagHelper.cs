using System.ComponentModel;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/// <summary>
/// Carrossel (<c>div.carousel.slide</c>). Gera <c>id</c> aleatório quando não informado.
/// Os filhos <c>bs-carousel-item</c> são envolvidos automaticamente em <c>div.carousel-inner</c>.
/// Como o pai é processado antes dos filhos, os indicators não podem ser contados automaticamente:
/// quando <c>indicators="true"</c>, informe a quantidade de slides na prop <c>slides</c>.
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/carousel/">docs</see>
/// </summary>
[HtmlTargetElement("bs-carousel")]
public class CarouselTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Quando <see langword="true"/>, usa transição de fade (<c>carousel-fade</c>).
    /// </summary>
    [HtmlAttributeName("fade")]
    public bool IsFade { get; set; } = CarouselDefaults.IsFade;

    /// <summary>
    /// Comportamento de autoplay (<c>data-bs-ride</c>): <c>carousel</c> inicia sozinho;
    /// <c>true</c> inicia após a primeira interação.
    /// </summary>
    public CarouselAutoplays Autoplay { get; set; } = CarouselDefaults.Autoplay;

    /// <summary>
    /// Intervalo padrão entre slides, em milissegundos (<c>data-bs-interval</c>).
    /// </summary>
    [HtmlAttributeName("interval")]
    public int? Interval { get; set; } = CarouselDefaults.Interval;

    /// <summary>
    /// Quando <see langword="true"/>, gera os botões prev/next.
    /// </summary>
    [HtmlAttributeName("controls")]
    public bool HasControls { get; set; } = CarouselDefaults.HasControls;

    /// <summary>
    /// Quando <see langword="true"/>, gera os indicators. Requer <c>slides</c> informado.
    /// </summary>
    [HtmlAttributeName("indicators")]
    public bool HasIndicators { get; set; } = CarouselDefaults.HasIndicators;

    /// <summary>
    /// Quantidade de slides — usada apenas para gerar os indicators
    /// (o pai não consegue contar os filhos, pois é processado antes deles).
    /// </summary>
    [HtmlAttributeName("slides")]
    public int Slides { get; set; } = CarouselDefaults.Slides;

    /// <summary>
    /// Quando <see langword="false"/>, desabilita swipe em touchscreen (<c>data-bs-touch="false"</c>).
    /// </summary>
    [HtmlAttributeName("touch")]
    public bool Touch { get; set; } = CarouselDefaults.Touch;

    /// <summary>
    /// Quando <see langword="false"/>, desabilita navegação por teclado (<c>data-bs-keyboard="false"</c>).
    /// </summary>
    [HtmlAttributeName("keyboard")]
    public bool Keyboard { get; set; } = CarouselDefaults.Keyboard;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div id="carouselExample" class="carousel slide">
          <div class="carousel-indicators">
            <button type="button" data-bs-target="#carouselExample" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
          </div>
          <div class="carousel-inner">
            <div class="carousel-item active">...</div>
          </div>
          <button class="carousel-control-prev" type="button" data-bs-target="#carouselExample" data-bs-slide="prev">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Previous</span>
          </button>
          <button class="carousel-control-next" type="button" data-bs-target="#carouselExample" data-bs-slide="next">
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Next</span>
          </button>
        </div>
        */
        var id = output.Attributes.TryGetAttribute("id", out var idAtt)
            ? idAtt.Value.ToString()!
            : $"crs-{Utils.GenerateRandomId()}";

        output.TagName = "div";
        output.Attributes.SetAttribute("id", id);

        output.AddClass("carousel", HtmlEncoder.Default);
        output.AddClass("slide", HtmlEncoder.Default);

        if (IsFade)
        {
            output.AddClass("carousel-fade", HtmlEncoder.Default);
        }

        if (Autoplay is not CarouselAutoplays.None)
        {
            output.Attributes.SetAttribute("data-bs-ride", Autoplay.ToFriendlyName());
        }

        if (Interval.HasValue)
        {
            output.Attributes.SetAttribute("data-bs-interval", Interval.Value.ToString());
        }

        if (!Touch)
        {
            output.Attributes.SetAttribute("data-bs-touch", "false");
        }

        if (!Keyboard)
        {
            output.Attributes.SetAttribute("data-bs-keyboard", "false");
        }

        // PreContent: indicators (opcional) + abertura do carousel-inner
        var pre = new StringBuilder();

        if (HasIndicators)
        {
            if (Slides <= 0)
            {
                throw new ArgumentException("Quando indicators=\"true\", a prop slides deve ser informada (> 0).");
            }

            pre.AppendLine("<div class=\"carousel-indicators\">");
            for (int i = 0; i < Slides; i++)
            {
                var active = i == 0 ? " class=\"active\" aria-current=\"true\"" : string.Empty;
                pre.AppendLine($"  <button type=\"button\" data-bs-target=\"#{id}\" data-bs-slide-to=\"{i}\"{active} aria-label=\"Slide {i + 1}\"></button>");
            }
            pre.AppendLine("</div>");
        }

        pre.AppendLine("<div class=\"carousel-inner\">");
        output.PreContent.SetHtmlContent(pre.ToString());

        // PostContent: fechamento do carousel-inner + controls (opcional)
        var post = new StringBuilder();
        post.AppendLine("</div>");

        if (HasControls)
        {
            post.AppendLine($"<button class=\"carousel-control-prev\" type=\"button\" data-bs-target=\"#{id}\" data-bs-slide=\"prev\">");
            post.AppendLine("  <span class=\"carousel-control-prev-icon\" aria-hidden=\"true\"></span>");
            post.AppendLine("  <span class=\"visually-hidden\">Previous</span>");
            post.AppendLine("</button>");
            post.AppendLine($"<button class=\"carousel-control-next\" type=\"button\" data-bs-target=\"#{id}\" data-bs-slide=\"next\">");
            post.AppendLine("  <span class=\"carousel-control-next-icon\" aria-hidden=\"true\"></span>");
            post.AppendLine("  <span class=\"visually-hidden\">Next</span>");
            post.AppendLine("</button>");
        }

        output.PostContent.SetHtmlContent(post.ToString());
    }
}

/// <summary>
/// Slide do carrossel (<c>div.carousel-item</c>). O primeiro slide deve ser marcado com <c>active</c> pelo usuário.
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/carousel/">docs</see>
/// </summary>
[HtmlTargetElement("bs-carousel-item", ParentTag = "bs-carousel")]
public class CarouselItemTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Quando <see langword="true"/>, marca o slide como ativo (obrigatório em exatamente um item).
    /// </summary>
    [HtmlAttributeName("active")]
    public bool IsActive { get; set; } = CarouselItemDefaults.IsActive;

    /// <summary>
    /// Intervalo individual do slide, em milissegundos (<c>data-bs-interval</c>).
    /// </summary>
    [HtmlAttributeName("interval")]
    public int? Interval { get; set; } = CarouselItemDefaults.Interval;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="carousel-item active" data-bs-interval="10000">
            ...
        </div>
        */
        output.TagName = "div";
        output.AddClass("carousel-item", HtmlEncoder.Default);

        if (IsActive)
        {
            output.AddClass("active", HtmlEncoder.Default);
        }

        if (Interval.HasValue)
        {
            output.Attributes.SetAttribute("data-bs-interval", Interval.Value.ToString());
        }
    }
}

/// <summary>
/// Legenda do slide (<c>div.carousel-caption.d-none.d-md-block</c>).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/carousel/">docs</see>
/// </summary>
[HtmlTargetElement("bs-carousel-caption", ParentTag = "bs-carousel-item")]
public class CarouselCaptionTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="carousel-caption d-none d-md-block">
            ...
        </div>
        */
        output.TagName = "div";
        output.AddClass("carousel-caption", HtmlEncoder.Default);
        output.AddClass("d-none", HtmlEncoder.Default);
        output.AddClass("d-md-block", HtmlEncoder.Default);
    }
}

/// <summary>
/// <list type="bullet">
/// <item>1.<see cref="Carousel"/> — inicia sozinho no load</item>
/// <item>2.<see cref="True"/> — inicia após a primeira interação do usuário</item>
/// </list>
/// </summary>
public enum CarouselAutoplays : byte
{
    None = 0,

    [Description("carousel")]
    Carousel = 1,

    [Description("true")]
    True = 2,
}

public static class CarouselDefaults
{
    public static bool IsFade = false;
    public static CarouselAutoplays Autoplay = CarouselAutoplays.None;
    public static int? Interval = null;
    public static bool HasControls = false;
    public static bool HasIndicators = false;
    public static int Slides = 0;
    public static bool Touch = true;
    public static bool Keyboard = true;
}

public static class CarouselItemDefaults
{
    public static bool IsActive = false;
    public static int? Interval = null;
}
