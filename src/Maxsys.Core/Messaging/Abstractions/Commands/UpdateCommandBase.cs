using AutoMapper;
using Maxsys.Core.Audit;
using Maxsys.Core.Entities;
using Maxsys.Core.EventSourcing;
using Maxsys.Core.Extensions;
using Maxsys.Core.Interfaces.Data;
using Maxsys.Core.Interfaces.Repositories;

namespace Maxsys.Core.Messaging.Abstractions.Commands;

public abstract class UpdateCommandBase<TKey> : OperationCommand, IKey<TKey>
{
    protected UpdateCommandBase(TKey id) => Id = id;

    public TKey Id { get; }
}

public abstract class UpdateCommandHandlerBase<TEntity, TKey, TCommand, TUpdatedEvent>
    : ICommandHandler<TCommand, OperationResult>
    where TCommand : UpdateCommandBase<TKey>
    where TEntity : Entity<TKey>
    where TUpdatedEvent : AuditableDomainEvent<TKey>
{
    protected readonly IMapper _mapper;
    protected readonly IRepository<TEntity> _repository;
    protected readonly IUnitOfWork _uow;

    public UpdateCommandHandlerBase(IMapper mapper, IRepository<TEntity> repository, IUnitOfWork uow)
    {
        _mapper = mapper;
        _repository = repository;
        _uow = uow;
    }

    public virtual async Task<OperationResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        // Obtém a entity
        var entity = await _repository.GetByIdAsync(request.Id!, @readonly: false, cancellationToken);
        if (entity is null)
        {
            return new(GenericMessages.ITEM_NOT_FOUND);
        }

        // Map + Audit
        var before = entity.ToJson();
        _mapper.Map(request, entity!);
        var after = entity.ToJson();
        var audit = AuditHelper.GetAuditLog(before, after);

        await OnUpdatingAsync(entity, cancellationToken);

        // atualizando
        if (await _repository.UpdateAsync(entity, cancellationToken))
        {
            await OnUpdatedAsync(entity, request, audit, cancellationToken);
        }

        // SaveChanges
        var result = await _uow.SaveChangesAsync(cancellationToken);

        return result;
    }

    protected virtual Task OnUpdatingAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Adds <typeparamref name="TUpdatedEvent"/> event with auditLog.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="command"></param>
    /// <param name="auditLog"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual Task OnUpdatedAsync(TEntity entity, TCommand command, AuditLog auditLog, CancellationToken cancellationToken)
    {
        var domainEvent = _mapper.Map<TEntity, TUpdatedEvent>(entity, opt => opt.AfterMap((src, dst) =>
        {
            dst.SetAudit(auditLog);
        }));
        entity.AddDomainEvent(domainEvent);

        return Task.CompletedTask;
    }
}