using AutoMapper;
using FluentValidation;
using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Data;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Services;

public abstract class ServiceBase<TEntity, TKey, TRepository, TReadDTO, TCreateDTO, TUpdateDTO, TFilter>
    : ServiceBase<TEntity, TKey, TRepository, TReadDTO, TReadDTO, TCreateDTO, TUpdateDTO, TFilter>
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
        IMapper mapper,
        ISortColumnSelector<InfoDTO<TKey>> infoSortColumnsSelector,
        ISortColumnSelector<TReadDTO> listSortColumnsSelector,
        IValidator<TEntity> validator)
        : base(uow, repository, mapper, infoSortColumnsSelector, listSortColumnsSelector, validator)
    { }
}