using Maxsys.Swagger.Helpers;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Maxsys.Swagger.Filters;

/// <summary>
/// Quando o parâmetro for enum, adiciona à description os detalhes dos literals.
/// Cada item do enum terá o seguinte formato:
/// <para/>
/// {valor} - {literal} ({atributo_description quando tiver})
/// </summary>
public class EnumParameterFilter : IParameterFilter
{
    public void Apply(IOpenApiParameter parameter, ParameterFilterContext context)
    {
        if (parameter is not OpenApiParameter concrete)
            return;

        if (!context.ParameterInfo.ParameterType.IsEnum)
            return;

        var referenceId = (concrete.Schema as OpenApiSchemaReference)?.Reference?.ReferenceV3;
        var typeName = context.ParameterInfo.ParameterType.Name;

        var contents = new List<string>
        {
            $"[{typeName}]({referenceId})."
        };

        contents.AddRange(EnumSwaggerHelper.GetEnumDescriptionsList(context.ParameterInfo.ParameterType));

        concrete.Description = string.Join("<br/>", contents);
    }
}
