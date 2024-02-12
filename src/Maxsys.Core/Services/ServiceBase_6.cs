using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IService{TKey, TListDTO, TFormDTO, TFilter}"/>
public abstract class ServiceBase<TEntity, TRepository, TKey, TListDTO, TFormDTO, TFilter>
    : ServiceBase<TEntity, TRepository, TKey, TFilter>
    , IService<TKey, TListDTO, TFormDTO, TFilter>
    where TKey : notnull
    where TEntity : class
    where TRepository : IRepository<TEntity, TFilter>
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TFilter : IFilter<TEntity>, new()
{
    protected ServiceBase(TRepository repository)
       : base(repository)
    { }

    #region GET

    public virtual async Task<TFormDTO?> GetAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await base.GetAsync<TFormDTO>(id, cancellationToken);
    }

    public virtual async Task<TFormDTO?> GetAsync(TFilter filters, CancellationToken cancellationToken = default)
    {
        return await base.GetAsync<TFormDTO>(filters, cancellationToken);
    }

    #endregion GET

    #region LIST

    public virtual async Task<ListDTO<TListDTO>> GetListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default)
    {
        return await base.GetListAsync<TListDTO>(filters, criteria, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TListDTO>> ToListAsync(TFilter filters, CancellationToken cancellationToken = default)
    {
        return await base.ToListAsync<TListDTO>(filters, cancellationToken);
    }

    #endregion LIST
}