using System.Globalization;
using System.Text.Encodings.Web;
using Maxsys.Bootstrap.Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

#region Context

internal class ProgressStackedTagContext
{
    // marker: children (bs-progress) devem aplicar width no wrapper e não na barra
}

internal class ProgressTagContext
{
    public required double Value { get; set; }
    public required double Min { get; set; }
    public required double Max { get; set; }
    public required bool IsStacked { get; set; }

    public double GetPercentage()
    {
        var range = Max - Min;
        var percentage = range <= 0 ? 0d : (Value - Min) / range * 100d;

        return Math.Clamp(percentage, 0d, 100d);
    }
}

#endregion Context

/// <summary>
/// Contêiner de barras de progresso empilhadas (stacked) Bootstrap.<br/>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/progress/#multiple-bars">docs</see>
/// </summary>
[HtmlTargetElement("bs-progress-stacked")]
public class ProgressStackedTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="progress-stacked">
          <div class="progress" role="progressbar" ... style="width: 15%">...</div>
          ...
        </div>
        */
        context.Items[typeof(ProgressStackedTagContext)] = new ProgressStackedTagContext();

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("progress-stacked", HtmlEncoder.Default);
    }
}

/// <summary>
/// Barra de progresso Bootstrap (wrapper). O preenchimento é definido pelo filho <c>bs-progress-bar</c>.<br/>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/progress/">docs</see>
/// </summary>
[HtmlTargetElement("bs-progress")]
public class ProgressTagHelper : TagHelper
{
    #region Props

    /// <summary>
    /// Valor atual da barra (entre <see cref="Min"/> e <see cref="Max"/>).
    /// </summary>
    [HtmlAttributeName("value")]
    public double Value { get; set; } = ProgressDefaults.Value;

    [HtmlAttributeName("min")]
    public double Min { get; set; } = ProgressDefaults.Min;

    [HtmlAttributeName("max")]
    public double Max { get; set; } = ProgressDefaults.Max;

    /// <summary>
    /// Altura da barra em pixels. Quando <see langword="null"/>, usa a altura padrão do Bootstrap.
    /// </summary>
    [HtmlAttributeName("height")]
    public int? Height { get; set; } = ProgressDefaults.Height;

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="progress" role="progressbar" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100">
          <div class="progress-bar" style="width: 25%"></div>
        </div>
        */
        var progressContext = new ProgressTagContext
        {
            Value = Value,
            Min = Min,
            Max = Max,
            IsStacked = context.Items.ContainsKey(typeof(ProgressStackedTagContext))
        };
        context.Items[typeof(ProgressTagContext)] = progressContext;

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("role", "progressbar");
        output.Attributes.SetAttribute("aria-valuenow", Value.ToString(CultureInfo.InvariantCulture));
        output.Attributes.SetAttribute("aria-valuemin", Min.ToString(CultureInfo.InvariantCulture));
        output.Attributes.SetAttribute("aria-valuemax", Max.ToString(CultureInfo.InvariantCulture));

        output.AddClass("progress", HtmlEncoder.Default);

        // Height
        if (Height.HasValue)
        {
            output.AddStyle($"height:{Height.Value}px;", HtmlEncoder.Default);
        }

        // Stacked: width vai no wrapper, não na barra
        if (progressContext.IsStacked)
        {
            var percentage = progressContext.GetPercentage().ToString(CultureInfo.InvariantCulture);
            output.AddStyle($"width:{percentage}%;", HtmlEncoder.Default);
        }
    }
}

/// <summary>
/// Preenchimento (progress-bar) de uma barra de progresso Bootstrap.<br/>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/progress/">docs</see>
/// </summary>
[HtmlTargetElement("bs-progress-bar", ParentTag = "bs-progress")]
public class ProgressBarTagHelper : TagHelper,
    IBootstrapBackground
{
    #region IBootstrapBackground

    [HtmlAttributeName("bg")]
    public BackgroundColors BackgroundColor { get; set; } = ProgressBarDefaults.BackgroundColor;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; } = ProgressBarDefaults.CustomBackgroundColor;

    #endregion IBootstrapBackground

    #region Props

    [HtmlAttributeName("striped")]
    public bool IsStriped { get; set; } = ProgressBarDefaults.IsStriped;

    [HtmlAttributeName("animated")]
    public bool IsAnimated { get; set; } = ProgressBarDefaults.IsAnimated;

    /// <summary>
    /// Renderiza o percentual (ex.: <c>25%</c>) como conteúdo da barra quando o conteúdo estiver vazio.
    /// </summary>
    [HtmlAttributeName("show-label")]
    public bool ShowLabel { get; set; } = ProgressBarDefaults.ShowLabel;

    #endregion Props

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        /*
        <div class="progress-bar progress-bar-striped progress-bar-animated" style="width: 25%">25%</div>
        */
        var progressContext = context.Items[typeof(ProgressTagContext)] as ProgressTagContext;
        ArgumentNullException.ThrowIfNull(progressContext);

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("progress-bar", HtmlEncoder.Default);

        // Striped/Animated (animated requer striped)
        if (IsStriped || IsAnimated)
        {
            output.AddClass("progress-bar-striped", HtmlEncoder.Default);
        }
        if (IsAnimated)
        {
            output.AddClass("progress-bar-animated", HtmlEncoder.Default);
        }

        var percentage = progressContext.GetPercentage();

        // Em stacked, o width fica no wrapper (bs-progress)
        if (!progressContext.IsStacked)
        {
            output.AddStyle($"width:{percentage.ToString(CultureInfo.InvariantCulture)}%;", HtmlEncoder.Default);
        }

        // Label
        if (ShowLabel)
        {
            var childContent = await output.GetChildContentAsync();
            if (childContent.IsEmptyOrWhiteSpace)
            {
                output.Content.SetContent($"{percentage.ToString(CultureInfo.InvariantCulture)}%");
            }
        }

        IBootstrapBackground.Apply(this, context, output);
    }
}

public static class ProgressDefaults
{
    public static double Value = 0;
    public static double Min = 0;
    public static double Max = 100;
    public static int? Height = null;
}

public static class ProgressBarDefaults
{
    public static BackgroundColors BackgroundColor = BackgroundColors.None;
    public static string? CustomBackgroundColor = null;
    public static bool IsStriped = false;
    public static bool IsAnimated = false;
    public static bool ShowLabel = false;
}
