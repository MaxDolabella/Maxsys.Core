using FluentValidation;
using Maxsys.Core.Messaging.Abstractions;
using MediatR;

namespace Maxsys.Core.Messaging.Pipelines;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, ICommand<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var tasks = _validators.Select(validator => validator.ValidateAsync(context)).ToList();
        var validationFailures = await Task.WhenAll(tasks);

        var errors = validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Distinct()
            .ToList();

        if (errors.Count != 0)
        {
            if (typeof(TResponse).IsAssignableTo(typeof(OperationResult)))
            {
                TResponse response = Activator.CreateInstance<TResponse>();

                (response as OperationResult)!.AddNotifications(errors.Select(error => new Notification(error.ErrorMessage, !string.IsNullOrWhiteSpace(error.ErrorCode) ? error.ErrorCode : null, (ResultTypes)error.Severity)));

                return response;
            }

            throw new ValidationException(errors);
        }

        return await next();
    }
}