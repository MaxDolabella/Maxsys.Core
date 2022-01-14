using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentValidation.Results
{
    /// <summary>
    /// Contains extension methods for <see cref="ValidationResult"/>
    /// </summary>
    public static class ValidationResultExtensions
    {
        /// <summary>
        /// Adds failures to current <see cref="ValidationResult"/>.
        /// </summary>
        /// <param name="validationResult">current ValidationResult</param>
        /// <param name="propertyName">name of property that failed</param>
        /// <param name="failureMessages">collection of failure messages</param>
        public static void AddFailures(this ValidationResult validationResult, string propertyName
            , IEnumerable<string> failureMessages)
        {
            foreach (var message in failureMessages)
                validationResult.AddFailure(propertyName, message);
        }

        /// <summary>
        /// Adds a failure to current <see cref="ValidationResult"/>.
        /// </summary>
        /// <param name="validationResult">current ValidationResult</param>
        /// <param name="propertyName">name of property that failed</param>
        /// <param name="failureMessage">failure messages</param>
        public static void AddFailure(this ValidationResult validationResult
            , string propertyName, string failureMessage)
            => validationResult.Errors.Add(new ValidationFailure(propertyName, failureMessage));

        /// <summary>
        /// Adds a failure to current <see cref="ValidationResult"/>.<para/>
        /// <see cref="ValidationFailure.PropertyName"/> will be an empty string.
        /// </summary>
        /// <param name="validationResult">current ValidationResult</param>
        /// <param name="failureMessage">failure messages</param>
        public static void AddFailure(this ValidationResult validationResult, string failureMessage)
            => validationResult.AddFailure(string.Empty, failureMessage);

        /// <summary>
        /// Adds a failure to current <see cref="ValidationResult"/>.<para/>
        /// <see cref="ValidationFailure.PropertyName"/> will be an empty string.
        /// </summary>
        /// <param name="validationResult">current ValidationResult</param>
        /// <param name="ex"><see cref="Exception"/> that contains a failure message.</param>
        public static void AddFailure(this ValidationResult validationResult, Exception ex)
            => validationResult.AddFailure(string.Empty, ex.Message);

        /// <summary>
        /// Adds a failure to current <see cref="ValidationResult"/>.<para/>
        /// <see cref="ValidationFailure.PropertyName"/> will be an empty string.
        /// </summary>
        /// <param name="validationResult">current ValidationResult</param>
        /// <param name="propertyName">name of property that failed</param>
        /// <param name="ex"><see cref="Exception"/> that contains a failure message.</param>
        public static void AddFailure(this ValidationResult validationResult, string propertyName, Exception ex)
            => validationResult.AddFailure(propertyName, ex.Message);

        /// <summary>
        /// Gets Error Messages as a string enumerable.
        /// </summary>
        /// <param name="validationResult">current ValidationResult</param>
        public static IEnumerable<string> ErrorMessagesAsEnumerable(this ValidationResult validationResult)
            => validationResult.Errors.Select(err => err.ErrorMessage);

        /// <summary>
        /// Creates and returns a new <see cref="ValidationResult"/> from an <see cref="Exception"/>.
        /// </summary>
        /// <param name="exception">exception to add as failure</param>
        public static ValidationResult ValidationResultFromException(Exception exception)
        {
            var validationResult = new ValidationResult();

            validationResult.AddFailure(exception);

            return validationResult;
        }
    }
}