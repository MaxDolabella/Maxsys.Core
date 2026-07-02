using System.Linq.Expressions;
using Maxsys.Core.Events;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IModelService{TEntity}"/>
public abstract class ModelServiceBase<TEntity, TRepository>
    : ServiceBase, IModelService<TEntity>
    where TEntity : class
    where TRepository : IRepository<TEntity>
{
    protected readonly TRepository _repository;

    protected ModelServiceBase(TRepository repository) : base()
    {
        _repository = repository;
    }

    #region EVENTS

    public event AsyncEventHandler<ValueEventArgs>? GetCompletedAsync;

    public event AsyncEventHandler<ValueEventArgs>? ToListCompletedAsync;

    public event AsyncEventHandler<ValueEventArgs>? GetListCompletedAsync;

    #region HOOKS

    protected virtual ValueTask OnAfterGetAsync(object? result, CancellationToken cancellationToken)
        => ValueTask.CompletedTask;

    protected virtual ValueTask OnAfterToListAsync(object? result, CancellationToken cancellationToken)
        => ValueTask.CompletedTask;

    protected virtual ValueTask OnAfterGetListAsync(object? result, CancellationToken cancellationToken)
        => ValueTask.CompletedTask;

    #endregion HOOKS

    protected virtual async ValueTask OnGetCompletedAsync(object? e, CancellationToken cancellationToken)
    {
        await OnAfterGetAsync(e, cancellationToken);

        if (GetCompletedAsync is not null)
        {
            foreach (var eventHandler in GetCompletedAsync.GetInvocationList().Cast<AsyncEventHandler<ValueEventArgs>>())
            {
                await eventHandler(this, new ValueEventArgs(e), cancellationToken);
            }
        }
    }

    protected virtual async ValueTask OnToListCompletedAsync(object? e, CancellationToken cancellationToken)
    {
        await OnAfterToListAsync(e, cancellationToken);

        if (ToListCompletedAsync is not null)
        {
            foreach (var eventHandler in ToListCompletedAsync.GetInvocationList().Cast<AsyncEventHandler<ValueEventArgs>>())
            {
                await eventHandler(this, new ValueEventArgs(e), cancellationToken);
            }
        }
    }

    protected virtual async ValueTask OnGetListCompletedAsync(object? e, CancellationToken cancellationToken)
    {
        await OnAfterGetListAsync(e, cancellationToken);

        if (GetListCompletedAsync is not null)
        {
            foreach (var eventHandler in GetListCompletedAsync.GetInvocationList().Cast<AsyncEventHandler<ValueEventArgs>>())
            {
                await eventHandler(this, new ValueEventArgs(e), cancellationToken);
            }
        }
    }

    protected virtual void UnsubscribeEvents()
    {
        GetCompletedAsync = null;
        ToListCompletedAsync = null;
        GetListCompletedAsync = null;
    }

    #endregion EVENTS

    #region GET

    public virtual async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var item = await _repository.GetAsync<TDestination>(predicate, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        var item = await _repository.GetAsync(predicate, projection, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetByIdAsync<TDestination>(object[] ids, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var item = await _repository.GetByIdAsync<TDestination>(ids, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetSingleOrDefaultAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var item = await _repository.GetSingleOrDefaultAsync<TDestination>(predicate, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetSingleOrThrowsAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var item = await _repository.GetSingleOrThrowsAsync<TDestination>(predicate, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    #endregion GET

    #region LIST

    // List

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var items = await _repository.ToListAsync<TDestination>(predicate, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var items = await _repository.ToListAsync<TDestination>(predicate, criteria, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var items = await _repository.ToListAsync(predicate, pagination, sortSelector, sortDirection, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        var items = await _repository.ToListAsync(projection, predicate, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
    {
        var items = await _repository.ToListAsync(projection, predicate, pagination, sortSelector, sortDirection, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var items = await _repository.ToListAsync(projection, predicate, criteria, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var items = await _repository.ToListAsync<TDestination>(criteria, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    // ListDTO
    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var list = new ListDTO<TDestination>()
        {
            Count = await _repository.CountAsync(predicate, cancellationToken),
            Items = await _repository.ToListAsync<TDestination>(predicate, cancellationToken)
        };

        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var list = new ListDTO<TDestination>()
        {
            Count = await _repository.CountAsync<TDestination>(predicate, criteria, cancellationToken),
            Items = await _repository.ToListAsync<TDestination>(predicate, criteria, cancellationToken)
        };

        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var list = new ListDTO<TDestination>()
        {
            Count = await _repository.CountAsync(predicate, cancellationToken),
            Items = await _repository.ToListAsync<TDestination>(predicate, pagination, sortSelector, sortDirection, cancellationToken)
        };

        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
    {
        var list = new ListDTO<TDestination>()
        {
            Count = await _repository.CountAsync(predicate, cancellationToken),
            Items = await _repository.ToListAsync<TDestination>(projection, predicate, cancellationToken)
        };

        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
    {
        var list = new ListDTO<TDestination>()
        {
            Count = await _repository.CountAsync(predicate, cancellationToken),
            Items = await _repository.ToListAsync<TDestination>(projection, predicate, pagination, sortSelector, sortDirection, cancellationToken)
        };

        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var list = new ListDTO<TDestination>()
        {
            Count = await _repository.CountAsync<TDestination>(predicate, criteria, cancellationToken),
            Items = await _repository.ToListAsync<TDestination>(projection, predicate, criteria, cancellationToken)
        };

        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var list = new ListDTO<TDestination>()
        {
            Count = await _repository.CountAsync<TDestination>(criteria, cancellationToken),
            Items = await _repository.ToListAsync<TDestination>(criteria, cancellationToken)
        };

        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }


    #endregion LIST

    #region UTIL

    public virtual ValueTask<bool> IdExistsAsync(object[] ids, CancellationToken cancellationToken = default)
        => _repository.IdExistsAsync(ids, cancellationToken);

    public virtual ValueTask<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return _repository.CountAsync(predicate, cancellationToken);
    }

    public virtual ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return _repository.AnyAsync(predicate, cancellationToken);
    }

    public virtual ValueTask<int> CountAsync<TDestination>(ICollection<ColumnFilter> filters, CancellationToken cancellationToken = default)
        where TDestination : class
        => _repository.CountAsync<TDestination>(filters, cancellationToken);

    public virtual ValueTask<bool> AnyAsync<TDestination>(ICollection<ColumnFilter> filters, CancellationToken cancellationToken = default)
        where TDestination : class
        => _repository.AnyAsync<TDestination>(filters, cancellationToken);

    #endregion UTIL

    #region GET - ColumnFilters

    public virtual async Task<TDestination?> GetAsync<TDestination>(ICollection<ColumnFilter> filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var item = await _repository.GetAsync<TDestination>(filters, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetAsync<TDestination>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var item = await _repository.GetAsync(filters, projection, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetSingleOrDefaultAsync<TDestination>(ICollection<ColumnFilter> filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var item = await _repository.GetSingleOrDefaultAsync<TDestination>(filters, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    public virtual async Task<TDestination?> GetSingleOrThrowsAsync<TDestination>(ICollection<ColumnFilter> filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var item = await _repository.GetSingleOrThrowsAsync<TDestination>(filters, cancellationToken);

        await OnGetCompletedAsync(item, cancellationToken);

        return item;
    }

    #endregion GET - ColumnFilters

    #region LIST - ColumnFilters

    // List

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(ICollection<ColumnFilter> filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var items = await _repository.ToListAsync<TDestination>(filters, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(ICollection<ColumnFilter> filters, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var items = await _repository.ToListAsync(filters, pagination, sortSelector, sortDirection, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var items = await _repository.ToListAsync(filters, projection, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, TDestination>> projection, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var items = await _repository.ToListAsync(projection, criteria, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TDestination>> projection, Pagination? pagination, Expression<Func<TDestination, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var items = await _repository.ToListAsync(filters, projection, pagination, keySelector, sortDirection, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(ICollection<ColumnFilter> modelFilters, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var items = await _repository.ToListAsync<TDestination>(modelFilters, criteria, cancellationToken);

        await OnToListCompletedAsync(items, cancellationToken);

        return items;
    }

    // ListDTO
    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(ICollection<ColumnFilter> filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var list = new ListDTO<TDestination>()
        {
            Count = await _repository.CountAsync<TDestination>(filters, cancellationToken),
            Items = await _repository.ToListAsync<TDestination>(filters, cancellationToken)
        };

        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(ICollection<ColumnFilter> filters, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var list = new ListDTO<TDestination>()
        {
            Count = await _repository.CountAsync<TDestination>(filters, cancellationToken),
            Items = await _repository.ToListAsync<TDestination>(filters, pagination, sortSelector, sortDirection, cancellationToken)
        };

        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var list = new ListDTO<TDestination>()
        {
            Count = await _repository.CountAsync<TDestination>(filters, cancellationToken),
            Items = await _repository.ToListAsync<TDestination>(filters, projection, cancellationToken)
        };

        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, TDestination>> projection, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var list = new ListDTO<TDestination>()
        {
            Count = await _repository.CountAsync<TDestination>(criteria, cancellationToken),
            Items = await _repository.ToListAsync<TDestination>(projection, criteria, cancellationToken)
        };

        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TDestination>> projection, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var list = new ListDTO<TDestination>()
        {
            Count = await _repository.CountAsync<TDestination>(filters, cancellationToken),
            Items = await _repository.ToListAsync<TDestination>(filters, projection, pagination, sortSelector, sortDirection, cancellationToken)
        };

        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }

    public virtual async Task<ListDTO<TDestination>> GetListAsync<TDestination>(ICollection<ColumnFilter> modelFilters, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var list = new ListDTO<TDestination>()
        {
            Count = await _repository.CountAsync<TDestination>(modelFilters, criteria, cancellationToken),
            Items = await _repository.ToListAsync<TDestination>(modelFilters, criteria, cancellationToken)
        };

        await OnGetListCompletedAsync(list, cancellationToken);

        return list;
    }

    #endregion LIST - ColumnFilters
}