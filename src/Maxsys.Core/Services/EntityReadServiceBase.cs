using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IEntityReadService{TKey, TListDTO, TFormDTO, TFilter}"/>
public abstract class EntityReadServiceBase<TEntity, TKey, TRepository, TListDTO, TFormDTO, TFilter>
    : ReadServiceBase<TEntity, TRepository, TFilter>
    , IEntityReadService<TKey, TListDTO, TFormDTO, TFilter>
    where TKey : notnull
    where TEntity : class
    where TRepository : IRepository<TEntity, TFilter>
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TFilter : IFilter<TEntity>, new()
{
    protected readonly ISortColumnSelector<InfoDTO<TKey>> _infoSortColumnsSelector;
    protected readonly ISortColumnSelector<TListDTO> _listSortColumnsSelector;

    /// <summary/>
    public EntityReadServiceBase(TRepository repository, ISortColumnSelector<InfoDTO<TKey>> infoSortColumnsSelector, ISortColumnSelector<TListDTO> listSortColumnsSelector)
        : base(repository)
    {
        _infoSortColumnsSelector = infoSortColumnsSelector;
        _listSortColumnsSelector = listSortColumnsSelector;
    }

    public virtual async Task<ListDTO<InfoDTO<TKey>>> GetInfoAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellation = default)
    {
        return await base.GetListAsync<InfoDTO<TKey>>(filters, criteria, _infoSortColumnsSelector, cancellation);
    }

    public virtual async Task<ListDTO<TListDTO>> GetListAsync(TFilter filters, ListCriteria criteria, CancellationToken cancellation = default)
    {
        return await base.GetListAsync(filters, criteria, _listSortColumnsSelector, cancellation);
    }

    public virtual async Task<TFormDTO?> GetAsync(object id, CancellationToken cancellation = default)
    {
        return await base.GetAsync<TFormDTO>(id, cancellation);
    }
}