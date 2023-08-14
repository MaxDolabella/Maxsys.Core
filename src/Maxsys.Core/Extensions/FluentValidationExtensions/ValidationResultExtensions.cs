namespace FluentValidation.Results;

/// <summary>
/// Contains extension methods for <see cref="ValidationResult"/>
/// </summary>
public static class ValidationResultExtensions
{
    /// <summary>
    /// Adiciona uma falha ao <see cref="ValidationResult"/> atual.<para/>
    /// </summary>
    /// <param name="validationResult">ValidationResult atual</param>
    /// <param name="errorMessage">mensagem de erro da falha</param>
    public static ValidationResult AddError(this ValidationResult validationResult, string errorMessage, Severity severity = Severity.Error)
    {
        validationResult.Errors.Add(new ValidationFailure
        {
            ErrorMessage = errorMessage,
            Severity = severity
        });

        return validationResult;
    }

    /// <summary>
    /// Adiciona uma falha ao <see cref="ValidationResult"/> atual
    /// onde <c>ErrorMessage = <paramref name="errorMessage"/></c>
    /// e <c>ErrorCode = <paramref name="identifier"/></c>
    /// </summary>
    /// <param name="validationResult"></param>
    /// <param name="errorMessage"></param>
    /// <param name="identifier">é um idenificador para o item do erro. Pode ser por exemplo, um Id, uma propriedade, etc...</param>
    public static ValidationResult AddError(this ValidationResult validationResult, string errorMessage, string identifier, Severity severity = Severity.Error)
    {
        validationResult.Errors.Add(new ValidationFailure
        {
            ErrorMessage = errorMessage,
            ErrorCode = $"Identifier: {identifier}",
            Severity = severity
        });

        return validationResult;
    }

    /// <summary>
    /// Adiciona uma falha ao <see cref="ValidationResult"/> atual.<para/>
    /// </summary>
    /// <param name="validationResult">ValidationResult atual</param>
    public static ValidationResult AddException(this ValidationResult validationResult, Exception exception, Severity severity = Severity.Error)
    {
        return validationResult.AddException(exception, exception.Message, severity);
    }

    /// <summary>
    /// Adiciona uma falha ao <see cref="ValidationResult"/> atual.<para/>
    /// </summary>
    /// <param name="validationResult">ValidationResult atual</param>
    /// <param name="errorMessage">código de erro da falha</param>
    public static ValidationResult AddException(this ValidationResult validationResult, Exception exception, string errorMessage, Severity severity = Severity.Error)
    {
        validationResult.Errors.Add(new ValidationFailure
        {
            ErrorMessage = errorMessage,
            ErrorCode = $"{exception.GetType()}: {exception}",
            Severity = severity
        });

        return validationResult;
    }

    /// <summary>
    /// Verifica se a o resultado da validação possui uma determinada <paramref name="errorMessage"/>.
    /// </summary>
    public static bool ContainsErrorMessage(this ValidationResult validationResult, string errorMessage)
    {
        return validationResult.Errors.Any(e => e.ErrorMessage == errorMessage);
    }

    /// <summary>
    /// Converte os <see cref="ValidationFailure"/> em <see cref="Notification"/>.
    /// </summary>
    /// <param name="validationResult"></param>
    /// <returns></returns>
    public static List<Notification>? ToNotifications(this ValidationResult? validationResult)
    {
        return validationResult?.Errors
            .Select(error => new Notification(
                message: error.ErrorMessage,
                details: !string.IsNullOrWhiteSpace(error.ErrorCode) ? error.ErrorCode : null,
                resultType: (ResultTypes)(byte)error.Severity)
            { Tag = error.CustomState ?? null})
            .ToList() ?? null;
    }
}