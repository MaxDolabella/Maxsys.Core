using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Maxsys.ModelCore.Filtering;
using Maxsys.ModelCore.Listing;

namespace Maxsys.ModelCore.Services;

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
/// Uma vez que a implementação dessa interface
/// utiliza-se de ProjectTo (automapper) na chamada do repositório,
/// é necessário garantir que os seguintes mapeamentos estão definidos:
/// <list type="bullet">
/// <item><description>Entidade -> <typeparamref name="TFormDTO"/></description></item>
/// <item><description>Entidade -> <typeparamref name="TListDTO"/></description></item>
/// <item><description>Entidade -> <typeparamref name="TInfoDTO"/></description></item>
/// </list>
/// </summary>
/// <typeparam name="TListDTO"></typeparam>
/// <typeparam name="TFormDTO"></typeparam>
/// <typeparam name="TInfoDTO"></typeparam>
/// <typeparam name="TFilter"></typeparam>
public interface IReadService<TListDTO, TFormDTO, TInfoDTO, TFilter> : IService
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TInfoDTO : class, IDTO
    where TFilter : IFilter
{
    /// <summary>
    /// Mapeamento necessário: Entidade -> <typeparamref name="TFormDTO"/>
    /// </summary>
    Task<TFormDTO?> GetAsync(object id, CancellationToken cancellation = default);

    /// <summary>
    /// Mapeamento necessário: Entidade -> <typeparamref name="TListDTO"/>
    /// </summary>
    Task<ListDTO<TListDTO>> GetListAsync(TFilter filters, Criteria criteria, CancellationToken cancellation = default);

    /// <summary>
    /// Mapeamento necessário: Entidade -> <typeparamref name="TInfoDTO"/>
    /// </summary>
    Task<ListDTO<TInfoDTO>> GetInfoAsync(TFilter filters, Criteria criteria, CancellationToken cancellation = default);
}

/// <summary>
/// Uma vez que a implementação dessa interface
/// utiliza-se de ProjectTo (automapper) na chamada do repositório,
/// é necessário garantir que os seguintes mapeamentos estão definidos:
/// <list type="bullet">
/// <item><description>Entidade -> <typeparamref name="TFormDTO"/></description></item>
/// <item><description>Entidade -> <typeparamref name="TListDTO"/></description></item>
/// <item><description>Entidade -> <typeparamref name="TInfoDTO"/></description></item>
/// <item><description><typeparamref name="TCreateDTO"/> -> Entidade</description></item>
/// <item><description><typeparamref name="TUpdateDTO"/> -> Entidade</description></item>
/// </list>
/// </summary>
/// <typeparam name="TListDTO"></typeparam>
/// <typeparam name="TFormDTO"></typeparam>
/// <typeparam name="TInfoDTO"></typeparam>
/// <typeparam name="TCreateDTO"></typeparam>
/// <typeparam name="TUpdateDTO"></typeparam>
/// <typeparam name="TFilter"></typeparam>
public interface IService<TListDTO, TFormDTO, TInfoDTO, TCreateDTO, TUpdateDTO, TFilter>
    : IReadService<TListDTO, TFormDTO, TInfoDTO, TFilter>
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TInfoDTO : class, IDTO
    where TCreateDTO : class, IDTO
    where TUpdateDTO : class, IDTO
    where TFilter : IFilter
{
    /// <summary>
    /// Mapeamento necessário: <typeparamref name="TCreateDTO"/> -> Entidade
    /// </summary>
    Task<ValidationResult> AddAsync(TCreateDTO itemToCreate, CancellationToken cancellation = default);

    /// <summary>
    /// Mapeamento necessário: <typeparamref name="TUpdateDTO"/> -> Entidade
    /// </summary>
    Task<ValidationResult> UpdateAsync(object id, TUpdateDTO itemToUpdate, CancellationToken cancellation = default);

    Task<ValidationResult> DeleteAsync(object id, CancellationToken cancellation = default);
}