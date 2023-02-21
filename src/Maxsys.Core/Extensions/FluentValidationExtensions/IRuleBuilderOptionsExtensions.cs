namespace FluentValidation;

/// <summary>
/// Extension Methods for <see cref="IRuleBuilder{T, TProperty}"/>
/// </summary>
public static class IRuleBuilderOptionsExtensions
{
    /// <summary>
    /// Método de extensão Headsoft.<br/>
    /// Atalho para .WithMessage(<paramref name="errorMessage"/>).WithSeverity(<paramref name="severity"/>);
    /// </summary>
    /// <param name="rule">a regra atual.</param>
    /// <param name="errorMessage">a mensagem de erro para aplicar.</param>
    /// <param name="severity">a severidade do erro.</param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string errorMessage, Severity severity = Severity.Error)
    {
        return rule.WithMessage(errorMessage).WithSeverity(severity);
    }
}