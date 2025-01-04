using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Maxsys.Core.Web.Swagger.Filters;

/// <summary>
/// Quando o parâmetro do endpoint tiver o atributo <see cref="FromJsonAttribute"/>,
/// adiciona à sua description informação do seu tipo.
/// </summary>
public class FromJsonParameterFilter : IParameterFilter
{
    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        if (context.ParameterInfo.GetCustomAttribute<FromJsonAttribute>() != null)
        {
            var referenceId = parameter.Schema.Reference.ReferenceV3;
            var typeName = context.ParameterInfo.ParameterType.Name;
            parameter.Description = $"String JSON do objeto [{typeName}]({referenceId}).<br/>Aceita valor em branco ou nulo.";
            parameter.Name = $"{parameter.Name} ({typeName})";
        }
    }
}