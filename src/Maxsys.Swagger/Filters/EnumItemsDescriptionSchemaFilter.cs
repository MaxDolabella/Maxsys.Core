using Maxsys.Swagger.Helpers;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Maxsys.Swagger.Filters;

/// <summary>
/// Quando o Schema for enum, adiciona à description os detalhes dos literals.
/// Cada item do enum terá no seguinte formato:
/// <para/>
/// {valor} - {literal} ({atributo_description quando tiver})
/// </summary>
public class EnumItemsDescriptionSchemaFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is not OpenApiSchema concrete)
            return;

        if (!context.Type.IsEnum)
            return;

        var contents = EnumSwaggerHelper.GetEnumDescriptionsList(context.Type);
        var description = string.Join("<br/>", contents);

        concrete.Description = string.IsNullOrWhiteSpace(concrete.Description)
            ? description
            : $"{concrete.Description}<br/>{description}";
    }
}
