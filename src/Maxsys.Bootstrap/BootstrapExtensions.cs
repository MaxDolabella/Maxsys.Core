using System.Text.Encodings.Web;
using Maxsys.Bootstrap;
using Maxsys.Bootstrap.TagHelpers;
using Microsoft.AspNetCore.Html;

namespace Microsoft.AspNetCore.Mvc.TagHelpers;

public static class BootstrapExtensions
{
    public static string GetContent(this IHtmlContent content)
    {
        using (var writer = new StringWriter())
        {
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }

    public static void InitializeDefaults()
    {
        TableDefaults.IsResponsive = true;

        TableHeadColumnDefaults.FontWeight = FontWeights.Semibold;
        TableHeadColumnDefaults.TextColor = TextColors.PrimaryEmphasis;
        TableHeadColumnDefaults.TextTransform = TextTransformations.UpperCase;

        FormLabelDefaults.IsItalic = true;
        FormLabelDefaults.IsSmall = true;
        FormLabelDefaults.Color = TextColors.PrimaryEmphasis;

        BreadcrumbDefaults.TextSize = FontSizes.Size6;
        BreadcrumbDefaults.FontWeight = FontWeights.Light;
    }
}