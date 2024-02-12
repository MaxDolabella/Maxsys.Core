namespace FluentValidation;

public static class FluentValidationExtensions
{
    /// <summary>
    /// Método de extensão Maxsys.<br/>
    /// Atalho para .WithMessage(<paramref name="errorMessage"/>).WithSeverity(<paramref name="severity"/>);
    /// </summary>
    /// <param name="rule">a regra atual.</param>
    /// <param name="errorMessage">a mensagem de erro para aplicar.</param>
    /// <param name="severity">a severidade do erro.</param>
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string errorMessage, Severity severity = Severity.Error)
    {
        return rule.WithMessage(errorMessage).WithErrorCode(" ").WithSeverity(severity);
    }

    /// <summary>
    /// Método de extensão Maxsys.<br/>
    /// Atalho para .WithMessage(<paramref name="errorMessage"/>).WithErrorCode(<paramref name="detailsMessage"/>).WithSeverity(<paramref name="severity"/>);
    /// </summary>
    /// <param name="rule">a regra atual.</param>
    /// <param name="errorMessage">a mensagem de erro para aplicar.</param>
    /// <param name="detailsMessage">a mensagem do detalhe do erro para aplicar.</param>
    /// <param name="severity">a severidade do erro.</param>
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string errorMessage, string detailsMessage, Severity severity = Severity.Error)
    {
        return rule.WithMessage(errorMessage).WithErrorCode(detailsMessage).WithSeverity(severity);
    }

    /// <summary>
    /// Método de extensão Maxsys.<br/>
    /// Atalho para .WithErrorCode(<paramref name="detailsMessage"/>);
    /// </summary>
    /// <param name="rule">a regra atual.</param>
    /// <param name="detailsMessage">a mensagem do detalhe do erro para aplicar.</param>
    public static IRuleBuilderOptions<T, TProperty> WithDetails<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string detailsMessage)
    {
        return rule.WithErrorCode(detailsMessage);
    }
}