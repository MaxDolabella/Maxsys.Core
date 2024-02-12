using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;

namespace Maxsys.Core.Interfaces.Services;

/// <summary>
/// Fornece uma interface básica para obtenção e alteração de dados.<br/>
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TListDTO"></typeparam>
/// <typeparam name="TFormDTO"></typeparam>
/// <typeparam name="TCreateDTO"></typeparam>
/// <typeparam name="TUpdateDTO"></typeparam>
/// <typeparam name="TFilter"></typeparam>
public interface IService<TKey, TListDTO, TFormDTO, TCreateDTO, TUpdateDTO, TFilter>
    : IService<TKey, TListDTO, TFormDTO, TFilter>
    where TKey : notnull
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TCreateDTO : class, IDTO
    where TUpdateDTO : class, IDTO, IKey<TKey>
    where TFilter : IFilter, new()
{
#pragma warning disable CS1735 // XML comment has a typeparamref tag, but there is no type parameter by that name

    #region ADD

    /// <remarks>Mapeamento necessário: <typeparamref name="TCreateDTO"/> → <typeparamref name="TEntity"/></remarks>
    Task<OperationResult<TCreateDTO>> AddAsync(TCreateDTO itemToCreate, CancellationToken cancellation = default);

    /// <remarks>Mapeamento necessário: <typeparamref name="TCreateDTO"/> → <typeparamref name="TEntity"/></remarks>
    Task<OperationResultCollection<TCreateDTO>> AddAsync(IEnumerable<TCreateDTO> items, bool stopOnFirstFail = true, CancellationToken cancellation = default);

    #endregion ADD

    #region UPDATE

    /// <remarks>Mapeamento necessário: <typeparamref name="TUpdateDTO"/> → <typeparamref name="TEntity"/></remarks>
    Task<OperationResult> UpdateAsync(TUpdateDTO itemToUpdate, CancellationToken cancellation = default);

    /// <remarks>Mapeamento necessário: <typeparamref name="TUpdateDTO"/> → <typeparamref name="TEntity"/></remarks>
    Task<OperationResultCollection<TKey?>> UpdateAsync(IEnumerable<TUpdateDTO> itemsToUpdate, bool stopOnFirstFail = true, CancellationToken cancellation = default);

    #endregion UPDATE

    #region DELETE

    Task<OperationResult> DeleteAsync(TKey id, CancellationToken cancellation = default);

    Task<OperationResultCollection<TKey?>> DeleteAsync(IEnumerable<TKey> ids, bool stopOnFirstFail = true, CancellationToken cancellation = default);

    #endregion DELETE

#pragma warning restore CS1735 // XML comment has a typeparamref tag, but there is no type parameter by that name
}