using Maxsys.Web.HealthCheck.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;

namespace Maxsys.Web.HealthCheck.Extensions;

public static class HealthCheckEndpointRouteBuilderExtensions
{
    /// <summary>
    /// Adiciona endpoint de health checks e um ResponseWriter personalizado.
    /// </summary>
    /// <remarks>
    /// Referências:
    ///     <para>
    ///     Health checks in ASP.NET Core: <see href="https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-8.0">Microsoft.com</see>
    ///     <br/>
    ///     Nick Chapsas on youtube: <see href="https://www.youtube.com/watch?v=p2faw9DCSsY">The Best Way to Add Health Checks in Any .NET App</see>
    ///     </para>
    /// </remarks>
    /// <param name="app">The <see cref="IEndpointRouteBuilder"/> to add the health checks endpoint to.</param>
    /// <param name="pattern">The URL pattern of the health checks endpoint.</param>
    public static IEndpointConventionBuilder UseHealthCheck(this IEndpointRouteBuilder app, string pattern = "/api/_health")
    {
        return app.MapHealthChecks(pattern, new HealthCheckOptions
        {
            ResponseWriter = HealthCheckWriter.WriteResponse
        });
    }
}