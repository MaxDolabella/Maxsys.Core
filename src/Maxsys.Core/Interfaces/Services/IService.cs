namespace Maxsys.Core.Interfaces.Services;

/// <summary>
/// Fornece uma interface básica para tipificar um objeto como Service.<br/>
/// </summary>
public interface IService : IDisposable
{
    /// <summary>
    /// Um identificador único para o Service.
    /// </summary>
    Guid Id { get; }
}