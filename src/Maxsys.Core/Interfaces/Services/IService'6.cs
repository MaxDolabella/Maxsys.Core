using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;

namespace Maxsys.Core.Interfaces.Services;

public interface IService<TKey, TListDTO, TFormDTO, TCreateDTO, TUpdateDTO, TFilter>
    : IService<TKey, TListDTO, TFormDTO, TFilter>
    where TKey : notnull
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TCreateDTO : class, IDTO
    where TUpdateDTO : class, IDTO, IKey<TKey>
    where TFilter : IFilter, new()
{
    #region ADD

    /// <summary>
    /// Mapeamento necessário: <typeparamref name="TCreateDTO"/> -> Entidade
    /// </summary>
    Task<OperationResult<TCreateDTO>> AddAsync(TCreateDTO itemToCreate, CancellationToken cancellation = default);

    /// <summary>
    /// Mapeamento necessário: <typeparamref name="TCreateDTO"/> -> Entidade
    /// </summary>
    Task<OperationResultCollection<TCreateDTO>> AddAsync(IEnumerable<TCreateDTO> items, bool stopOnFirstFail = true, CancellationToken cancellation = default);

    #endregion ADD

    #region UPDATE

    /// <summary>
    /// Mapeamento necessário: <typeparamref name="TUpdateDTO"/> -> Entidade<br/>
    /// </summary>
    Task<OperationResult<TUpdateDTO>> UpdateAsync(TUpdateDTO itemToUpdate, CancellationToken cancellation = default);

    /// <summary>
    /// Mapeamento necessário: <typeparamref name="TUpdateDTO"/> -> Entidade<br/>
    /// </summary>
    Task<OperationResultCollection<TUpdateDTO>> UpdateAsync(IEnumerable<TUpdateDTO> itemsToUpdate, bool stopOnFirstFail = true, CancellationToken cancellation = default);

    #endregion UPDATE

    #region DELETE

    Task<OperationResult<TKey>> DeleteAsync(TKey id, CancellationToken cancellation = default);

    Task<OperationResultCollection<TKey>> DeleteAsync(IEnumerable<TKey> ids, bool stopOnFirstFail = true, CancellationToken cancellation = default);

    #endregion DELETE
}