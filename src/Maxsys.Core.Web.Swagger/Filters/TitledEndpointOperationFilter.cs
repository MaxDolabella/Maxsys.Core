using Maxsys.Core.Web.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Maxsys.Core.Web.Swagger.Filters;

/// <summary>
/// Adiciona o title do endpoint (obtido do atributo <see cref="TitledActionAttribute"/>) ao summary, quando houver.
/// </summary>
public class TitledEndpointOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var attribute = context.ApiDescription.ActionDescriptor.EndpointMetadata.OfType<TitledActionAttribute>().FirstOrDefault();
        if (attribute is null)
            return;

        operation.Summary = string.IsNullOrWhiteSpace(operation.Summary) ? attribute.Title : $"{attribute.Title} | {operation.Summary}";
    }
}