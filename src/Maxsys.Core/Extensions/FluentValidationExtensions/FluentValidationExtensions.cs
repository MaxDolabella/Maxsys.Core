using System;
using System.Collections.Generic;
using System.Linq;

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
    /// <returns></returns>
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string errorMessage, Severity severity = Severity.Error)
    {
        return rule.WithMessage(errorMessage).WithErrorCode(" ").WithSeverity(severity);
    }



    /// <summary>
    /// Defines a 'list must contain fewer or equal than' validator on the current rule builder.
    /// Validation will fail if the collection is null or itens count is more than <paramref name="maxItensCount">max itens count</paramref>
    /// </summary>
    /// <typeparam name="T">Type of object being validated</typeparam>
    /// <typeparam name="TElement">Type of collection being validated</typeparam>
    /// <param name="maxItensCount">Type of collection being validated</param>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, IEnumerable<TElement>> ListMustContainFewerOrEqualsThan<T, TElement>(this IRuleBuilder<T, IEnumerable<TElement>> ruleBuilder, int maxItensCount)
    {
        return ruleBuilder.Must(collection => collection?.Count() <= maxItensCount).WithMessage("The list contains too many items");
    }

    /// <summary>
    /// Defines a 'not empty' validator on the current rule builder.
    /// Validation will fail if the collection is null or empty.
    /// </summary>
    /// <typeparam name="T">Type of object being validated</typeparam>
    /// <typeparam name="TElement">Type of collection being validated</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, IEnumerable<TElement>> NotEmptyList<T, TElement>(this IRuleBuilder<T, IEnumerable<TElement>> ruleBuilder)
    {
        return ruleBuilder.Must(collection => collection != null && collection.Any())
            .WithMessage("The collection must not be null or empty.");
    }

    /// <summary>
    /// Defines a 'not null' validator on the current rule builder.
    /// Validation will fail if the collection is null.
    /// </summary>
    /// <typeparam name="T">Type of object being validated</typeparam>
    /// <typeparam name="TElement">Type of collection being validated</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, IEnumerable<TElement>> NotNullList<T, TElement>(this IRuleBuilder<T, IEnumerable<TElement>> ruleBuilder)
    {
        return ruleBuilder.Must(collection => collection != null)
            .WithMessage("The collection must not be null.");
    }

    /// <summary>
    /// Defines a 'only distinct itens' validator on the current rule builder.
    /// Validation will fail if the collection is null, empty or itens not be unique.
    /// </summary>
    /// <typeparam name="T">Type of object being validated</typeparam>
    /// <typeparam name="TElement">Type of collection being validated</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, IEnumerable<TElement>> OnlyUniqueItens<T, TElement>(this IRuleBuilder<T, IEnumerable<TElement>> ruleBuilder)
    {
        return ruleBuilder.Must(collection
            => collection != null
            && collection?.Count() == collection?.Distinct()?.Count())
            .WithMessage("The collection must be only unique itens.");
    }

    /// <summary>
    /// Defines a 'only distinct itens' validator on the current rule builder.
    /// Validation will fail if the collection is null, empty or itens not be unique.
    /// </summary>
    /// <typeparam name="T">Type of object being validated</typeparam>
    /// <typeparam name="TElement">Type of collection being validated</typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
    /// <param name="selector">The property that will be compared</param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, IEnumerable<TElement>> OnlyUniqueItens<T, TElement, TKey>(this IRuleBuilder<T, IEnumerable<TElement>> ruleBuilder, Func<TElement, TKey> selector)
    {
        return ruleBuilder.Must(collection
            => collection != null
            && collection?.Count() == collection?.Select(selector).Distinct()?.Count())
        .WithMessage("The collection must be only unique itens.");
    }

}