using AutoMapper;
using FluentValidation;
using Maxsys.Core.DTO;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Data;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Interfaces.Services;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Services;

/// <inheritdoc cref="IService"/>
public abstract class ServiceBase : IService
{
    /// <inheritdoc />
    public Guid Id { get; } = Guid.NewGuid();

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

public abstract class ServiceBase<TEntity, TKey, TRepository, TListDTO, TFormDTO, TCreateDTO, TUpdateDTO, TFilter>
    : EntityReadServiceBase<TEntity, TKey, TRepository, TListDTO, TFormDTO, TFilter>
    , IService<TKey, TListDTO, TFormDTO, TCreateDTO, TUpdateDTO, TFilter>
    where TEntity : class
    where TKey : notnull
    where TRepository : IRepository<TEntity, TFilter>
    where TListDTO : class, IDTO
    where TFormDTO : class, IDTO
    where TCreateDTO : class, IDTO
    where TUpdateDTO : class, IDTO, IKey<TKey>
    where TFilter : class, IFilter<TEntity>, new()
{
    protected readonly IUnitOfWork _uow;
    protected readonly IMapper _mapper;
    protected readonly IValidator<TEntity> _validator;

    public ServiceBase(IUnitOfWork uow, TRepository repository, IMapper mapper, ISortColumnSelector<InfoDTO<TKey>> infoSortColumnsSelector, ISortColumnSelector<TListDTO> listSortColumnsSelector, IValidator<TEntity> validator)
        : base(repository, infoSortColumnsSelector, listSortColumnsSelector)
    {
        _uow = uow;
        _mapper = mapper;
        _validator = validator;
    }

    public virtual async Task<OperationResult<TCreateDTO>> AddAsync(TCreateDTO itemToCreate, CancellationToken cancellation = default)
    {
        // mapeia pra entity
        var entity = _mapper.Map<TEntity>(itemToCreate);

        // valida
        var validationResult = await _validator.ValidateAsync(entity, cancellation);
        if (!validationResult.IsValid)
            return new(itemToCreate, validationResult);

        // insere
        await _repository.AddAsync(entity, cancellation);

        // retorna
        return new(itemToCreate, await _uow.CommitAsync(cancellation));
    }

    public virtual async Task<OperationResultCollection<TCreateDTO>> AddAsync(IEnumerable<TCreateDTO> items, bool stopOnFirstFail = true, CancellationToken cancellation = default)
    {
        var results = new OperationResultCollection<TCreateDTO>();

        if (stopOnFirstFail)
            await _uow.BeginTransactionAsync(null, cancellation);

        foreach (var item in items)
        {
            if (!stopOnFirstFail)
                await _uow.BeginTransactionAsync(null, cancellation);

            try
            {
                var operationResult = await AddAsync(item, cancellation);
                if (!operationResult.IsValid)
                {
                    await _uow.RollbackTransactionAsync(cancellation);
                    results.Add(operationResult);

                    if (stopOnFirstFail)
                        return results;
                    else
                        continue;
                }

                if (!stopOnFirstFail)
                    await _uow.CommitTransactionAsync(cancellation);

                results.Add(new(item));
            }
            catch (Exception exception)
            {
                await _uow.RollbackTransactionAsync(cancellation);
                results.Add(new(item, exception));

                if (stopOnFirstFail)
                    return results;
                else
                    continue;
            }
        }

        if (stopOnFirstFail)
            await _uow.CommitTransactionAsync(cancellation);

        return results;
    }

    public virtual async Task<OperationResult<TUpdateDTO>> UpdateAsync(TUpdateDTO itemToUpdate, CancellationToken cancellation = default)
    {
        // Obtém a entity
        var entity = await _repository.GetAsync(IdSelector(itemToUpdate.Id), @readonly: false, cancellation);
        if (entity is null)
            return new(itemToUpdate, CommonMessages.ITEM_NOT_FOUND);

        // mapeia pra entity
        _mapper.Map(itemToUpdate, entity!);

        // valida
        var validationResult = await _validator.ValidateAsync(entity, cancellation);
        if (!validationResult.IsValid)
            return new(itemToUpdate, validationResult);

        // atualiza
        await _repository.UpdateAsync(entity, cancellation);

        // retorna
        return new(itemToUpdate, await _uow.CommitAsync(cancellation));
    }

    public virtual async Task<OperationResultCollection<TUpdateDTO>> UpdateAsync(IEnumerable<TUpdateDTO> itemsToUpdate, bool stopOnFirstFail = true, CancellationToken cancellation = default)
    {
        var results = new OperationResultCollection<TUpdateDTO>();

        if (stopOnFirstFail)
            await _uow.BeginTransactionAsync(null, cancellation);

        foreach (var item in itemsToUpdate)
        {
            if (!stopOnFirstFail)
                await _uow.BeginTransactionAsync(null, cancellation);

            try
            {
                var operationResult = await UpdateAsync(item, cancellation);
                if (!operationResult.IsValid)
                {
                    await _uow.RollbackTransactionAsync(cancellation);
                    results.Add(operationResult);

                    if (stopOnFirstFail)
                        return results;
                    else
                        continue;
                }

                if (!stopOnFirstFail)
                    await _uow.CommitTransactionAsync(cancellation);

                results.Add(new(item));
            }
            catch (Exception exception)
            {
                await _uow.RollbackTransactionAsync(cancellation);
                results.Add(new(item, exception));

                if (stopOnFirstFail)
                    return results;
                else
                    continue;
            }
        }

        if (stopOnFirstFail)
            await _uow.CommitTransactionAsync(cancellation);

        return results;
    }

    public virtual async Task<OperationResult<TKey>> DeleteAsync(TKey id, CancellationToken cancellation = default)
    {
        // verifica se entity existe
        if (!await _repository.AnyAsync(IdSelector(id), cancellation))
            return new(id, CommonMessages.ITEM_NOT_FOUND);

        // atualiza
        await _repository.DeleteAsync(id, cancellation);

        // atualiza / retorna
        return new(id, await _uow.CommitAsync(cancellation));
    }

    public virtual async Task<OperationResultCollection<TKey>> DeleteAsync(IEnumerable<TKey> ids, bool stopOnFirstFail = true, CancellationToken cancellation = default)
    {
        var results = new OperationResultCollection<TKey>();

        if (stopOnFirstFail)
            await _uow.BeginTransactionAsync(null, cancellation);

        foreach (var id in ids)
        {
            if (!stopOnFirstFail)
                await _uow.BeginTransactionAsync(null, cancellation);

            try
            {
                var operationResult = await DeleteAsync(id, cancellation);
                if (!operationResult.IsValid)
                {
                    await _uow.RollbackTransactionAsync(cancellation);
                    results.Add(operationResult);

                    if (stopOnFirstFail)
                        return results;
                    else
                        continue;
                }

                if (!stopOnFirstFail)
                    await _uow.CommitTransactionAsync(cancellation);

                results.Add(new(id));
            }
            catch (Exception exception)
            {
                await _uow.RollbackTransactionAsync(cancellation);
                results.Add(new(id, exception));

                if (stopOnFirstFail)
                    return results;
                else
                    continue;
            }
        }

        if (stopOnFirstFail)
            await _uow.CommitTransactionAsync(cancellation);

        return results;
    }
}