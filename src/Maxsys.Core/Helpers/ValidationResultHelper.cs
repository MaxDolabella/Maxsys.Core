using FluentValidation.Results;

namespace Maxsys.Core.Helpers;

public static class ValidationResultHelper
{
    /// <summary>
    /// Cria um <see cref="ValidationResult"/> a partir de uma exception+message.
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public static ValidationResult Create(Exception exception, string errorMessage)
    {
        return new ValidationResult().AddException(exception, errorMessage);
    }

    /// <summary>
    /// Cria um <see cref="ValidationResult"/> a partir de uma exception+message.
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public static ValidationResult Create(string errorMessage)
    {
        return new ValidationResult().AddError(errorMessage);
    }
}