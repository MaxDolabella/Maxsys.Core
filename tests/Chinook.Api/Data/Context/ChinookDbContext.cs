using Maxsys.Data;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Api.Data.Context;

public class ChinookDbContext : DbContext
{
    public ChinookDbContext(DbContextOptions<ChinookDbContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChinookDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}

public sealed class ChinookUnitOfWork(ILogger<ChinookUnitOfWork> logger, ChinookDbContext context)
    : UnitOfWorkBase<ChinookDbContext>(logger, context);
