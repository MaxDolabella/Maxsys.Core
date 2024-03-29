using Maxsys.Core.Entities;
using Maxsys.Core.Messaging.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Maxsys.Core.Data.Extensions;

public static class BusExtensions
{
    public static async Task DispatchDomainEventsAsync(this IBus mediator, DbContext context)
    {
        var domainEntities = context.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Count > 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(x => x.Entity.ClearDomainEvent());

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}