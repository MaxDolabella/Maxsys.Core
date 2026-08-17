namespace FluentValidation.Results;

/// <summary>
/// Extensões de <see cref="ValidationResult"/> usadas internamente por <see cref="Maxsys.Drawing.ImageHelper"/>.
/// </summary>
internal static class ValidationResultExtensions
{
    /// <summary>
    /// Adiciona uma falha ao <see cref="ValidationResult"/> a partir de uma exceção.
    /// </summary>
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
}
