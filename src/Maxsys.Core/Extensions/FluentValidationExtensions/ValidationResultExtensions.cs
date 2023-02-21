using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentValidation.Results;

/// <summary>
/// Contains extension methods for <see cref="ValidationResult"/>
/// </summary>
public static class ValidationResultExtensions
{
    /// <summary>
    /// Adds a <see cref="ValidationFailure"/> from an <see cref="Exception"/> to current <see cref="ValidationResult"/>.
    /// </summary>
    /// <param name="validationResult">current <see cref="ValidationResult"/>.</param>
    /// <param name="errorMessage">failure error message.</param>
    /// <param name="severity">failure severity.</param>
    public static ValidationResult AddError(this ValidationResult validationResult, string errorMessage, Severity severity = Severity.Error)
        => validationResult.AddError(errorMessage, string.Empty, severity);

    /// <summary>
    /// Adds a <see cref="ValidationFailure"/> from an <see cref="Exception"/> to current <see cref="ValidationResult"/>.
    /// </summary>
    /// <param name="validationResult">current <see cref="ValidationResult"/>.</param>
    /// <param name="errorMessage">failure error message.</param>
    /// <param name="errorCode">failure error code.</param>
    /// <param name="severity">failure severity.</param>
    public static ValidationResult AddError(this ValidationResult validationResult, string errorMessage, string errorCode, Severity severity = Severity.Error)
    {
        validationResult.Errors.Add(new ValidationFailure
        {
            ErrorMessage = errorMessage,
            ErrorCode = errorCode,
            Severity = severity
        });

        return validationResult;
    }

    /// <summary>
    /// Adds a <see cref="ValidationFailure"/> from an <see cref="Exception"/> to current <see cref="ValidationResult"/>.
    /// </summary>
    /// <param name="validationResult">current <see cref="ValidationResult"/>.</param>
    /// <param name="errorCode">failure error code.</param>
    /// <param name="severity">failure severity.</param>
    public static ValidationResult AddErrorCode(this ValidationResult validationResult, string errorCode, Severity severity = Severity.Error)
        => validationResult.AddError(string.Empty, errorCode, severity);

    /// <summary>
    /// Adds a <see cref="ValidationFailure"/> from an <see cref="Exception"/> to current <see cref="ValidationResult"/> where
    /// <see cref="ValidationFailure.ErrorMessage"/> will be setted as the <paramref name="exception"/>.Message
    /// and <see cref="ValidationFailure.ErrorCode"/> will be setted the <paramref name="exception"/> type.
    /// </summary>
    /// <param name="validationResult">current ValidationResult</param>
    /// <param name="exception">exception to add as failure</param>
    public static ValidationResult AddException(this ValidationResult validationResult, Exception exception)
    {
        validationResult.AddException(exception, exception.Message);

        return validationResult;
    }

    /// <summary>
    /// Adds a <see cref="ValidationFailure"/> from an <see cref="Exception"/> to current <see cref="ValidationResult"/>
    /// with a specified <paramref name="errorMessage"/>
    /// where <see cref="ValidationFailure.ErrorCode"/> will be setted the <paramref name="exception"/> type.
    /// <br/>
    /// Exception Message will be ignored.
    /// </summary>
    /// <param name="validationResult">current ValidationResult</param>
    /// <param name="errorMessage">the failure error message. Exception message will be ignored.</param>
    /// <param name="exception">exception to add as failure</param>
    public static ValidationResult AddException(this ValidationResult validationResult, Exception exception, string errorMessage)
    {
        validationResult.Errors.Add(new ValidationFailure
        {
            ErrorMessage = errorMessage,
            ErrorCode = $"[{exception.GetType()}]: {exception}",
        });

        return validationResult;
    }

    /// <summary>
    /// Gets Error Messages as a string enumerable.
    /// </summary>
    /// <param name="validationResult">current ValidationResult</param>
    public static IEnumerable<string> GetErrorMessages(this ValidationResult validationResult)
        => validationResult.Errors.Select(err => err.ErrorMessage);

    /// <summary>
    /// Gets Error Codes as a string enumerable.
    /// </summary>
    /// <param name="validationResult">current ValidationResult</param>
    public static IEnumerable<string> GetErrorCodes(this ValidationResult validationResult)
        => validationResult.Errors.Select(err => err.ErrorCode);

    /// <summary>
    /// Determines whether an error list contains an specified error message.
    /// </summary>
    public static bool ContainsErrorMessage(this ValidationResult validationResult, string errorMessage)
    {
        return validationResult.Errors.Any(e => e.ErrorMessage == errorMessage);
    }
}