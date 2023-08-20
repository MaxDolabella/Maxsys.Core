using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IService{TKey, TFilter}"/>
public abstract class ServiceBase<TEntity, TRepository, TKey, TDTO, TFilter>
    : ServiceBase<TEntity, TRepository, TKey, TDTO, TDTO, TFilter>
    , IService<TKey, TDTO, TFilter>
    where TKey : notnull
    where TEntity : class
    where TRepository : IRepository<TEntity, TFilter>
    where TDTO : class, IDTO
    where TFilter : IFilter<TEntity>, new()
{
    protected ServiceBase(
        TRepository repository,
        ISortColumnSelector<InfoDTO<TKey>> infoSortColumnsSelector,
        ISortColumnSelector<TDTO> listSortColumnsSelector)
       : base(repository, infoSortColumnsSelector, listSortColumnsSelector)
    { }
}