using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IService{TKey, TFilter}"/>
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
    protected ServiceBase(
        TRepository repository,
        ISortColumnSelector<InfoDTO<TKey>> infoSortColumnsSelector,
        ISortColumnSelector<TListDTO> listSortColumnsSelector)
       : base(repository, infoSortColumnsSelector)
    {
        _listSortColumnsSelector = listSortColumnsSelector;
    }

    protected readonly ISortColumnSelector<TListDTO> _listSortColumnsSelector;

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
        return new()
        {
            Count = await _repository.CountAsync(filters, cancellationToken),
            List = await _repository.GetListAsync<TListDTO>(filters, criteria, _listSortColumnsSelector, cancellationToken)
        };
    }

    #endregion LIST
}