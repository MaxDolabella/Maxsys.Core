using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;

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

public interface IService<TKey, TListDTO, TFormDTO, TCreateDTO, TUpdateDTO, TFilter>
    : IEntityReadService<TKey, TListDTO, TFormDTO, TFilter>
    , IWriteService<TKey, TCreateDTO, TUpdateDTO>
    where TKey : notnull
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TCreateDTO : class, IDTO
    where TUpdateDTO : class, IDTO, IKey<TKey>
    where TFilter : IFilter, new()
{ }