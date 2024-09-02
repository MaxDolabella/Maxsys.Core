using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Microsoft.AspNetCore.Mvc.TagHelpers;

/// <summary>
/// Utility related extensions for <see cref="TagHelperOutput"/>.
/// </summary>
public static class CustomTagHelperOutputExtensions
{
    /// <summary>
    /// Adds the given <paramref name="styleValue"/> to the <paramref name="tagHelperOutput"/>'s
    /// <see cref="TagHelperOutput.Attributes"/>.
    /// </summary>
    /// <param name="tagHelperOutput">The <see cref="TagHelperOutput"/> this method extends.</param>
    /// <param name="styleValue">The style value to add.</param>
    /// <param name="htmlEncoder">The current HTML encoder.</param>
    public static void AddStyle(
        this TagHelperOutput tagHelperOutput,
        string styleValue,
        HtmlEncoder htmlEncoder)
    {
        ArgumentNullException.ThrowIfNull(tagHelperOutput);

        if (string.IsNullOrEmpty(styleValue))
        {
            return;
        }

        if (!tagHelperOutput.Attributes.TryGetAttribute("style", out TagHelperAttribute styleAttribute))
        {
            tagHelperOutput.Attributes.Add("style", styleValue);
        }
        else
        {
            var currentStyleValue = ExtractStyleValue(styleAttribute, htmlEncoder);
            var encodedStyleValue = htmlEncoder.Encode(styleValue);

            if (string.Equals(currentStyleValue, encodedStyleValue, StringComparison.Ordinal))
            {
                return;
            }

            var arrayOfStyles = currentStyleValue.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                //.SelectMany(perhapsEncoded => perhapsEncoded.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .ToArray();

            if (arrayOfStyles.Contains(encodedStyleValue, StringComparer.Ordinal))
            {
                return;
            }

            var newStyleAttribute = new TagHelperAttribute(
                styleAttribute.Name,
                new HtmlString($"{currentStyleValue} {encodedStyleValue}"),
                styleAttribute.ValueStyle);

            tagHelperOutput.Attributes.SetAttribute(newStyleAttribute);
        }
    }

    private static string ExtractStyleValue(
        TagHelperAttribute styleAttribute,
        HtmlEncoder htmlEncoder)
    {
        string extractedStyleValue;
        switch (styleAttribute.Value)
        {
            case string valueAsString:
                extractedStyleValue = htmlEncoder.Encode(valueAsString);
                break;

            case HtmlString valueAsHtmlString:
                extractedStyleValue = valueAsHtmlString.Value;
                break;

            case IHtmlContent htmlContent:
                using (var stringWriter = new StringWriter())
                {
                    htmlContent.WriteTo(stringWriter, htmlEncoder);
                    extractedStyleValue = stringWriter.ToString();
                }
                break;

            default:
                extractedStyleValue = htmlEncoder.Encode(styleAttribute.Value?.ToString());
                break;
        }
        var currentStyleValue = extractedStyleValue ?? string.Empty;
        return currentStyleValue;
    }
}