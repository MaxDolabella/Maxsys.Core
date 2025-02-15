﻿using System.Linq.Expressions;
using Maxsys.Core.Events;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Interfaces.Services;

/// <summary>
/// Fornece uma interface básica para obtenção de dados.<br/>
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IService<TEntity, TKey> : IService<TEntity>
    where TEntity : class
    where TKey : notnull
{
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
    Task<OperationResult<TCreateDTO>> AddAsync<TCreateDTO>(TCreateDTO itemToCreate, CancellationToken cancellation = default) where TCreateDTO : class;

    /// <remarks>Mapeamento necessário: <typeparamref name="TCreateDTO"/> → <typeparamref name="TEntity"/></remarks>
    Task<OperationResultCollection<TCreateDTO>> AddAsync<TCreateDTO>(IEnumerable<TCreateDTO> items, bool stopOnFirstFail = true, CancellationToken cancellation = default) where TCreateDTO : class;

    #endregion ADD

    #region UPDATE

    /// <remarks>Mapeamento necessário: <typeparamref name="TUpdateDTO"/> → <typeparamref name="TEntity"/></remarks>
    Task<OperationResult> UpdateAsync<TUpdateDTO>(TUpdateDTO itemToUpdate, CancellationToken cancellation = default) where TUpdateDTO : class, IKey<TKey>;

    /// <remarks>Mapeamento necessário: <typeparamref name="TUpdateDTO"/> → <typeparamref name="TEntity"/></remarks>
    Task<OperationResultCollection<TKey?>> UpdateAsync<TUpdateDTO>(IEnumerable<TUpdateDTO> itemsToUpdate, bool stopOnFirstFail = true, CancellationToken cancellation = default) where TUpdateDTO : class, IKey<TKey>;

    #endregion UPDATE

    #region DELETE

    Task<OperationResult> DeleteAsync(TKey id, CancellationToken cancellation = default);

    Task<OperationResultCollection<TKey?>> DeleteAsync(IEnumerable<TKey> ids, bool stopOnFirstFail = true, CancellationToken cancellation = default);

    #endregion DELETE

    #region GET

    Task<TDestination?> GetAsync<TDestination>(TKey id, CancellationToken cancellationToken = default);

    Task<TDestination?> GetAsync<TDestination>(TKey id, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default);

    #endregion GET

    #region LIST

    Task<List<InfoDTO<TKey>>> ToInfoListAsync(Expression<Func<TEntity, bool>> predicate, ListCriteria criteria, CancellationToken cancellationToken = default);

    Task<List<InfoDTO<TKey>>> ToInfoListAsync(Expression<Func<TEntity, bool>> predicate, Pagination? pagination, Expression<Func<InfoDTO<TKey>, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);

    Task<List<InfoDTO<TKey>>> ToInfoListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    #endregion LIST
}