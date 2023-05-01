using System;

namespace Maxsys.Core;

/// <summary>
/// Provides a base class for an Entity of key (Key) type <see cref="Guid"/>
/// </summary>
public abstract class EntityBase : Entity<Guid>
{
    public override Guid Id { get; set; } = Guid.NewGuid();
}