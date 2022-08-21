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
    /// <param name="errorCode">failure error code.</param>
    public static ValidationResult AddError(this ValidationResult validationResult, string errorMessage, string errorCode)
    {
        validationResult.Errors.Add(new ValidationFailure
        {
            ErrorMessage = errorMessage,
            ErrorCode = errorCode
        });

        return validationResult;
    }

    /// <summary>
    /// Adds a <see cref="ValidationFailure"/> from an <see cref="Exception"/> to current <see cref="ValidationResult"/>.
    /// </summary>
    /// <param name="validationResult">current <see cref="ValidationResult"/>.</param>
    /// <param name="errorCode">failure error code.</param>
    public static ValidationResult AddErrorCode(this ValidationResult validationResult, string errorCode)
        => validationResult.AddError(string.Empty, errorCode);

    /// <summary>
    /// Adds a <see cref="ValidationFailure"/> from an <see cref="Exception"/> to current <see cref="ValidationResult"/>.
    /// </summary>
    /// <param name="validationResult">current <see cref="ValidationResult"/>.</param>
    /// <param name="errorMessage">failure error message.</param>
    public static ValidationResult AddErrorMessage(this ValidationResult validationResult, string errorMessage)
        => validationResult.AddError(errorMessage, string.Empty);

    /// <summary>
    /// Adds a <see cref="ValidationFailure"/> from an <see cref="Exception"/> to current <see cref="ValidationResult"/> where
    /// <see cref="ValidationFailure.ErrorMessage"/> will be setted as the <paramref name="exception"/>.Message
    /// and <see cref="ValidationFailure.ErrorCode"/> will be setted the <paramref name="exception"/> type.
    /// </summary>
    /// <param name="validationResult">current ValidationResult</param>
    /// <param name="exception">exception to add as failure</param>
    public static ValidationResult AddException(this ValidationResult validationResult, Exception exception)
    {
        validationResult.AddError(exception.Message, $"Exception.{exception.GetType().Name}");

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
        validationResult.AddError(errorMessage, $"EXCEPTION.{exception.GetType().Name}");

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
}