using System.Text.Encodings.Web;
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
}