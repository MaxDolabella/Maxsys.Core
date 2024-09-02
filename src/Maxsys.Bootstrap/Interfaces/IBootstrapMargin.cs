using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Maxsys.Bootstrap.Interfaces;

public interface IBootstrapMargin
{
    IDictionary<string, bool> MarginDictionary { get; set; }
    IDictionary<string, bool> MarginXDictionary { get; set; }
    IDictionary<string, bool> MarginYDictionary { get; set; }
    IDictionary<string, bool> MarginSDictionary { get; set; }
    IDictionary<string, bool> MarginTDictionary { get; set; }
    IDictionary<string, bool> MarginEDictionary { get; set; }
    IDictionary<string, bool> MarginBDictionary { get; set; }
    
    public virtual void ApplyMargin(TagHelperContext context, TagHelperOutput output)
    {
        throw new NotImplementedException();
    }

    public static void Apply(IBootstrapMargin me, TagHelperContext context, TagHelperOutput output)
    {
        me.ApplyMargin(context, output);
    }
}