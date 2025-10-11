using Maxsys.Core.Exceptions;
using Maxsys.Core.Web.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Maxsys.Core.Web.Extensions;

/// <summary>
/// Extensões para <see cref="HttpContext"/> relacionadas à identificação de endpoints.
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// Obtém um identificador único para o endpoint atual do <see cref="HttpContext"/>, com base em um atributo personalizado
    /// ou, em sua ausência, na combinação de nome do controller e da action.
    /// </summary>
    /// <param name="context">O contexto HTTP da requisição.</param>
    /// <returns>
    /// Uma string representando o identificador do endpoint. Se o atributo <see cref="ActionIdentifierAttribute"/> estiver presente,
    /// seu valor <c>Title</c> será retornado. Caso contrário, retorna uma string no formato <c>Controller:Action</c>.
    /// </returns>
    /// <exception cref="DomainException">
    /// Lançada quando não é possível obter o nome da action ou do controller a partir das rotas.
    /// </exception>
    public static string GetEndpointIdentifier(this HttpContext? context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.GetEndpoint()?.Metadata.OfType<ActionIdentifierAttribute>().FirstOrDefault()?.Title is string title)
        {
            return title;
        }

        var actionName = context.GetRouteValue("action")?.ToString() ?? throw new DomainException("No action name found.");
        var controllerName = context.GetRouteValue("controller")?.ToString() ?? throw new DomainException("No controller name found.");

        return $"{controllerName}:{actionName}";
    }
}