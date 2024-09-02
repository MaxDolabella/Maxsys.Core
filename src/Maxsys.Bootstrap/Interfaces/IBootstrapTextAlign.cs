using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.Interfaces;

public interface IBootstrapTextAlign
{
    IDictionary<string, bool> AlignmentDictionary { get; set; }

    public virtual void ApplyTextAlignment(TagHelperContext context, TagHelperOutput output)
    {
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
    }

    public static void Apply(IBootstrapTextAlign me, TagHelperContext context, TagHelperOutput output)
    {
        me.ApplyTextAlignment(context, output);
    }
}