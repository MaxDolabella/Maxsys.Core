using System.ComponentModel;
using System.Text.Encodings.Web;
using Maxsys.SolutionScaffolder.MVC.Bootstrap.Interfaces;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.TagHelpers;

/*
    Lembrar de adicionar o seguite código .css:

.table {
    --bs-table-bg: transparent;
}

thead.bg-dark {
    --bs-table-bg: var(--bs-secondary-bg-subtle);
    --bs-table-color: var(--bs-secondary-text-emphasis);
}
*/

internal class TableContext
{
    public bool HasDivider { get; set; } = false;
    public TextPositions TextPosition { get; set; } = TextPositions.None;
    public Alignments TextAlignment { get; set; } = Alignments.None;
}

/// <summary>
/// bs-table
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/content/tables/">docs</see>
/// </summary>
[HtmlTargetElement(TAG)]
public class TableTagHelper : TagHelper,
    IBootstrapText,
    IBootstrapBackground
{
    internal const string TAG = "bs-table";

    #region IBootstrapText

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = TableDefaults.CustomTextColor;

    public FontWeights FontWeight { get; set; } = TableDefaults.FontWeight;
    public TextTransformations TextTransform { get; set; } = TableDefaults.TextTransform;
    public FontSizes TextSize { get; set; } = TableDefaults.TextSize;
    public TextColors TextColor { get; set; } = TableDefaults.TextColor;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = TableDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = TableDefaults.IsMonospace;

    void IBootstrapForeground.ApplyTextColor(TagHelperContext context, TagHelperOutput output)
    {
        if (!string.IsNullOrWhiteSpace(CustomTextColor))
        {
            output.AddStyle($"--bs-table-color:{CustomTextColor};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-hover-color:{CustomTextColor};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-striped-color:{CustomTextColor};", HtmlEncoder.Default);
        }
        else if (TextColor is not TextColors.None)
        {
            var color = TextColor.ToCssVar();
            output.AddStyle($"--bs-table-color:{color};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-hover-color:{color};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-striped-color:{color};", HtmlEncoder.Default);
        }
    }

    #endregion IBootstrapText

    #region IBootstrapBackground

    public BackgroundColors BackgroundColor { get; set; } = TableDefaults.BackgroundColor;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; } = TableDefaults.CustomBackgroundColor;

    void IBootstrapBackground.ApplyBackgroundColor(TagHelperContext context, TagHelperOutput output)
    {
        if (!string.IsNullOrWhiteSpace(CustomBackgroundColor))
        {
            output.AddStyle($"--bs-table-bg:{CustomBackgroundColor};", HtmlEncoder.Default);
        }
        else if (BackgroundColor is not BackgroundColors.None)
        {
            output.AddClass(BackgroundColor.ToFriendlyName(), HtmlEncoder.Default);
        }
    }

    #endregion IBootstrapBackground

    #region Props

    public BorderColors BorderColor { get; set; } = TableDefaults.BorderColor;

    [HtmlAttributeName("small")]
    public bool IsSmall { get; set; } = TableDefaults.IsSmall;

    [HtmlAttributeName("striped")]
    public bool IsStriped { get; set; } = TableDefaults.IsStriped;

    [HtmlAttributeName("striped-columns")]
    public bool IsStripedColumns { get; set; } = TableDefaults.IsStripedColumns;

    [HtmlAttributeName("hover")]
    public bool IsHover { get; set; } = TableDefaults.IsHover;

    [HtmlAttributeName("bordered")]
    public bool HasBorder { get; set; } = TableDefaults.HasBorder;

    [HtmlAttributeName("borderless")]
    public bool IsBorderless { get; set; } = TableDefaults.IsBorderless;

    [HtmlAttributeName("responsive")]
    public bool IsResponsive { get; set; } = TableDefaults.IsResponsive;

    [HtmlAttributeName("divider")]
    public bool HasDivider { get; set; } = TableDefaults.HasDivider;

    [HtmlAttributeName("shadow")]
    public bool HasShadow { get; set; } = TableDefaults.HasShadow;

    [HtmlAttributeName("caption-top")]
    public bool IsCaptionTop { get; set; } = TableDefaults.IsCaptionTop;

    [HtmlAttributeName(null, DictionaryAttributePrefix = "align-")]
    public IDictionary<string, bool> AlignmentDictionary { get; set; } = new Dictionary<string, bool>();

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // <table class="table"></table>
        output.TagName = "table";
        output.TagMode = TagMode.StartTagAndEndTag;

        output.AddClass("table", HtmlEncoder.Default);

        if (IsStriped)
        {
            output.AddClass("table-striped", HtmlEncoder.Default);
        }
        if (IsStripedColumns)
        {
            output.AddClass("table-striped-columns", HtmlEncoder.Default);
        }
        if (IsHover)
        {
            output.AddClass("table-hover", HtmlEncoder.Default);
        }
        if (IsSmall)
        {
            output.AddClass("table-sm", HtmlEncoder.Default);
        }
        if (HasShadow)
        {
            output.AddClass("shadow", HtmlEncoder.Default);
        }
        if (IsCaptionTop)
        {
            output.AddClass("caption-top", HtmlEncoder.Default);
        }
        if (IsResponsive)
        {
            output.PreElement.AppendHtml("<div class=\"table-responsive\">");
            output.PostElement.AppendHtml("</div>");
        }

        if (IsBorderless)
        {
            output.AddClass("table-borderless", HtmlEncoder.Default);
        }
        else
        {
            if (BorderColor is not BorderColors.None)
            {
                output.AddClass(BorderColor.ToFriendlyName(), HtmlEncoder.Default);
            }

            if (HasBorder)
            {
                output.AddClass("table-bordered", HtmlEncoder.Default);
            }
        }

        var textPosition = TextPositions.None;
        var textAlignment = Alignments.None;
        if (AlignmentDictionary.Count != 0)
        {
            foreach (var item in Enum.GetValues<TextPositions>())
            {
                if (AlignmentDictionary.ContainsKey(item.ToString().ToLower()))
                {
                    textPosition = item;
                    break;
                }
            }

            foreach (var item in Enum.GetValues<Alignments>())
            {
                if (AlignmentDictionary.ContainsKey(item.ToString().ToLower()))
                {
                    textAlignment = item;
                    break;
                }
            }
        }

        IBootstrapText.Apply(this, context, output);
        IBootstrapBackground.Apply(this, context, output);

        // Create the context to pass to child elements
        var tableContext = new TableContext
        {
            HasDivider = HasDivider,
            TextPosition = textPosition,
            TextAlignment = textAlignment
        };
        context.Items[typeof(TableContext)] = tableContext;
    }
}

[HtmlTargetElement(TAG, ParentTag = TableTagHelper.TAG)]
public class TableHeaderTagHelper : TagHelper,
    IBootstrapText,
    IBootstrapBackground
{
    internal const string TAG = "bs-thead";

    #region IBootstrapText

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = TableHeaderDefaults.CustomTextColor;

    public FontWeights FontWeight { get; set; } = TableHeaderDefaults.FontWeight;
    public TextTransformations TextTransform { get; set; } = TableHeaderDefaults.TextTransform;
    public FontSizes TextSize { get; set; } = TableHeaderDefaults.TextSize;
    public TextColors TextColor { get; set; } = TableHeaderDefaults.TextColor;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = TableHeaderDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = TableHeaderDefaults.IsMonospace;

    void IBootstrapForeground.ApplyTextColor(TagHelperContext context, TagHelperOutput output)
    {
        if (!string.IsNullOrWhiteSpace(CustomTextColor))
        {
            output.AddStyle($"--bs-table-color:{CustomTextColor};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-hover-color:{CustomTextColor};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-striped-color:{CustomTextColor};", HtmlEncoder.Default);
        }
        else if (TextColor is not TextColors.None)
        {
            var color = TextColor.ToCssVar();
            output.AddStyle($"--bs-table-color:{color};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-hover-color:{color};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-striped-color:{color};", HtmlEncoder.Default);
        }
    }

    #endregion IBootstrapText

    #region IBootstrapBackground

    public BackgroundColors BackgroundColor { get; set; } = TableHeaderDefaults.BackgroundColor;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; } = TableHeaderDefaults.CustomBackgroundColor;

    void IBootstrapBackground.ApplyBackgroundColor(TagHelperContext context, TagHelperOutput output)
    {
        if (!string.IsNullOrWhiteSpace(CustomBackgroundColor))
        {
            output.AddStyle($"--bs-table-bg:{CustomBackgroundColor};", HtmlEncoder.Default);
        }
        else if (BackgroundColor is not BackgroundColors.None)
        {
            output.AddClass(BackgroundColor.ToFriendlyName(), HtmlEncoder.Default);
        }
    }

    #endregion IBootstrapBackground

    #region Props

    public TableStyle TableStyle { get; set; } = TableHeaderDefaults.TableStyle;
    public BorderColors BorderColor { get; set; } = TableHeaderDefaults.BorderColor;

    [HtmlAttributeName("bordered")]
    public bool HasBorder { get; set; } = TableHeaderDefaults.HasBorder;

    [HtmlAttributeName(null, DictionaryAttributePrefix = "align-")]
    public IDictionary<string, bool> AlignmentDictionary { get; set; } = new Dictionary<string, bool>();

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // <thead ></thead>

        var tableContext = context.Items[typeof(TableContext)] as TableContext ?? throw new Exception("TableContext not found");

        output.TagName = "thead";
        output.TagMode = TagMode.StartTagAndEndTag;

        if (BorderColor is not BorderColors.None)
        {
            output.AddClass(BorderColor.ToFriendlyName(), HtmlEncoder.Default);
        }

        if (HasBorder)
        {
            output.AddClass("table-bordered", HtmlEncoder.Default);
        }

        if (TableStyle is not TableStyle.None)
        {
            output.AddClass(TableStyle.ToFriendlyName(), HtmlEncoder.Default);
        }

        #region TextAlignment

        var textPosition = tableContext.TextPosition;
        var textAlignment = tableContext.TextAlignment;
        if (AlignmentDictionary.Count != 0)
        {
            foreach (var item in Enum.GetValues<TextPositions>())
            {
                if (AlignmentDictionary.ContainsKey(item.ToString().ToLower()))
                {
                    textPosition = item;
                    break;
                }
            }

            foreach (var item in Enum.GetValues<Alignments>())
            {
                if (AlignmentDictionary.ContainsKey(item.ToString().ToLower()))
                {
                    textAlignment = item;
                    break;
                }
            }
        }

        if (textPosition is not TextPositions.None)
        {
            output.AddClass(textPosition.ToFriendlyName(), HtmlEncoder.Default);
        }
        if (textAlignment is not Alignments.None)
        {
            output.AddClass(textAlignment.ToFriendlyName(), HtmlEncoder.Default);
        }

        #endregion TextAlignment

        IBootstrapText.Apply(this, context, output);
        IBootstrapBackground.Apply(this, context, output);
    }
}

[HtmlTargetElement(TAG, ParentTag = TableTagHelper.TAG)]
public class TableBodyTagHelper : TagHelper,
    IBootstrapText,
    IBootstrapBackground
{
    internal const string TAG = "bs-tbody";

    #region IBootstrapText

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = TableBodyDefaults.CustomTextColor;

    public FontWeights FontWeight { get; set; } = TableBodyDefaults.FontWeight;
    public TextTransformations TextTransform { get; set; } = TableBodyDefaults.TextTransform;
    public FontSizes TextSize { get; set; } = TableBodyDefaults.TextSize;
    public TextColors TextColor { get; set; } = TableBodyDefaults.TextColor;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = TableBodyDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = TableBodyDefaults.IsMonospace;

    void IBootstrapForeground.ApplyTextColor(TagHelperContext context, TagHelperOutput output)
    {
        if (!string.IsNullOrWhiteSpace(CustomTextColor))
        {
            output.AddStyle($"--bs-table-color:{CustomTextColor};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-hover-color:{CustomTextColor};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-striped-color:{CustomTextColor};", HtmlEncoder.Default);
        }
        else if (TextColor is not TextColors.None)
        {
            var color = TextColor.ToCssVar();
            output.AddStyle($"--bs-table-color:{color};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-hover-color:{color};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-striped-color:{color};", HtmlEncoder.Default);
        }
    }

    #endregion IBootstrapText

    #region IBootstrapBackground

    public BackgroundColors BackgroundColor { get; set; } = TableBodyDefaults.BackgroundColor;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; } = TableBodyDefaults.CustomBackgroundColor;

    void IBootstrapBackground.ApplyBackgroundColor(TagHelperContext context, TagHelperOutput output)
    {
        if (!string.IsNullOrWhiteSpace(CustomBackgroundColor))
        {
            output.AddStyle($"--bs-table-bg:{CustomBackgroundColor};", HtmlEncoder.Default);
        }
        else if (BackgroundColor is not BackgroundColors.None)
        {
            output.AddClass(BackgroundColor.ToFriendlyName(), HtmlEncoder.Default);
        }
    }

    #endregion IBootstrapBackground

    #region Props

    public TableStyle TableStyle { get; set; } = TableBodyDefaults.TableStyle;

    [HtmlAttributeName("divider")]
    public bool HasDivider { get; set; } = TableBodyDefaults.HasDivider; // <tbody class="table-group-divider">

    public BorderColors BorderColor { get; set; } = TableBodyDefaults.BorderColor;

    [HtmlAttributeName("bordered")]
    public bool HasBorder { get; set; } = TableBodyDefaults.HasBorder;

    [HtmlAttributeName(null, DictionaryAttributePrefix = "align-")]
    public IDictionary<string, bool> AlignmentDictionary { get; set; } = new Dictionary<string, bool>();

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // <tbody ></tbody>

        var tableContext = context.Items[typeof(TableContext)] as TableContext ?? throw new Exception("TableContext not found");

        output.TagName = "tbody";
        output.TagMode = TagMode.StartTagAndEndTag;

        if (HasDivider || tableContext.HasDivider)
        {
            output.AddClass("table-group-divider", HtmlEncoder.Default);
        }

        if (BorderColor is not BorderColors.None)
        {
            output.AddClass(BorderColor.ToFriendlyName(), HtmlEncoder.Default);
        }

        if (HasBorder)
        {
            output.AddClass("table-bordered", HtmlEncoder.Default);
        }

        if (TableStyle is not TableStyle.None)
        {
            output.AddClass(TableStyle.ToFriendlyName(), HtmlEncoder.Default);
        }

        #region TextAlignment

        var textPosition = tableContext.TextPosition;
        var textAlignment = tableContext.TextAlignment;
        if (AlignmentDictionary.Count != 0)
        {
            foreach (var item in Enum.GetValues<TextPositions>())
            {
                if (AlignmentDictionary.ContainsKey(item.ToString().ToLower()))
                {
                    textPosition = item;
                    break;
                }
            }

            foreach (var item in Enum.GetValues<Alignments>())
            {
                if (AlignmentDictionary.ContainsKey(item.ToString().ToLower()))
                {
                    textAlignment = item;
                    break;
                }
            }
        }

        if (textPosition is not TextPositions.None)
        {
            output.AddClass(textPosition.ToFriendlyName(), HtmlEncoder.Default);
        }
        if (textAlignment is not Alignments.None)
        {
            output.AddClass(textAlignment.ToFriendlyName(), HtmlEncoder.Default);
        }

        #endregion TextAlignment

        IBootstrapText.Apply(this, context, output);
        IBootstrapBackground.Apply(this, context, output);
    }
}

[HtmlTargetElement(TAG, ParentTag = "table")]
[HtmlTargetElement(TAG, ParentTag = TableHeaderTagHelper.TAG)]
[HtmlTargetElement(TAG, ParentTag = TableBodyTagHelper.TAG)]
public class TableRowTagHelper : TagHelper,
    IBootstrapText,
    IBootstrapBackground
{
    internal const string TAG = "bs-tr";

    #region IBootstrapText

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = TableRowDefaults.CustomTextColor;

    public FontWeights FontWeight { get; set; } = TableRowDefaults.FontWeight;
    public TextTransformations TextTransform { get; set; } = TableRowDefaults.TextTransform;
    public FontSizes TextSize { get; set; } = TableRowDefaults.TextSize;
    public TextColors TextColor { get; set; } = TableRowDefaults.TextColor;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = TableRowDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = TableRowDefaults.IsMonospace;

    void IBootstrapForeground.ApplyTextColor(TagHelperContext context, TagHelperOutput output)
    {
        if (!string.IsNullOrWhiteSpace(CustomTextColor))
        {
            output.AddStyle($"--bs-table-color:{CustomTextColor};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-hover-color:{CustomTextColor};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-striped-color:{CustomTextColor};", HtmlEncoder.Default);
        }
        else if (TextColor is not TextColors.None)
        {
            var color = TextColor.ToCssVar();
            output.AddStyle($"--bs-table-color:{color};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-hover-color:{color};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-striped-color:{color};", HtmlEncoder.Default);
        }
    }

    #endregion IBootstrapText

    #region IBootstrapBackground

    public BackgroundColors BackgroundColor { get; set; } = TableRowDefaults.BackgroundColor;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; } = TableRowDefaults.CustomBackgroundColor;

    void IBootstrapBackground.ApplyBackgroundColor(TagHelperContext context, TagHelperOutput output)
    {
        if (!string.IsNullOrWhiteSpace(CustomBackgroundColor))
        {
            output.AddStyle($"--bs-table-bg:{CustomBackgroundColor};", HtmlEncoder.Default);
        }
        else if (BackgroundColor is not BackgroundColors.None)
        {
            output.AddClass(BackgroundColor.ToFriendlyName(), HtmlEncoder.Default);
        }
    }

    #endregion IBootstrapBackground

    #region Props

    public TableStyle TableStyle { get; set; } = TableRowDefaults.TableStyle;
    public bool IsActive { get; set; } = TableRowDefaults.IsActive; //table-active

    public BorderColors BorderColor { get; set; } = TableRowDefaults.BorderColor;

    [HtmlAttributeName("bordered")]
    public bool HasBorder { get; set; } = TableRowDefaults.HasBorder;

    [HtmlAttributeName(null, DictionaryAttributePrefix = "align-")]
    public IDictionary<string, bool> AlignmentDictionary { get; set; } = new Dictionary<string, bool>();

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "tr";
        output.TagMode = TagMode.StartTagAndEndTag;

        if (BorderColor is not BorderColors.None)
        {
            output.AddClass(BorderColor.ToFriendlyName(), HtmlEncoder.Default);
        }

        if (HasBorder)
        {
            output.AddClass("table-bordered", HtmlEncoder.Default);
        }

        if (TableStyle is not TableStyle.None)
        {
            output.AddClass(TableStyle.ToFriendlyName(), HtmlEncoder.Default);
        }

        #region TextAlignment

        var textPosition = TextPositions.None;
        var textAlignment = Alignments.None;
        if (AlignmentDictionary.Count != 0)
        {
            foreach (var item in Enum.GetValues<TextPositions>())
            {
                if (AlignmentDictionary.ContainsKey(item.ToString().ToLower()))
                {
                    textPosition = item;
                    break;
                }
            }

            foreach (var item in Enum.GetValues<Alignments>())
            {
                if (AlignmentDictionary.ContainsKey(item.ToString().ToLower()))
                {
                    textAlignment = item;
                    break;
                }
            }
        }

        if (textPosition is not TextPositions.None)
        {
            output.AddClass(textPosition.ToFriendlyName(), HtmlEncoder.Default);
        }
        if (textAlignment is not Alignments.None)
        {
            output.AddClass(textAlignment.ToFriendlyName(), HtmlEncoder.Default);
        }

        #endregion TextAlignment

        IBootstrapText.Apply(this, context, output);
        IBootstrapBackground.Apply(this, context, output);
    }
}

[HtmlTargetElement(TAG, ParentTag = "tr")]
[HtmlTargetElement(TAG, ParentTag = TableRowTagHelper.TAG)]
public class TableColumnTagHelper : TagHelper,
    IBootstrapText,
    IBootstrapBackground
{
    internal const string TAG = "bs-td";

    #region IBootstrapText

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = TableColumnDefaults.CustomTextColor;

    public FontWeights FontWeight { get; set; } = TableColumnDefaults.FontWeight;
    public TextTransformations TextTransform { get; set; } = TableColumnDefaults.TextTransform;
    public FontSizes TextSize { get; set; } = TableColumnDefaults.TextSize;
    public TextColors TextColor { get; set; } = TableColumnDefaults.TextColor;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = TableColumnDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = TableColumnDefaults.IsMonospace;

    void IBootstrapForeground.ApplyTextColor(TagHelperContext context, TagHelperOutput output)
    {
        if (!string.IsNullOrWhiteSpace(CustomTextColor))
        {
            output.AddStyle($"--bs-table-color:{CustomTextColor};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-hover-color:{CustomTextColor};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-striped-color:{CustomTextColor};", HtmlEncoder.Default);
        }
        else if (TextColor is not TextColors.None)
        {
            var color = TextColor.ToCssVar();
            output.AddStyle($"--bs-table-color:{color};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-hover-color:{color};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-striped-color:{color};", HtmlEncoder.Default);
        }
    }

    #endregion IBootstrapText

    #region IBootstrapBackground

    public BackgroundColors BackgroundColor { get; set; } = TableColumnDefaults.BackgroundColor;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; } = TableColumnDefaults.CustomBackgroundColor;

    void IBootstrapBackground.ApplyBackgroundColor(TagHelperContext context, TagHelperOutput output)
    {
        if (!string.IsNullOrWhiteSpace(CustomBackgroundColor))
        {
            output.AddStyle($"--bs-table-bg:{CustomBackgroundColor};", HtmlEncoder.Default);
        }
        else if (BackgroundColor is not BackgroundColors.None)
        {
            output.AddClass(BackgroundColor.ToFriendlyName(), HtmlEncoder.Default);
        }
    }

    #endregion IBootstrapBackground

    #region Props

    public TableStyle TableStyle { get; set; } = TableColumnDefaults.TableStyle;
    public BorderColors BorderColor { get; set; } = TableColumnDefaults.BorderColor;

    [HtmlAttributeName("bordered")]
    public bool HasBorder { get; set; } = TableColumnDefaults.HasBorder;

    [HtmlAttributeName(null, DictionaryAttributePrefix = "align-")]
    public IDictionary<string, bool> AlignmentDictionary { get; set; } = new Dictionary<string, bool>();

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "td";
        output.TagMode = TagMode.StartTagAndEndTag;

        if (BorderColor is not BorderColors.None)
        {
            output.AddClass(BorderColor.ToFriendlyName(), HtmlEncoder.Default);
        }

        if (HasBorder)
        {
            output.AddClass("table-bordered", HtmlEncoder.Default);
        }

        if (TableStyle is not TableStyle.None)
        {
            output.AddClass(TableStyle.ToFriendlyName(), HtmlEncoder.Default);
        }

        #region TextAlignment

        var textPosition = TextPositions.None;
        var textAlignment = Alignments.None;
        if (AlignmentDictionary.Count != 0)
        {
            foreach (var item in Enum.GetValues<TextPositions>())
            {
                if (AlignmentDictionary.ContainsKey(item.ToString().ToLower()))
                {
                    textPosition = item;
                    break;
                }
            }

            foreach (var item in Enum.GetValues<Alignments>())
            {
                if (AlignmentDictionary.ContainsKey(item.ToString().ToLower()))
                {
                    textAlignment = item;
                    break;
                }
            }
        }

        if (textPosition is not TextPositions.None)
        {
            output.AddClass(textPosition.ToFriendlyName(), HtmlEncoder.Default);
        }
        if (textAlignment is not Alignments.None)
        {
            output.AddClass(textAlignment.ToFriendlyName(), HtmlEncoder.Default);
        }

        #endregion TextAlignment

        IBootstrapText.Apply(this, context, output);
        IBootstrapBackground.Apply(this, context, output);
    }
}

[HtmlTargetElement(TAG, ParentTag = "tr")]
[HtmlTargetElement(TAG, ParentTag = TableRowTagHelper.TAG)]
public class TableHeadColumnTagHelper : TagHelper,
    IBootstrapText,
    IBootstrapBackground
{
    internal const string TAG = "bs-th";

    #region IBootstrapText

    [HtmlAttributeName("custom-fg")]
    public string? CustomTextColor { get; set; } = TableHeadColumnDefaults.CustomTextColor;

    public FontWeights FontWeight { get; set; } = TableHeadColumnDefaults.FontWeight;
    public TextTransformations TextTransform { get; set; } = TableHeadColumnDefaults.TextTransform;
    public FontSizes TextSize { get; set; } = TableHeadColumnDefaults.TextSize;
    public TextColors TextColor { get; set; } = TableHeadColumnDefaults.TextColor;

    [HtmlAttributeName("italic")]
    public bool IsItalic { get; set; } = TableHeadColumnDefaults.IsItalic;

    [HtmlAttributeName("monospace")]
    public bool IsMonospace { get; set; } = TableHeadColumnDefaults.IsMonospace;

    void IBootstrapForeground.ApplyTextColor(TagHelperContext context, TagHelperOutput output)
    {
        if (!string.IsNullOrWhiteSpace(CustomTextColor))
        {
            output.AddStyle($"--bs-table-color:{CustomTextColor};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-hover-color:{CustomTextColor};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-striped-color:{CustomTextColor};", HtmlEncoder.Default);
        }
        else if (TextColor is not TextColors.None)
        {
            var color = TextColor.ToCssVar();
            output.AddStyle($"--bs-table-color:{color};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-hover-color:{color};", HtmlEncoder.Default);
            output.AddStyle($"--bs-table-striped-color:{color};", HtmlEncoder.Default);
        }
    }

    #endregion IBootstrapText

    #region IBootstrapBackground

    public BackgroundColors BackgroundColor { get; set; } = TableHeadColumnDefaults.BackgroundColor;

    [HtmlAttributeName("custom-bg")]
    public string? CustomBackgroundColor { get; set; } = TableHeadColumnDefaults.CustomBackgroundColor;

    void IBootstrapBackground.ApplyBackgroundColor(TagHelperContext context, TagHelperOutput output)
    {
        if (!string.IsNullOrWhiteSpace(CustomBackgroundColor))
        {
            output.AddStyle($"--bs-table-bg:{CustomBackgroundColor};", HtmlEncoder.Default);
        }
        else if (BackgroundColor is not BackgroundColors.None)
        {
            output.AddClass(BackgroundColor.ToFriendlyName(), HtmlEncoder.Default);
        }
    }

    #endregion IBootstrapBackground

    #region Props

    public TableStyle TableStyle { get; set; } = TableHeadColumnDefaults.TableStyle;
    public BorderColors BorderColor { get; set; } = TableHeadColumnDefaults.BorderColor;

    [HtmlAttributeName("bordered")]
    public bool HasBorder { get; set; } = TableHeadColumnDefaults.HasBorder;

    [HtmlAttributeName(null, DictionaryAttributePrefix = "align-")]
    public IDictionary<string, bool> AlignmentDictionary { get; set; } = new Dictionary<string, bool>();

    #endregion Props

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "th";
        output.TagMode = TagMode.StartTagAndEndTag;

        if (BorderColor is not BorderColors.None)
        {
            output.AddClass(BorderColor.ToFriendlyName(), HtmlEncoder.Default);
        }

        if (HasBorder)
        {
            output.AddClass("table-bordered", HtmlEncoder.Default);
        }

        if (TableStyle is not TableStyle.None)
        {
            output.AddClass(TableStyle.ToFriendlyName(), HtmlEncoder.Default);
        }

        #region TextAlignment

        var textPosition = TextPositions.None;
        var textAlignment = Alignments.None;
        if (AlignmentDictionary.Count != 0)
        {
            foreach (var item in Enum.GetValues<TextPositions>())
            {
                if (AlignmentDictionary.ContainsKey(item.ToString().ToLower()))
                {
                    textPosition = item;
                    break;
                }
            }

            foreach (var item in Enum.GetValues<Alignments>())
            {
                if (AlignmentDictionary.ContainsKey(item.ToString().ToLower()))
                {
                    textAlignment = item;
                    break;
                }
            }
        }

        if (textPosition is not TextPositions.None)
        {
            output.AddClass(textPosition.ToFriendlyName(), HtmlEncoder.Default);
        }
        if (textAlignment is not Alignments.None)
        {
            output.AddClass(textAlignment.ToFriendlyName(), HtmlEncoder.Default);
        }

        #endregion TextAlignment

        IBootstrapText.Apply(this, context, output);
        IBootstrapBackground.Apply(this, context, output);
    }
}

/// <summary>
/// <list type="bullet">
/// <item><see cref="Primary"/></item>
/// <item><see cref="Secondary"/></item>
/// <item><see cref="Success"/></item>
/// <item><see cref="Danger"/></item>
/// <item><see cref="Warning"/></item>
/// <item><see cref="Info"/></item>
/// <item><see cref="Light"/></item>
/// <item><see cref="Dark"/></item>
/// </list>
/// </summary>
public enum TableStyle : byte
{
    None = 0,

    [Description("table-primary")]
    Primary,

    [Description("table-secondary")]
    Secondary,

    [Description("table-success")]
    Success,

    [Description("table-danger")]
    Danger,

    [Description("table-warning")]
    Warning,

    [Description("table-info")]
    Info,

    [Description("table-light")]
    Light,

    [Description("table-dark")]
    Dark
}

internal static class Extensions
{
    public static string ToCssVar(this TextColors textColor)
    {
        var @var = textColor switch
        {
            TextColors.Primary => "--bs-primary",
            TextColors.PrimaryEmphasis => "--bs-primary-text-emphasis",
            TextColors.Secondary => "--bs-secondary",
            TextColors.SecondaryEmphasis => "--bs-secondary-text-emphasis",
            TextColors.Success => "--bs-success",
            TextColors.SuccessEmphasis => "--bs-success-text-emphasis",
            TextColors.Danger => "--bs-danger",
            TextColors.DangerEmphasis => "--bs-danger-text-emphasis",
            TextColors.Warning => "--bs-warning",
            TextColors.WarningEmphasis => "--bs-warning-text-emphasis",
            TextColors.Info => "--bs-info",
            TextColors.InfoEmphasis => "--bs-info-text-emphasis",
            TextColors.Light => "--bs-light",
            TextColors.LightEmphasis => "--bs-light-text-emphasis",
            TextColors.Dark => "--bs-dark",
            TextColors.DarkEmphasis => "--bs-dark-text-emphasis",
            TextColors.Body => "--bs-body-color",
            TextColors.BodyEmphasis => "-bs-emphasis-color",
            TextColors.BodySecondary => "--bs-secondary-color",
            TextColors.BodyTertiary => "--bs-tertiary-color",
            TextColors.Black => "--bs-black",
            TextColors.White => "--bs-white",
            TextColors.Black50 => "--bs-gray-900",
            TextColors.White50 => "--bs-gray-100",
            _ => throw new InvalidEnumArgumentException(),
        };

        return $"var({@var})";
    }
}

#region DEFAULTS

public static class TableDefaults
{
    public static string? CustomTextColor = null;
    public static FontWeights FontWeight = FontWeights.None;
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontSizes TextSize = FontSizes.None;
    public static TextColors TextColor = TextColors.None;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
    public static BackgroundColors BackgroundColor = BackgroundColors.None;
    public static string? CustomBackgroundColor = null;
    public static BorderColors BorderColor = BorderColors.None;
    public static bool IsSmall = false;
    public static bool IsStriped = false;
    public static bool IsStripedColumns = false;
    public static bool IsHover = false;
    public static bool HasBorder = false;
    public static bool IsBorderless = false;
    public static bool IsResponsive = false;
    public static bool HasDivider = false;
    public static bool HasShadow = false;
    public static bool IsCaptionTop = false;
}

public static class TableHeaderDefaults
{
    public static string? CustomTextColor = null;
    public static FontWeights FontWeight = FontWeights.None;
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontSizes TextSize = FontSizes.None;
    public static TextColors TextColor = TextColors.None;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
    public static BackgroundColors BackgroundColor = BackgroundColors.None;
    public static string? CustomBackgroundColor = null;
    public static TableStyle TableStyle = TableStyle.None;
    public static BorderColors BorderColor = BorderColors.None;
    public static bool HasBorder = false;
}

public static class TableBodyDefaults
{
    public static string? CustomTextColor = null;
    public static FontWeights FontWeight = FontWeights.None;
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontSizes TextSize = FontSizes.None;
    public static TextColors TextColor = TextColors.None;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
    public static BackgroundColors BackgroundColor = BackgroundColors.None;
    public static string? CustomBackgroundColor = null;
    public static TableStyle TableStyle = TableStyle.None;
    public static bool HasDivider = false; // <tbody class="table-group-divider">
    public static BorderColors BorderColor = BorderColors.None;
    public static bool HasBorder = false;
}

public static class TableRowDefaults
{
    public static string? CustomTextColor = null;
    public static FontWeights FontWeight = FontWeights.None;
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontSizes TextSize = FontSizes.None;
    public static TextColors TextColor = TextColors.None;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
    public static BackgroundColors BackgroundColor = BackgroundColors.None;
    public static string? CustomBackgroundColor = null;
    public static TableStyle TableStyle = TableStyle.None;
    public static bool IsActive = false; //table-active
    public static BorderColors BorderColor = BorderColors.None;
    public static bool HasBorder = false;
}

public static class TableColumnDefaults
{
    public static string? CustomTextColor = null;
    public static FontWeights FontWeight = FontWeights.None;
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontSizes TextSize = FontSizes.None;
    public static TextColors TextColor = TextColors.None;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
    public static BackgroundColors BackgroundColor = BackgroundColors.None;
    public static string? CustomBackgroundColor = null;
    public static TableStyle TableStyle = TableStyle.None;
    public static BorderColors BorderColor = BorderColors.None;
    public static bool HasBorder = false;
}

public static class TableHeadColumnDefaults
{
    public static string? CustomTextColor = null;
    public static FontWeights FontWeight = FontWeights.None;
    public static TextTransformations TextTransform = TextTransformations.None;
    public static FontSizes TextSize = FontSizes.None;
    public static TextColors TextColor = TextColors.None;
    public static bool IsItalic = false;
    public static bool IsMonospace = false;
    public static BackgroundColors BackgroundColor = BackgroundColors.None;
    public static string? CustomBackgroundColor = null;
    public static TableStyle TableStyle = TableStyle.None;
    public static BorderColors BorderColor = BorderColors.None;
    public static bool HasBorder = false;
}
#endregion