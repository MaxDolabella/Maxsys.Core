using Maxsys.Core.Web.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Maxsys.Core.Web.Swagger.Filters;

/// <summary>
/// Adiciona o title do endpoint (obtido do atributo <see cref="ActionIdentifierAttribute"/>) ao summary, quando houver.
/// </summary>
public class IdentifiedEndpointOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var attribute = context.ApiDescription.ActionDescriptor.EndpointMetadata.OfType<ActionIdentifierAttribute>().FirstOrDefault();
        if (attribute is null)
            return;

        operation.Summary = string.IsNullOrWhiteSpace(operation.Summary) ? attribute.Title : $"{attribute.Title} | {operation.Summary}";
    }
}