using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Maxsys.Core.Filtering;
using Maxsys.Core.Listing;

namespace Maxsys.Core.Services;

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

/// <summary>
/// Fornece uma interface para utilização de um service que recupera dados do repositório e retorna nos mais variados objetos.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TListDTO"></typeparam>
/// <typeparam name="TFormDTO"></typeparam>
/// <typeparam name="TInfoDTO"></typeparam>
/// <typeparam name="TFilter"></typeparam>
/// <remarks>
/// Uma vez que a implementação dessa interface
/// utiliza-se de ProjectTo (automapper) na chamada do repositório,
/// é necessário garantir que os seguintes mapeamentos estão definidos: <br/>
/// Entidade ➔ <typeparamref name="TFormDTO"/><br/>
/// Entidade ➔ <typeparamref name="TListDTO"/><br/>
/// Entidade ➔ <typeparamref name="TInfoDTO"/>
/// </remarks>
public interface IService<TKey, TListDTO, TFormDTO, TInfoDTO, TFilter> : IService
    where TKey : notnull
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TInfoDTO : class, IDTO
    where TFilter : IFilter
{
    /// <summary>
    /// Mapeamento necessário: Entidade ➔ <typeparamref name="TFormDTO"/>
    /// </summary>
    Task<TFormDTO?> GetAsync(TKey id, CancellationToken cancellation = default);

    /// <summary>
    /// Mapeamento necessário: Entidade ➔ <typeparamref name="TListDTO"/>
    /// </summary>
    Task<ListDTO<TListDTO>> GetListAsync(TFilter filters, Criteria criteria, CancellationToken cancellation = default);

    /// <summary>
    /// Mapeamento necessário: Entidade ➔ <typeparamref name="TInfoDTO"/>
    /// </summary>
    Task<ListDTO<TInfoDTO>> GetInfoAsync(TFilter filters, Criteria criteria, CancellationToken cancellation = default);
}

/// <summary>
/// Fornece uma interface para utilização de um service que recupera dados do repositório e retorna nos mais variados objetos.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TListDTO"></typeparam>
/// <typeparam name="TFormDTO"></typeparam>
/// <typeparam name="TInfoDTO"></typeparam>
/// <typeparam name="TCreateDTO"></typeparam>
/// <typeparam name="TUpdateDTO"></typeparam>
/// <typeparam name="TFilter"></typeparam>
/// <remarks>
/// Uma vez que a implementação dessa interface
/// utiliza-se de ProjectTo (automapper) na chamada do repositório,
/// é necessário garantir que os seguintes mapeamentos estão definidos: <br/>
/// Entidade ➔ <typeparamref name="TFormDTO"/><br/>
/// Entidade ➔ <typeparamref name="TListDTO"/><br/>
/// Entidade ➔ <typeparamref name="TInfoDTO"/><br/>
/// Entidade ➔ <typeparamref name="TUpdateDTO"/><br/>
/// <typeparamref name="TUpdateDTO"/> ➔ Entidade<br/>
/// <typeparamref name="TCreateDTO"/> ➔ Entidade<br/>
/// </remarks>
public interface IService<TKey, TListDTO, TFormDTO, TInfoDTO, TCreateDTO, TUpdateDTO, TFilter>
    : IService<TKey, TListDTO, TFormDTO, TInfoDTO, TFilter>
    where TKey : notnull
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TInfoDTO : class, IDTO
    where TCreateDTO : class, IDTO
    where TUpdateDTO : class, IDTO<TKey>
    where TFilter : IFilter
{
    /// <summary>
    /// Mapeamento necessário: Entidade ➔ <typeparamref name="TUpdateDTO"/>
    /// </summary>
    Task<TUpdateDTO?> GetToEditAsync(TKey id, CancellationToken cancellation = default);

    /// <summary>
    /// Mapeamento necessário: <typeparamref name="TCreateDTO"/> ➔ Entidade
    /// </summary>
    Task<ValidationResult> AddAsync(TCreateDTO itemToCreate, CancellationToken cancellation = default);

    /// <summary>
    /// Mapeamento necessário: <typeparamref name="TUpdateDTO"/> ➔ Entidade
    /// </summary>
    Task<ValidationResult> UpdateAsync(TUpdateDTO itemToUpdate, CancellationToken cancellation = default);

    Task<ValidationResult> DeleteAsync(TKey id, CancellationToken cancellation = default);
}