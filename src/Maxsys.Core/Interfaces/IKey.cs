namespace Maxsys.Core;

/// <summary>
/// Interface para um objeto que contenha uma chave (<see cref="Id"/>) do tipo não nulo
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IKey<TKey>
{
    /// <summary>
    /// Id do objeto.
    /// </summary>
    TKey Id { get; }
}