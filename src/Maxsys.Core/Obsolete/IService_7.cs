using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;

namespace Maxsys.Core.Interfaces.Services;

/// <summary>
/// Fornece uma interface básica para obtenção e alteração de dados.<br/>
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TListDTO"></typeparam>
/// <typeparam name="TFormDTO"></typeparam>
/// <typeparam name="TCreateDTO"></typeparam>
/// <typeparam name="TUpdateDTO"></typeparam>
/// <typeparam name="TFilter"></typeparam>
[Obsolete("Use IService<TEntity, TKey, TFilter> instead.", true)]
public interface IService<TEntity, TKey, TListDTO, TFormDTO, TCreateDTO, TUpdateDTO, TFilter>
    : IService<TEntity, TKey, TListDTO, TFormDTO, TFilter>
    where TEntity : class
    where TKey : notnull
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TCreateDTO : class, IDTO
    where TUpdateDTO : class, IDTO, IKey<TKey>
    where TFilter : IFilter<TEntity>, new()
{
    /*

    #region EVENTS

    /// <summary>
    /// Evento que ocorre logo antes de adicionar um objeto ao repositório.
    /// <br/>
    /// TEventArgs é a entidade.
    /// </summary>
    event OperationResultAsyncEventHandler<TEntity>? AddingAsync;

    /// <summary>
    /// Evento que ocorre logo antes de atualizar um objeto no repositório.
    /// <br/>
    /// TEventArgs é a entidade.
    /// </summary>
    event OperationResultAsyncEventHandler<TEntity>? UpdatingAsync;

    /// <summary>
    /// Evento que ocorre logo antes de deletar um objeto do repositório.
    /// <br/>
    /// <b>Retorno</b>: <c>ValueTask</c>
    /// <br/>
    /// <b>Delegate</b>: <c>(object? sender, TKey e, CancellationToken cancellationToken)</c>
    /// </summary>
    event OperationResultAsyncEventHandler<TKey>? DeletingAsync;

    /// <summary>
    /// Evento que ocorre logo após adicionar um objeto ao repositório.
    /// </summary>
    event AsyncEventHandler<AddedEntityEventArgs<TEntity, object>>? AddedAsync;

    /// <summary>
    /// Evento que ocorre logo após atualizar um objeto no repositório.
    /// </summary>
    event AsyncEventHandler<UpdatedEntityEventArgs<TEntity, object>>? UpdatedAsync;

    /// <summary>
    /// Evento que ocorre logo após deletar um objeto do repositório.
    /// <br/>
    /// TEventArgs é a entidade.
    /// </summary>
    event AsyncEventHandler<ValueEventArgs>? DeletedAsync;

    #endregion EVENTS

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

    */
}