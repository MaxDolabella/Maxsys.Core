using System.Text.Encodings.Web;
using Maxsys.Bootstrap;
using Maxsys.Bootstrap.TagHelpers;
using Microsoft.AspNetCore.Html;

namespace Microsoft.AspNetCore.Mvc.TagHelpers;

public static class HtmlContentExtensions
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