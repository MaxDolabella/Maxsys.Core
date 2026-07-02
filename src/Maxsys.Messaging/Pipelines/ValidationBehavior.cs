using FluentValidation;
using FluentValidation.Results;
using Maxsys.Core;
using Maxsys.Core.Extensions;
using Maxsys.Messaging.Abstractions;

namespace Maxsys.Messaging.Pipelines;

/// <summary>
/// Behavior de validação automática via FluentValidation.
/// Aplica-se apenas a commands com retorno (ICommand&lt;TResponse&gt;).
/// Se TResponse herda de OperationResult: erros viram Notifications (sem exception).
/// Caso contrário: lança ValidationException.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, ICommand<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public async Task<TResponse> HandleAsync(TRequest request, Func<Task<TResponse>> next, CancellationToken ct)
    {
        var context = new ValidationContext<TRequest>(request);

        var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context)));

        var errors = results
            .Where(r => !r.IsValid)
            .SelectMany(r => r.Errors)
            .Distinct()
            .ToList();

        if (errors.Count == 0)
            return await next();

        if (typeof(TResponse).IsAssignableTo(typeof(OperationResult)))
        {
            var response = Activator.CreateInstance<TResponse>();
            var operationResult = (response as OperationResult)!;

            operationResult.AddNotifications(errors.Select(e => e.ConvertToNotification()));

            return response;
        }

        throw new ValidationException(errors);
    }
}