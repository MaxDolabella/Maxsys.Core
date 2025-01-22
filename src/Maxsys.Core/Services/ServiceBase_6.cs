using System.Linq.Expressions;
using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Services;



/// <inheritdoc cref="IService{TEntity, TKey, TListDTO, TFormDTO, TFilter}"/>
[Obsolete("Use ServiceBase<TEntity, TRepository, TKey, TFilter> instead.", true)]
public abstract class ServiceBase<TEntity, TRepository, TKey, TListDTO, TFormDTO, TFilter>
    : ServiceBase<TEntity, TRepository, TKey, TFilter>
    , IService<TEntity, TKey, TListDTO, TFormDTO, TFilter>
    where TKey : notnull
    where TEntity : class
    where TRepository : IRepository<TEntity, TFilter>
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TFilter : IFilter<TEntity>, new()
{

    protected ServiceBase(TRepository repository)
       : base(repository, null, null)
    { }
    /*

    #region GET

    public virtual Task<TFormDTO?> GetAsync(TKey id, CancellationToken cancellationToken = default)
        => base.GetAsync<TFormDTO>(id, cancellationToken);

    public virtual Task<TFormDTO?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => base.GetAsync<TFormDTO>(predicate, cancellationToken);

    public virtual Task<TFormDTO?> GetAsync(TFilter filters, CancellationToken cancellationToken = default)
        => base.GetAsync<TFormDTO>(filters, cancellationToken);

    #endregion GET

    #region LIST

    public virtual Task<ListDTO<TListDTO>> GetListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default)
        => base.GetListAsync<TListDTO>(filters, criteria, cancellationToken);

    public virtual Task<List<TListDTO>> ToListAsync(TFilter filters, CancellationToken cancellationToken = default)
        => base.ToListAsync<TListDTO>(filters, cancellationToken);

    public Task<List<TListDTO>> ToListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => base.ToListAsync<TListDTO>(predicate, cancellationToken);

    public Task<List<TListDTO>> ToListAsync(Expression<Func<TEntity, bool>> predicate, ListCriteria criteria, CancellationToken cancellationToken = default)
        => base.ToListAsync<TListDTO>(predicate, criteria, cancellationToken);

    public Task<List<TListDTO>> ToListAsync(Expression<Func<TEntity, bool>> predicate, Pagination? pagination, Expression<Func<TListDTO, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        => base.ToListAsync(predicate, pagination, sortSelector, sortDirection, cancellationToken);

    public Task<List<TListDTO>> ToListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default)
        => base.ToListAsync<TListDTO>(filters, criteria, cancellationToken);

    public Task<List<TListDTO>> ToListAsync(TFilter filters, Pagination? pagination, Expression<Func<TListDTO, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        => base.ToListAsync(filters, pagination, sortSelector, sortDirection, cancellationToken);

    #endregion LIST
    */
}
