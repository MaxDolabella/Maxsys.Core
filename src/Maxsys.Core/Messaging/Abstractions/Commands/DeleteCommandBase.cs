using AutoMapper;
using Maxsys.Core.Entities;
using Maxsys.Core.EventSourcing;
using Maxsys.Core.Interfaces.Data;
using Maxsys.Core.Interfaces.Repositories;

namespace Maxsys.Core.Messaging.Abstractions.Commands;

public abstract class DeleteCommandBase<TKey> : OperationCommand, IKey<TKey>
{
    protected DeleteCommandBase(TKey id) => Id = id;

    public TKey Id { get; }
}

public abstract class DeleteCommandHandlerBase<TEntity, TKey, TCommand, TDeletedEvent>
    : ICommandHandler<TCommand, OperationResult>
    where TCommand : DeleteCommandBase<TKey>
    where TEntity : Entity<TKey>
    where TDeletedEvent : DomainEvent
{
    protected readonly IMapper _mapper;
    protected readonly IRepository<TEntity> _repository;
    protected readonly IUnitOfWork _uow;

    protected DeleteCommandHandlerBase(IMapper mapper, IRepository<TEntity> repository, IUnitOfWork uow)
    {
        _mapper = mapper;
        _repository = repository;
        _uow = uow;
    }

    public virtual async Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        // Obtém a entity
        var entity = await _repository.GetByIdAsync([request.Id!], @readonly: false, cancellationToken);
        if (entity is null)
        {
            return new(GenericMessages.ITEM_NOT_FOUND);
        }

        await OnDeletingAsync(entity, cancellationToken);

        // deletando
        if (await _repository.DeleteAsync([request.Id!], cancellationToken))
        {
            await OnDeletedAsync(entity, request, cancellationToken);
        }

        // SaveChanges
        var result = await _uow.SaveChangesAsync(cancellationToken);

        return result;
    }

    protected virtual Task OnDeletingAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnDeletedAsync(TEntity entity, TCommand command, CancellationToken cancellationToken)
    {
        var domainEvent = _mapper.Map<TDeletedEvent>(entity);
        entity.AddDomainEvent(domainEvent);

        return Task.CompletedTask;
    }
}