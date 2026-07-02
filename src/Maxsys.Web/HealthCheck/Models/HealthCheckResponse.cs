namespace Maxsys.Web.HealthCheck;

public record HealthCheckResponse(string Service, string Status, string? Description);