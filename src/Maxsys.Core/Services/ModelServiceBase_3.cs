using System.Linq.Expressions;
using AutoMapper;
using Maxsys.Core.Events;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Data;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IModelService{TEntity, TKey}"/>
public abstract class ModelServiceBase<TEntity, TRepository, TKey>
    : ModelServiceBase<TEntity, TRepository>, IModelService<TEntity, TKey>
    where TEntity : class
    where TRepository : IRepository<TEntity>
    where TKey : notnull
{
    protected readonly IUnitOfWork _uow;
    protected readonly IMapper _mapper;

    protected abstract Expression<Func<TEntity, bool>> IdSelector(TKey id);

    protected ModelServiceBase(TRepository repository, IUnitOfWork uow, IMapper mapper)
        : base(repository)
    {
        _uow = uow;
        _mapper = mapper;
    }

    #region EVENTS

    public event OperationResultAsyncEventHandler<TEntity>? AddingAsync;

    public event OperationResultAsyncEventHandler<TEntity>? UpdatingAsync;

    public event OperationResultAsyncEventHandler<TKey>? DeletingAsync;

    public event AsyncEventHandler<AddedEntityEventArgs<TEntity, object>>? AddedAsync;

    public event AsyncEventHandler<UpdatedEntityEventArgs<TEntity, object>>? UpdatedAsync;

    public event AsyncEventHandler<ValueEventArgs>? DeletedAsync;

    #region HOOKS

    protected virtual ValueTask<OperationResult> OnBeforeAddAsync(TEntity entity, CancellationToken cancellationToken)
        => ValueTask.FromResult(Result.Success());

    protected virtual ValueTask<OperationResult> OnBeforeUpdateAsync(TEntity entity, CancellationToken cancellationToken)
        => ValueTask.FromResult(Result.Success());

    protected virtual ValueTask<OperationResult> OnBeforeDeleteAsync(TKey id, CancellationToken cancellationToken)
        => ValueTask.FromResult(Result.Success());

    protected virtual ValueTask OnAfterAddAsync(AddedEntityEventArgs<TEntity, object> e, CancellationToken cancellationToken)
        => ValueTask.CompletedTask;

    protected virtual ValueTask OnAfterUpdateAsync(UpdatedEntityEventArgs<TEntity, object> e, CancellationToken cancellationToken)
        => ValueTask.CompletedTask;

    protected virtual ValueTask OnAfterDeleteAsync(ValueEventArgs e, CancellationToken cancellationToken)
        => ValueTask.CompletedTask;

    #endregion HOOKS

    protected virtual async ValueTask<OperationResult> OnAddingAsync(TEntity e, CancellationToken cancellationToken)
    {
        var hookResult = await OnBeforeAddAsync(e, cancellationToken);
        if (!hookResult.IsValid)
            return hookResult;

        if (AddingAsync is not null)
        {
            foreach (var eventHandler in AddingAsync.GetInvocationList().Cast<OperationResultAsyncEventHandler<TEntity>>())
            {
                var result = await eventHandler(this, e, cancellationToken);
                if (!result.IsValid)
                {
                    return result;
                }
            }
        }

        return Result.Success();
    }

    protected virtual async ValueTask<OperationResult> OnUpdatingAsync(TEntity e, CancellationToken cancellationToken)
    {
        var hookResult = await OnBeforeUpdateAsync(e, cancellationToken);
        if (!hookResult.IsValid)
            return hookResult;

        if (UpdatingAsync is not null)
        {
            foreach (var eventHandler in UpdatingAsync.GetInvocationList().Cast<OperationResultAsyncEventHandler<TEntity>>())
            {
                var result = await eventHandler(this, e, cancellationToken);
                if (!result.IsValid)
                {
                    return result;
                }
            }
        }

        return Result.Success();
    }

    protected virtual async ValueTask<OperationResult> OnDeletingAsync(TKey e, CancellationToken cancellationToken)
    {
        var hookResult = await OnBeforeDeleteAsync(e, cancellationToken);
        if (!hookResult.IsValid)
            return hookResult;

        if (DeletingAsync is not null)
        {
            foreach (var eventHandler in DeletingAsync.GetInvocationList().Cast<OperationResultAsyncEventHandler<TKey>>())
            {
                var result = await eventHandler(this, e, cancellationToken);
                if (!result.IsValid)
                {
                    return result;
                }
            }
        }

        return Result.Success();
    }

    protected virtual async ValueTask OnAddedAsync(AddedEntityEventArgs<TEntity, object> e, CancellationToken cancellationToken)
    {
        await OnAfterAddAsync(e, cancellationToken);

        if (AddedAsync is not null)
        {
            foreach (var eventHandler in AddedAsync.GetInvocationList().Cast<AsyncEventHandler<AddedEntityEventArgs<TEntity, object>>>())
            {
                await eventHandler(this, e, cancellationToken);
            }
        }
    }

    protected virtual async ValueTask OnUpdatedAsync(UpdatedEntityEventArgs<TEntity, object> e, CancellationToken cancellationToken)
    {
        await OnAfterUpdateAsync(e, cancellationToken);

        if (UpdatedAsync is not null)
        {
            foreach (var eventHandler in UpdatedAsync.GetInvocationList().Cast<AsyncEventHandler<UpdatedEntityEventArgs<TEntity, object>>>())
            {
                await eventHandler(this, e, cancellationToken);
            }
        }
    }

    protected virtual async ValueTask OnDeletedAsync(ValueEventArgs e, CancellationToken cancellationToken)
    {
        await OnAfterDeleteAsync(e, cancellationToken);

        if (DeletedAsync is not null)
        {
            foreach (var eventHandler in DeletedAsync.GetInvocationList().Cast<AsyncEventHandler<ValueEventArgs>>())
            {
                await eventHandler(this, e, cancellationToken);
            }
        }
    }

    protected override void UnsubscribeEvents()
    {
        AddingAsync = null;
        UpdatingAsync = null;
        DeletingAsync = null;
        AddedAsync = null;
        UpdatedAsync = null;
        DeletedAsync = null;

        base.UnsubscribeEvents();
    }

    #endregion EVENTS

    #region ADD

    public virtual async Task<OperationResult<TCreateDTO>> AddAsync<TCreateDTO>(TCreateDTO itemToCreate, CancellationToken cancellationToken = default)
        where TCreateDTO : class
    {
        // mapeia pra entity
        var entity = _mapper.Map<TEntity>(itemToCreate);

        // antes de inserir
        var onAddingResult = await OnAddingAsync(entity, cancellationToken);
        if (!onAddingResult.IsValid)
        {
            return onAddingResult.Cast(itemToCreate);
        }

        // inserindo
        await _repository.AddAsync(entity, cancellationToken);

        // SaveChanges
        var result = await _uow.SaveChangesAsync(cancellationToken);

        // depois de inserir
        if (result.IsValid)
        {
            await OnAddedAsync(new(entity, itemToCreate), cancellationToken);
        }

        // retorna
        return result.Cast(itemToCreate);
    }

    public virtual async Task<OperationResultCollection<TCreateDTO>> AddAsync<TCreateDTO>(IEnumerable<TCreateDTO> items, bool stopOnFirstFail = true, CancellationToken cancellationToken = default)
        where TCreateDTO : class
    {
        var results = new OperationResultCollection<TCreateDTO>();

        if (stopOnFirstFail)
            await _uow.BeginTransactionAsync(null, cancellationToken);

        foreach (var item in items)
        {
            if (!stopOnFirstFail)
                await _uow.BeginTransactionAsync(null, cancellationToken);

            try
            {
                var operationResult = await AddAsync(item, cancellationToken);
                if (!operationResult.IsValid)
                {
                    await _uow.RollbackTransactionAsync(cancellationToken);
                    results.Add(operationResult);

                    if (stopOnFirstFail)
                        return results;
                    else
                        continue;
                }

                if (!stopOnFirstFail)
                    await _uow.CommitTransactionAsync(cancellationToken);

                results.Add(new(item));
            }
            catch (Exception exception)
            {
                await _uow.RollbackTransactionAsync(cancellationToken);
                results.Add(new(item, exception));

                if (stopOnFirstFail)
                    return results;
                else
                    continue;
            }
        }

        if (stopOnFirstFail)
        {
            try
            {
                await _uow.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                await _uow.RollbackTransactionAsync(cancellationToken);
                results.Add(new(default!, exception));
            }
        }

        return results;
    }

    #endregion ADD

    #region UPDATE

    public virtual async Task<OperationResult> UpdateAsync<TUpdateDTO>(TUpdateDTO itemToUpdate, CancellationToken cancellationToken = default)
        where TUpdateDTO : class, IKey<TKey>
    {
        // Obtém a entity
        var entity = await _repository.GetAsync(IdSelector(itemToUpdate.Id), @readonly: false, cancellationToken);
        if (entity is null)
        {
            return Result.Error(GenericMessages.ITEM_NOT_FOUND);
        }

        // mapeia pra entity
        _mapper.Map(itemToUpdate, entity);

        // antes de atualizar
        var onUpdatingResult = await OnUpdatingAsync(entity, cancellationToken);
        if (!onUpdatingResult.IsValid)
        {
            return onUpdatingResult;
        }

        // atualizando
        await _repository.UpdateAsync(entity, cancellationToken);

        // SaveChanges
        var result = await _uow.SaveChangesAsync(cancellationToken);

        // depois de atualizar
        if (result.IsValid)
        {
            await OnUpdatedAsync(new(entity, itemToUpdate), cancellationToken);
        }

        return result;
    }

    public virtual async Task<OperationResultCollection<TKey?>> UpdateAsync<TUpdateDTO>(IEnumerable<TUpdateDTO> itemsToUpdate, bool stopOnFirstFail = true, CancellationToken cancellationToken = default)
        where TUpdateDTO : class, IKey<TKey>
    {
        var results = new OperationResultCollection<TKey?>();

        if (stopOnFirstFail)
            await _uow.BeginTransactionAsync(null, cancellationToken);

        foreach (var item in itemsToUpdate)
        {
            if (!stopOnFirstFail)
                await _uow.BeginTransactionAsync(null, cancellationToken);

            try
            {
                var operationResult = await UpdateAsync(item, cancellationToken);
                if (!operationResult.IsValid)
                {
                    await _uow.RollbackTransactionAsync(cancellationToken);
                    results.Add(operationResult.Cast<TKey?>(item.Id));

                    if (stopOnFirstFail)
                        return results;
                    else
                        continue;
                }

                if (!stopOnFirstFail)
                    await _uow.CommitTransactionAsync(cancellationToken);

                results.Add(new(item.Id));
            }
            catch (Exception exception)
            {
                await _uow.RollbackTransactionAsync(cancellationToken);
                results.Add(new(item.Id, exception));

                if (stopOnFirstFail)
                    return results;
                else
                    continue;
            }
        }

        if (stopOnFirstFail)
        {
            try
            {
                await _uow.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                await _uow.RollbackTransactionAsync(cancellationToken);
                results.Add(new(default, exception));
            }
        }

        return results;
    }

    #endregion UPDATE

    #region DELETE

    public virtual async Task<OperationResult> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        // antes de deletar
        var onDeletingResult = await OnDeletingAsync(id, cancellationToken);
        if (!onDeletingResult.IsValid)
        {
            return onDeletingResult;
        }

        // deletando
        var entity = await _repository.GetAsync(IdSelector(id), @readonly: false, cancellationToken);
        if (entity is null)
        {
            return Result.Error(GenericMessages.ITEM_NOT_FOUND);
        }

        if (!await _repository.DeleteAsync(entity, cancellationToken))
        {
            return Result.Error(GenericMessages.ITEM_NOT_FOUND);
        }

        // SaveChanges
        var result = await _uow.SaveChangesAsync(cancellationToken);

        // depois de deletar
        if (result.IsValid)
        {
            await OnDeletedAsync(new ValueEventArgs(id), cancellationToken);
        }

        return result;
    }

    public virtual async Task<OperationResultCollection<TKey?>> DeleteAsync(IEnumerable<TKey> ids, bool stopOnFirstFail = true, CancellationToken cancellationToken = default)
    {
        var results = new OperationResultCollection<TKey?>();

        if (stopOnFirstFail)
            await _uow.BeginTransactionAsync(null, cancellationToken);

        foreach (var id in ids)
        {
            if (!stopOnFirstFail)
                await _uow.BeginTransactionAsync(null, cancellationToken);

            try
            {
                var operationResult = await DeleteAsync(id, cancellationToken);
                if (!operationResult.IsValid)
                {
                    await _uow.RollbackTransactionAsync(cancellationToken);
                    results.Add(operationResult.Cast<TKey?>(id));

                    if (stopOnFirstFail)
                        return results;
                    else
                        continue;
                }

                if (!stopOnFirstFail)
                    await _uow.CommitTransactionAsync(cancellationToken);

                results.Add(new(id));
            }
            catch (Exception exception)
            {
                await _uow.RollbackTransactionAsync(cancellationToken);
                results.Add(new(id, exception));

                if (stopOnFirstFail)
                    return results;
                else
                    continue;
            }
        }

        if (stopOnFirstFail)
        {
            try
            {
                await _uow.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                await _uow.RollbackTransactionAsync(cancellationToken);
                results.Add(new(default, exception));
            }
        }

        return results;
    }

    #endregion DELETE

    #region GET

    public virtual async Task<TDestination?> GetAsync<TDestination>(TKey id, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var item = await _repository.GetAsync<TDestination>(IdSelector(id), cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(TKey id, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetAsync(IdSelector(id), projection, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    #endregion GET

    #region LIST (InfoDTO)

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(Expression<Func<TEntity, bool>> predicate, ListCriteria criteria, CancellationToken cancellationToken = default)
        => ToListAsync<InfoDTO<TKey>>(predicate, criteria, cancellationToken);

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(Expression<Func<TEntity, bool>> predicate, Pagination? pagination, Expression<Func<InfoDTO<TKey>, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        => ToListAsync(predicate, pagination, sortSelector, sortDirection, cancellationToken);

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => ToListAsync<InfoDTO<TKey>>(predicate, new ListCriteria(), cancellationToken);

    public virtual Task<ListDTO<InfoDTO<TKey>>> GetInfoListAsync(Expression<Func<TEntity, bool>> predicate, ListCriteria criteria, CancellationToken cancellationToken = default)
        => GetListAsync<InfoDTO<TKey>>(predicate, criteria, cancellationToken);

    public virtual Task<ListDTO<InfoDTO<TKey>>> GetInfoListAsync(Expression<Func<TEntity, bool>> predicate, Pagination? pagination, Expression<Func<InfoDTO<TKey>, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        => GetListAsync(predicate, pagination, sortSelector, sortDirection, cancellationToken);

    public virtual Task<ListDTO<InfoDTO<TKey>>> GetInfoListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => GetListAsync<InfoDTO<TKey>>(predicate, cancellationToken);

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(ICollection<ColumnFilter> filters, CancellationToken cancellationToken = default)
        => ToListAsync<InfoDTO<TKey>>(filters, cancellationToken);

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(ListCriteria criteria, CancellationToken cancellationToken = default)
        => ToListAsync<InfoDTO<TKey>>(criteria, cancellationToken);

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(ICollection<ColumnFilter> filters, Pagination? pagination, Expression<Func<InfoDTO<TKey>, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        => ToListAsync(filters, pagination, sortSelector, sortDirection, cancellationToken);

    public virtual Task<ListDTO<InfoDTO<TKey>>> GetInfoListAsync(ListCriteria criteria, CancellationToken cancellationToken = default)
        => GetListAsync<InfoDTO<TKey>>(criteria, cancellationToken);

    public virtual Task<List<InfoDTO<TKey>>> ToInfoListAsync(ICollection<ColumnFilter> modelFilters, ListCriteria criteria, CancellationToken cancellationToken = default)
        => ToListAsync<InfoDTO<TKey>>(modelFilters, criteria, cancellationToken);

    public virtual Task<ListDTO<InfoDTO<TKey>>> GetInfoListAsync(ICollection<ColumnFilter> filters, CancellationToken cancellationToken = default)
        => GetListAsync<InfoDTO<TKey>>(filters, cancellationToken);

    public virtual Task<ListDTO<InfoDTO<TKey>>> GetInfoListAsync(ICollection<ColumnFilter> modelFilters, ListCriteria criteria, CancellationToken cancellationToken = default)
        => GetListAsync<InfoDTO<TKey>>(modelFilters, criteria, cancellationToken);

    #endregion LIST (InfoDTO)
}