using Chinook.Api.Data.Context;
using Chinook.Api.Data.Repositories;
using Chinook.Api.Model.Repositories;
using Chinook.Api.Model.Services;
using Maxsys.Data.Extensions;
using Maxsys.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Api;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModel(configuration)
                .AddData(configuration)
                .AddMapper();

        return services;
    }

    internal static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        // Interceptors here

        services.AddDbContext<ChinookDbContext>((sp, options) =>
        {
            //options.AddInterceptors(sp.GetRequiredService<SomeInterceptor>());

            options.UseSqlite(configuration.GetConnectionString<ChinookDbContext>());

            //var _hostEnvironment = sp.GetRequiredService<IHostEnvironment>();
            //if (_hostEnvironment.IsDevelopment())
            //{
            //    options//.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))
            //      .EnableSensitiveDataLogging(_hostEnvironment.IsDevelopment())
            //          .UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() })); //.UseLoggerFactory(DbContextLoggerFactory);
            //}
        });

        services.AddUnitOfWork<ChinookUnitOfWork>();

        services.AddScoped<IArtistRepository, ArtistRepository>();

        return services;
    }

    internal static IServiceCollection AddModel(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IArtistService, ArtistService>();

        return services;
    }

    internal static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddMaxsysAutoMapper<Program>();

        return services;
    }
}
