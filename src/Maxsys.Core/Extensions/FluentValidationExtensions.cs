using FluentValidation;
using FluentValidation.Results;

namespace Maxsys.Core.Extensions;

/// <summary>
/// Maxsys extension methods for FluentValidation types.
/// </summary>
public static class FluentValidationExtensions
{
    private static Severity ResultTypeToSeverity(ResultTypes resultType)
    {
        return resultType switch
        {
            ResultTypes.Success 
                or ResultTypes.Info => Severity.Info,
            ResultTypes.Warning => Severity.Warning,
            ResultTypes.Error => Severity.Error,
            _ => Severity.Error
        };
    }

    extension<T, TProperty>(IRuleBuilderOptions<T, TProperty> rule)
    {
        /// <summary>
        /// Método de extensão Maxsys.<br/>
        /// Atalho para .WithMessage(<paramref name="message"/>).WithSeverity(ResultTypeToSeverity(<paramref name="resultType"/>));
        /// </summary>
        /// <param name="message">a mensagem para aplicar.</param>
        /// <param name="resultType">o tipo de resultado da notificação.</param>
        public IRuleBuilderOptions<T, TProperty> WithNotification(string message, ResultTypes resultType = ResultTypes.Warning)
        {
            return rule.WithMessage(message).WithErrorCode(" ").WithSeverity(ResultTypeToSeverity(resultType));
        }

        /// <summary>
        /// Método de extensão Maxsys.<br/>
        /// Atalho para .WithMessage(<paramref name="message"/>).WithErrorCode(<paramref name="details"/>).WithSeverity(ResultTypeToSeverity(<paramref name="resultType"/>));
        /// </summary>
        /// <param name="message">a mensagem para aplicar.</param>
        /// <param name="details">a mensagem do detalhe para aplicar.</param>
        /// <param name="resultType">o tipo de resultado da notificação.</param>
        public IRuleBuilderOptions<T, TProperty> WithNotification(string message, string details, ResultTypes resultType = ResultTypes.Warning)
        {
            return rule.WithMessage(message).WithErrorCode(details).WithSeverity(ResultTypeToSeverity(resultType));
        }

        /// <summary>
        /// Método de extensão Maxsys.<br/>
        /// Atalho para .WithMessage(<paramref name="message"/>).WithErrorCode(<paramref name="message"/>).WithSeverity(ResultTypeToSeverity(<paramref name="resultType"/>)).WithState(x => <paramref name="tag"/>)
        /// </summary>
        /// <param name="message">a mensagem para aplicar.</param>
        /// <param name="details">a mensagem do detalhe para aplicar.</param>
        /// <param name="tag">o objeto de estado para aplicar.</param>
        /// <param name="resultType">o tipo de resultado da notificação.</param>
        public IRuleBuilderOptions<T, TProperty> WithNotification(string message, string details, string tag, ResultTypes resultType = ResultTypes.Warning)
        {
            return rule.WithMessage(message).WithErrorCode(details).WithSeverity(ResultTypeToSeverity(resultType)).WithState( x => tag);
        }

        /// <summary>
        /// Método de extensão Maxsys.<br/>
        /// Atalho para .WithMessage(<paramref name="message"/>).WithErrorCode(<paramref name="message"/>).WithSeverity(ResultTypeToSeverity(<paramref name="resultType"/>)).WithState(<paramref name="objectFactory"/>)
        /// </summary>
        /// <param name="message">a mensagem para aplicar.</param>
        /// <param name="details">a mensagem do detalhe para aplicar.</param>
        /// <param name="objectFactory">uma função para criar o objeto de estado.</param>
        /// <param name="resultType">o tipo de resultado da notificação.</param>
        public IRuleBuilderOptions<T, TProperty> WithNotification(string message, string details, Func<T, object> objectFactory, ResultTypes resultType = ResultTypes.Warning)
        {
            return rule.WithMessage(message).WithErrorCode(details).WithSeverity(ResultTypeToSeverity(resultType)).WithState(objectFactory);
        }

    }

    extension(ValidationResult validationResult)
    {
        /// <summary>
        /// Converte os <see cref="ValidationFailure"/> em <see cref="Notification"/>.
        /// </summary>
        public List<Notification> ConvertToNotifications()
        {
            return validationResult.Errors.ConvertAll(e => e.ConvertToNotification());
        }
    }

    extension(ValidationFailure validationFailure)
    {
        /// <summary>
        /// Converte o <see cref="ValidationFailure"/> em <see cref="Notification"/>.
        /// </summary>
        public Notification ConvertToNotification()
        {
            return new Notification(
                validationFailure.ErrorMessage,
                !string.IsNullOrWhiteSpace(validationFailure.ErrorCode) ? validationFailure.ErrorCode : null,
                (ResultTypes)(byte)validationFailure.Severity)
            {
                Tag = validationFailure.CustomState
            };
        }
    }
}