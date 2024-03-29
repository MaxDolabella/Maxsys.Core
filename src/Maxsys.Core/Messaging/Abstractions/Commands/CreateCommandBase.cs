using AutoMapper;
using Maxsys.Core.Entities;
using Maxsys.Core.EventSourcing;
using Maxsys.Core.Interfaces.Data;
using Maxsys.Core.Interfaces.Repositories;

namespace Maxsys.Core.Messaging.Abstractions.Commands;

public abstract class CreateCommandBase<TKey> : OperationCommand<TKey?>
{ }

/// <remarks>
/// Mapeamentos necessários:
///     <list type="bullet">
///         <item><typeparamref name="TCommand"/> -> <typeparamref name="TEntity"/></item>
///         <item><typeparamref name="TEntity"/> -> <typeparamref name="TCreatedEvent"/></item>
///     </list>
/// </remarks>
public abstract class CreateCommandHandlerBase<TEntity, TKey, TCommand, TCreatedEvent>
    : ICommandHandler<TCommand, OperationResult<TKey?>>
    where TCommand : CreateCommandBase<TKey>
    where TEntity : Entity<TKey>
    where TCreatedEvent : DomainEvent

{
    protected readonly IMapper _mapper;
    protected readonly IRepository<TEntity> _repository;
    protected readonly IUnitOfWork _uow;

    protected CreateCommandHandlerBase(IMapper mapper, IRepository<TEntity> repository, IUnitOfWork uow)
    {
        _mapper = mapper;
        _repository = repository;
        _uow = uow;
    }

    public virtual async Task<OperationResult<TKey?>> Handle(TCommand request, CancellationToken cancellationToken)
    {
        // mapeia pra entity
        var entity = _mapper.Map<TEntity>(request);

        await OnAddingAsync(entity, cancellationToken);

        // inserindo
        if (await _repository.AddAsync(entity, cancellationToken))
        {
            await OnAddedAsync(entity, request, cancellationToken);
        }

        // SaveChanges
        var result = await _uow.SaveChangesAsync(cancellationToken);

        return result.IsValid
            ? result.Cast<TKey?>(entity.Id)
            : result.Cast<TKey?>();
    }

    protected virtual Task OnAddingAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Adds <typeparamref name="TCreatedEvent"/> event.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual Task OnAddedAsync(TEntity entity, TCommand command, CancellationToken cancellationToken)
    {
        var domainEvent = _mapper.Map<TCreatedEvent>(entity)!;
        entity.AddDomainEvent(domainEvent);

        return Task.CompletedTask;
    }
}