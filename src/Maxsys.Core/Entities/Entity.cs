namespace Maxsys.Core.Entities;

public abstract class Entity<TKey> : IKey<TKey> where TKey : notnull
{
    // TODO estudar comportamento quando required for aplicado.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public virtual TKey Id { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}