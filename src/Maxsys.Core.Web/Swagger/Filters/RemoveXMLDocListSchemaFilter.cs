using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Maxsys.Core.Web.Swagger.Filters;

/// <summary>
/// Remove a tag &lt;list&gt; (e seu conteúdo) da description.
/// </summary>
public class RemoveXMLDocListSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Description?.Contains("<list") != true)
            return;

        var startIndex = schema.Description.IndexOf("<list");
        var endIndex = schema.Description.IndexOf("</list>") + 7;

        schema.Description = schema.Description.Remove(startIndex, endIndex - startIndex).Trim();
    }
}