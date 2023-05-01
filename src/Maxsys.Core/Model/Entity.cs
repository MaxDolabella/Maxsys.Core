namespace Maxsys.Core;

/// <summary>
/// Provides a base class for an Entity of key type <typeparamref name="TKey"/>
/// </summary>
/// <typeparam name="TKey">Type of key (Key). <para/>Example: TKey=Guid => public Guid Key { get; protected set; }</typeparam>
public abstract class Entity<TKey> : IKey<TKey> where TKey : notnull
{
    public virtual TKey Id { get; set; }
}

/// <summary>
/// Interface para um objeto que contenha uma chave (<see cref="Id"/>) do tipo não nulo
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IKey<TKey> where TKey : notnull
{
    /// <summary>
    /// Id do objeto.
    /// </summary>
    TKey Id { get; }
}