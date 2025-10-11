using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;

namespace Maxsys.Core.Services;

[Obsolete("Use ServiceBase<TEntity, TRepository, TKey, TFilter> instead.", true)]
public abstract class ServiceBase<TEntity, TRepository, TKey, TListDTO, TFormDTO, TFilter>
    : ServiceBase<TEntity, TRepository, TKey, TFilter>
    , IService
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
}