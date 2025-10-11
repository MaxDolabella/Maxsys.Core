using AutoMapper;
using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Data;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;

namespace Maxsys.Core.Services;

[Obsolete("Use ServiceBase<TEntity, TRepository, TKey, TFilter> instead.", true)]
public abstract class ServiceBase<TEntity, TRepository, TKey, TListDTO, TFormDTO, TCreateDTO, TUpdateDTO, TFilter>
    : ServiceBase<TEntity, TRepository, TKey, TListDTO, TFormDTO, TFilter>
    , IService
    where TEntity : class
    where TKey : notnull
    where TRepository : IRepository<TEntity, TFilter>
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TCreateDTO : class, IDTO
    where TUpdateDTO : class, IDTO, IKey<TKey>
    where TFilter : class, IFilter<TEntity>, new()
{
    protected ServiceBase(
        IUnitOfWork uow,
        TRepository repository,
        IMapper mapper)
        : base(repository)
    {
    }
}