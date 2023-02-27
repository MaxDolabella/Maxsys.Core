using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Maxsys.DataCore.Interfaces;
using Maxsys.ModelCore.Filtering;
using Maxsys.ModelCore.Listing;
using Maxsys.ModelCore.Sorting;

namespace Maxsys.ModelCore.Services;

/// <inheritdoc cref="IService"/>
public abstract class ServiceBase : IService
{
    /// <inheritdoc/>
    public Guid Id { get; } = Guid.NewGuid();

    /// <inheritdoc/>
    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

public abstract class ReadServiceBase<TEntity, TRepository, TListDTO, TFormDTO, TInfoDTO, TFilter>
    : ServiceBase, IReadService<TListDTO, TFormDTO, TInfoDTO, TFilter>
    where TEntity : class
    where TRepository : IRepository<TEntity, TFilter, object>
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TInfoDTO : class, IDTO
    where TFilter : IFilter<TEntity>
{
    protected readonly TRepository _repository;
    protected readonly IMapper _mapper;
    protected readonly ISortColumnSelector<TInfoDTO> _infoSortColumnsSelector;
    protected readonly ISortColumnSelector<TListDTO> _listSortColumnsSelector;

    public ReadServiceBase(TRepository repository, IMapper mapper, ISortColumnSelector<TInfoDTO> infoSortColumnsSelector, ISortColumnSelector<TListDTO> listSortColumnsSelector) : base()
    {
        _repository = repository;
        _mapper = mapper;
        _infoSortColumnsSelector = infoSortColumnsSelector;
        _listSortColumnsSelector = listSortColumnsSelector;
    }

    protected abstract Expression<Func<TEntity, bool>> GetIdSelector(object id);

    public virtual async Task<ListDTO<TInfoDTO>> GetInfoAsync(TFilter filters, Criteria criteria, CancellationToken cancellation = default)
    {
        return new()
        {
            Count = await _repository.CountAsync(filters, cancellation),
            List = await _repository.GetAsync<TInfoDTO>(filters, criteria, _infoSortColumnsSelector, cancellation)
        };
    }

    public virtual async Task<ListDTO<TListDTO>> GetListAsync(TFilter filters, Criteria criteria, CancellationToken cancellation = default)
    {
        return new()
        {
            Count = await _repository.CountAsync(filters, cancellation),
            List = await _repository.GetAsync<TListDTO>(filters, criteria, _listSortColumnsSelector, cancellation)
        };
    }

    public virtual async Task<TFormDTO?> GetAsync(object id, CancellationToken cancellation = default)
    {
        return await _repository.GetFirstAsync<TFormDTO>(GetIdSelector(id), cancellation);
    }
}

public abstract class ServiceBase<TEntity, TRepository, TListDTO, TFormDTO, TInfoDTO, TCreateDTO, TUpdateDTO, TFilter>
    : ReadServiceBase<TEntity, TRepository, TListDTO, TFormDTO, TInfoDTO, TFilter>, IService<TListDTO, TFormDTO, TInfoDTO, TCreateDTO, TUpdateDTO, TFilter>
    where TEntity : class
    where TRepository : IRepository<TEntity, TFilter, object>
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TInfoDTO : class, IDTO
    where TCreateDTO : class, IDTO
    where TUpdateDTO : class, IDTO
    where TFilter : IFilter<TEntity>
{
    protected readonly IUnitOfWork _uow;
    protected readonly IValidator<TEntity> _validator;

    public ServiceBase(IUnitOfWork uow, TRepository repository, IMapper mapper, ISortColumnSelector<TInfoDTO> infoSortColumnsSelector, ISortColumnSelector<TListDTO> listSortColumnsSelector, IValidator<TEntity> validator)
        : base(repository, mapper, infoSortColumnsSelector, listSortColumnsSelector)
    {
        _uow = uow;
        _validator = validator;
    }

    public virtual async Task<ValidationResult> AddAsync(TCreateDTO itemToCreate, CancellationToken cancellation = default)
    {
        // mapeia pra entity
        var entity = _mapper.Map<TEntity>(itemToCreate);

        // valida
        var validationResult = await _validator.ValidateAsync(entity, cancellation);
        if (!validationResult.IsValid)
            return validationResult;

        // insere
        await _repository.AddAsync(entity, cancellation);

        // retorna
        return await _uow.CommitAsync(cancellation);
    }

    public virtual async Task<ValidationResult> UpdateAsync(object id, TUpdateDTO itemToUpdate, CancellationToken cancellation = default)
    {
        // Obtém a entity
        var entity = await _repository.GetFirstAsync(GetIdSelector(id), @readonly: false, cancellation);
        if (entity is null)
            return new ValidationResult().AddError("Item Not Found");

        // mapeia pra entity
        _mapper.Map(itemToUpdate, entity!);

        // valida
        var validationResult = await _validator.ValidateAsync(entity, cancellation);
        if (!validationResult.IsValid)
            return validationResult;

        // atualiza
        await _repository.UpdateAsync(entity, cancellation);

        // retorna
        return await _uow.CommitAsync(cancellation);
    }

    public virtual async Task<ValidationResult> DeleteAsync(object id, CancellationToken cancellation = default)
    {
        // verifica se entity existe
        if (!await _repository.AnyAsync(GetIdSelector(id), cancellation))
            return new ValidationResult().AddError("Item Not Found");

        // atualiza
        await _repository.DeleteAsync(id, cancellation);

        // atualiza / retorna
        return await _uow.CommitAsync(cancellation);
    }
}