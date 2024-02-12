using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IService{TKey, TReadDTO, TFilter}"/>
public abstract class ServiceBase<TEntity, TRepository, TKey, TReadDTO, TFilter>
    : ServiceBase<TEntity, TRepository, TKey, TReadDTO, TReadDTO, TFilter>
    , IService<TKey, TReadDTO, TFilter>
    where TKey : notnull
    where TEntity : class
    where TRepository : IRepository<TEntity, TFilter>
    where TReadDTO : class, IDTO
    where TFilter : IFilter<TEntity>, new()
{
    protected ServiceBase(TRepository repository)
       : base(repository)
    { }
}