using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Maxsys.Swagger.Filters;

/// <summary>
/// Quando o parâmetro do endpoint tiver o atributo <see cref="FromJsonAttribute"/>,
/// adiciona à sua description informação do seu tipo.
/// </summary>
public class FromJsonParameterFilter : IParameterFilter
{
    public void Apply(IOpenApiParameter parameter, ParameterFilterContext context)
    {
        if (parameter is not OpenApiParameter concrete)
            return;

        if (context.ParameterInfo.GetCustomAttributes(typeof(FromJsonAttribute), inherit: false).Length == 0)
            return;

        var referenceId = (concrete.Schema as OpenApiSchemaReference)?.Reference?.ReferenceV3;
        var typeName = context.ParameterInfo.ParameterType.Name;

        concrete.Description = $"String JSON do objeto [{typeName}]({referenceId}).<br/>Aceita valor em branco ou nulo.";
        concrete.Name = $"{concrete.Name} ({typeName})";
    }
}
