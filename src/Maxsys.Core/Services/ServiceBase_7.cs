using AutoMapper;
using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Data;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IService{TKey, TReadDTO, TCreateDTO, TUpdateDTO, TFilter}"/>
public abstract class ServiceBase<TEntity, TRepository, TKey, TReadDTO, TCreateDTO, TUpdateDTO, TFilter>
    : ServiceBase<TEntity, TRepository, TKey, TReadDTO, TReadDTO, TCreateDTO, TUpdateDTO, TFilter>
    , IService<TKey, TReadDTO, TCreateDTO, TUpdateDTO, TFilter>
    where TEntity : class
    where TKey : notnull
    where TRepository : IRepository<TEntity, TFilter>
    where TReadDTO : class, IDTO
    where TCreateDTO : class, IDTO
    where TUpdateDTO : class, IDTO, IKey<TKey>
    where TFilter : class, IFilter<TEntity>, new()
{
    protected ServiceBase(
        IUnitOfWork uow,
        TRepository repository,
        IMapper mapper)
        : base(uow, repository, mapper)
    { }
}