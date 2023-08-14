namespace Maxsys.Core.Interfaces.Repositories;

/// <summary>
/// Fornece uma interface básica para tipificar um objeto como Repositório.<br/>
/// </summary>
public interface IRepository : IDisposable
{
    /// <summary>
    /// Um identificador único para o Repositório.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// O identificador único do Contexto que está sendo usado por este Repositório.
    /// </summary>
    Guid ContextId { get; }
}