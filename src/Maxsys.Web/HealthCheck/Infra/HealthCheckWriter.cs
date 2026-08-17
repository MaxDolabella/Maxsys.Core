using Maxsys.Core;
using Maxsys.Core.Extensions;
using Maxsys.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Maxsys.Web.HealthCheck.Infra;

internal static class HealthCheckWriter
{
    internal static async Task WriteResponse(HttpContext context, HealthReport healthReport)
    {
        var data = healthReport.Entries.Select(x => new HealthCheckResponse(x.Key, x.Value.Status.ToString(), x.Value.Description)).ToList();

        var (statusCode, resultType) = healthReport.Status is HealthStatus.Unhealthy or HealthStatus.Degraded
            ? (StatusCodes.Status503ServiceUnavailable, ResultTypes.Warning)
            : (StatusCodes.Status200OK, ResultTypes.Success);

        var apiResult = new ApiResult<List<HealthCheckResponse>>("HealthCheck", statusCode, resultType, data);
        var json = apiResult.ToJson()!;

        context.Response.ContentType = "application/json; charset=utf-8";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(json);
    }
}