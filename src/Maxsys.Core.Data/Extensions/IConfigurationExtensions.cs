using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Maxsys.Core.Data.Extensions;

public static class IConfigurationExtensions
{
    /// <summary>
    /// Gets the connection string with the same name of DbContext.
    /// Shorthand for <c>GetSection("ConnectionStrings")[typeof(TContext).Name]</c>.
    /// <para/>
    /// Use:
    /// <code>
    /// void ConfigureContext(this IServiceCollection services, IConfiguration configuration)
    /// {
    ///     var conn = configuration.GetConnectionString&lt;YourAppContext&gt;();
    ///     services.AddDbContext&lt;YourAppContext&gt;(options =>
    ///     {
    ///         options.UseSqlServer(conn);
    ///     }
    /// }
    /// </code>
    /// </summary>
    /// <param name="configuration">The configuration to enumerate.</param>
    /// <typeparam name="TContext">Is the Context</typeparam>
    /// <returns>The connection string.</returns>
    public static string? GetConnectionString<TContext>(this IConfiguration configuration)
        where TContext : DbContext
    {
        return configuration.GetConnectionString(typeof(TContext).Name);
    }
}