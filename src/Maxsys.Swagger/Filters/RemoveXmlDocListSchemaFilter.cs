using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Maxsys.Swagger.Filters;

/// <summary>
/// Remove a tag &lt;list&gt; (e seu conteúdo) da description dos schemas.
/// </summary>
public class RemoveXmlDocListSchemaFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is not OpenApiSchema concrete)
            return;

        if (concrete.Description?.Contains("<list") != true)
            return;

        var startIndex = concrete.Description.IndexOf("<list", StringComparison.Ordinal);
        var endIndex = concrete.Description.IndexOf("</list>", StringComparison.Ordinal) + "</list>".Length;

        if (endIndex > startIndex)
            concrete.Description = concrete.Description.Remove(startIndex, endIndex - startIndex).Trim();
    }
}
