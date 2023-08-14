using Maxsys.Core.DTO;

namespace Maxsys.Core.Interfaces.Services;

/// <summary>
/// Uma vez que a implementação dessa interface
/// utiliza-se de ProjectTo (automapper) na chamada do repositório,
/// é necessário garantir que os seguintes mapeamentos estão definidos:
/// <list type="bullet">
/// <item><description><typeparamref name="TCreateDTO"/> -> Entidade</description></item>
/// <item><description><typeparamref name="TUpdateDTO"/> -> Entidade</description></item>
/// </list>
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TCreateDTO"></typeparam>
/// <typeparam name="TUpdateDTO"></typeparam>
public interface IWriteService<TKey, TCreateDTO, TUpdateDTO> : IService
    where TKey : notnull
    where TCreateDTO : class, IDTO
    where TUpdateDTO : class, IDTO, IKey<TKey>
{
    /// <summary>
    /// Mapeamento necessário: <typeparamref name="TCreateDTO"/> -> Entidade
    /// </summary>
    Task<OperationResult<TCreateDTO>> AddAsync(TCreateDTO itemToCreate, CancellationToken cancellation = default);

    /// <summary>
    /// Mapeamento necessário: <typeparamref name="TCreateDTO"/> -> Entidade
    /// </summary>
    Task<OperationResultCollection<TCreateDTO>> AddAsync(IEnumerable<TCreateDTO> items, bool stopOnFirstFail = true, CancellationToken cancellation = default);

    /// <summary>
    /// Mapeamento necessário: <typeparamref name="TUpdateDTO"/> -> Entidade<br/>
    /// </summary>
    Task<OperationResult<TUpdateDTO>> UpdateAsync(TUpdateDTO itemToUpdate, CancellationToken cancellation = default);

    /// <summary>
    /// Mapeamento necessário: <typeparamref name="TUpdateDTO"/> -> Entidade<br/>
    /// </summary>
    Task<OperationResultCollection<TUpdateDTO>> UpdateAsync(IEnumerable<TUpdateDTO> itemsToUpdate, bool stopOnFirstFail = true, CancellationToken cancellation = default);

    Task<OperationResult<TKey>> DeleteAsync(TKey id, CancellationToken cancellation = default);

    Task<OperationResultCollection<TKey>> DeleteAsync(IEnumerable<TKey> ids, bool stopOnFirstFail = true, CancellationToken cancellation = default);
}