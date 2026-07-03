using System.ComponentModel;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Maxsys.Bootstrap.ViewComponents;

/// <summary>
/// Carousel <i>data-driven</i>: recebe a lista de slides e renderiza a estrutura completa
/// (indicators e controls com contagem correta, primeiro slide ativo).
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/carousel/">docs</see>
/// </summary>
/// <remarks>
/// Uso (requer <c>AddMaxsysBootstrap()</c> no MVC):
/// <code>
/// &lt;vc:bs-carousel slides="Model.Slides" controls="true" indicators="true" /&gt;
/// // onde Slides é IEnumerable&lt;CarouselSlide&gt;:
/// new CarouselSlide[] { new("/img/1.jpg", "Primeiro", CaptionTitle: "Título", CaptionText: "Texto") }
/// </code>
/// Para markup manual, use o TagHelper <c>&lt;bs-carousel&gt;</c>.
/// </remarks>
public class BsCarouselViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(
        IEnumerable<CarouselSlide> slides,
        string? id = null,
        bool? fade = null,
        bool? controls = null,
        bool? indicators = null,
        CarouselAutoplayModes? autoplay = null,
        int? intervalMs = null)
    {
        ArgumentNullException.ThrowIfNull(slides, nameof(slides));

        var encoder = HtmlEncoder.Default;
        var slideList = slides.ToList();

        id ??= $"carousel-{Utils.GenerateRandomId()}";
        var showControls = controls ?? BsCarouselViewDefaults.Controls;
        var showIndicators = indicators ?? BsCarouselViewDefaults.Indicators;
        var autoplayMode = autoplay ?? BsCarouselViewDefaults.Autoplay;
        var interval = intervalMs ?? BsCarouselViewDefaults.IntervalMs;

        var carouselClasses = "carousel slide" + ((fade ?? BsCarouselViewDefaults.Fade) ? " carousel-fade" : string.Empty);
        var ride = autoplayMode.ToFriendlyName();
        var rideAttr = string.IsNullOrEmpty(ride) ? string.Empty : $" data-bs-ride=\"{ride}\"";

        var encodedId = encoder.Encode(id);

        var sb = new StringBuilder();
        sb.Append($"<div id=\"{encodedId}\" class=\"{carouselClasses}\"{rideAttr}>");

        // indicators (contagem correta a partir do modelo)
        if (showIndicators && slideList.Count > 0)
        {
            sb.Append("<div class=\"carousel-indicators\">");
            for (var i = 0; i < slideList.Count; i++)
            {
                var active = i == 0 ? " class=\"active\" aria-current=\"true\"" : string.Empty;
                sb.Append($"<button type=\"button\" data-bs-target=\"#{encodedId}\" data-bs-slide-to=\"{i}\"{active} aria-label=\"Slide {i + 1}\"></button>");
            }
            sb.Append("</div>");
        }

        // slides
        sb.Append("<div class=\"carousel-inner\">");
        for (var i = 0; i < slideList.Count; i++)
        {
            var slide = slideList[i];
            var itemClass = "carousel-item" + (i == 0 ? " active" : string.Empty);
            var intervalAttr = interval > 0 ? $" data-bs-interval=\"{interval}\"" : string.Empty;

            sb.Append($"<div class=\"{itemClass}\"{intervalAttr}>");
            sb.Append($"<img src=\"{encoder.Encode(slide.ImageUrl)}\" class=\"d-block w-100\" alt=\"{encoder.Encode(slide.Alt ?? string.Empty)}\">");

            if (slide.CaptionTitle is not null || slide.CaptionText is not null)
            {
                sb.Append("<div class=\"carousel-caption d-none d-md-block\">");
                if (slide.CaptionTitle is not null)
                    sb.Append($"<h5>{encoder.Encode(slide.CaptionTitle)}</h5>");
                if (slide.CaptionText is not null)
                    sb.Append($"<p>{encoder.Encode(slide.CaptionText)}</p>");
                sb.Append("</div>");
            }

            sb.Append("</div>");
        }
        sb.Append("</div>");

        // controls
        if (showControls)
        {
            sb.Append($"<button class=\"carousel-control-prev\" type=\"button\" data-bs-target=\"#{encodedId}\" data-bs-slide=\"prev\">");
            sb.Append("<span class=\"carousel-control-prev-icon\" aria-hidden=\"true\"></span><span class=\"visually-hidden\">Anterior</span></button>");
            sb.Append($"<button class=\"carousel-control-next\" type=\"button\" data-bs-target=\"#{encodedId}\" data-bs-slide=\"next\">");
            sb.Append("<span class=\"carousel-control-next-icon\" aria-hidden=\"true\"></span><span class=\"visually-hidden\">Próximo</span></button>");
        }

        sb.Append("</div>");

        return new HtmlContentViewComponentResult(new HtmlString(sb.ToString()));
    }
}

/// <summary>
/// Slide do carousel.
/// </summary>
/// <param name="ImageUrl">URL da imagem.</param>
/// <param name="Alt">Texto alternativo da imagem.</param>
/// <param name="CaptionTitle">Título da legenda (opcional).</param>
/// <param name="CaptionText">Texto da legenda (opcional).</param>
public sealed record CarouselSlide(string ImageUrl, string? Alt = null, string? CaptionTitle = null, string? CaptionText = null);

/// <summary>
/// Modo de autoplay do carousel (<c>data-bs-ride</c>).
/// </summary>
public enum CarouselAutoplayModes : byte
{
    /// <summary>Sem autoplay.</summary>
    [Description("")]
    None = 0,

    /// <summary>Inicia autoplay ao carregar a página (<c>data-bs-ride="carousel"</c>).</summary>
    [Description("carousel")]
    OnLoad = 1,

    /// <summary>Inicia autoplay após a primeira interação do usuário (<c>data-bs-ride="true"</c>).</summary>
    [Description("true")]
    AfterInteraction = 2,
}

public static class BsCarouselViewDefaults
{
    public static bool Fade = false;
    public static bool Controls = true;
    public static bool Indicators = false;
    public static CarouselAutoplayModes Autoplay = CarouselAutoplayModes.None;

    /// <summary>Intervalo entre slides em ms. 0 usa o padrão do Bootstrap (5000).</summary>
    public static int IntervalMs = 0;
}
