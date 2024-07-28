using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Maxsys.Core.Attributes;
using Maxsys.Core.Filtering;

namespace Maxsys.Core.Helpers;

/// <summary>
/// Provides help methods to handle and create Expressions.
/// </summary>
public static class ExpressionHelper
{
    /// <summary>
    /// Transforma uma string em uma expression.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <remarks>
    /// Para um <paramref name="propertyName"/> igual a "<c>Address.State.Name</c>", por exemplo,
    /// será obtida uma expression "<c>x => convert(x.Address.State.Name, Object)</c>" onde <c>x</c> é um <typeparamref name="T"/>.
    /// </remarks>
    public static Expression<Func<T, dynamic>> GetMemberAccessExpression<T>(string propertyName) where T : class
    {
        // Obtém as properties aninhadas. Ex: ["Address", "State", "Name"]
        // Em seguida obtém-se os PropertyInfos aninhados.
        var chainedProperties = propertyName.Split('.', StringSplitOptions.RemoveEmptyEntries).ToList();
        var propertyInfos = GetPropertyInfos(typeof(T), chainedProperties);

        var parameterExpression = Expression.Parameter(typeof(T), "x");

        var memberExpression = GetPropertyExpression(parameterExpression, propertyInfos[0]);
        if (propertyInfos.Count > 1)
        {
            foreach (var item in propertyInfos.Skip(1))
            {
                memberExpression = GetPropertyExpression(memberExpression, item);
            }
        }

        // Necessário! Por quê? Não sei.
        var conversion = Expression.Convert(memberExpression, typeof(object));

        // {x => convert(x.Address.State.Name, Object)}
        var expression = Expression.Lambda<Func<T, dynamic>>(conversion, parameterExpression);

        return expression;
    }

    public static Expression<Func<T, bool>> SearchTermToExpression<T>(SearchTerm search, Expression<Func<T, string?[]>> entityFieldArray)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(search?.Term, "search.Term");

        if (entityFieldArray.Body is not NewArrayExpression arrayExpression)
        {
            throw new InvalidOperationException("Invalid expression.");
        }

        var methodName = search.Mode switch
        {
            SearchTermModes.Any => nameof(string.Contains),
            SearchTermModes.StartsWith => nameof(string.StartsWith),
            SearchTermModes.EndsWith => nameof(string.EndsWith),
            _ => throw new InvalidEnumArgumentException(nameof(search.Mode), (int)search.Mode, typeof(SearchTermModes)),
        };

        // methodInfo: '.Contains()' / '.StartsWith()' / '.EndsWith()'
        var methodInfo = typeof(string).GetMethod(methodName, [typeof(string)]);
        var filterConstant = Expression.Constant(search.Term!);

        Expression? memberFilteredExpression = null;

        foreach (var item in arrayExpression.Expressions)
        {
            // item: 'x.Cuca'
            if (item is MemberExpression memberExpression)
            {
                // convertedMemberExpression: 'x.Cuca' ou 'Convert.ToString(x.CucaXml)'
                var convertedMemberExpression = MustConvertToString(memberExpression.Member)
                    ? Expression.Call(typeof(Convert).GetMethod(nameof(Convert.ToString), [typeof(string)])!, item)
                    : item;

                // filteredMemberValue: 'x.Cuca.Contains("beludo")' 'Convert.ToString(x.CucaXml).Contains("<beludo>")'
                var filteredMemberValue = Expression.Call(convertedMemberExpression, methodInfo!, filterConstant);

                memberFilteredExpression = memberFilteredExpression is null
                    ? filteredMemberValue
                    : Expression.Or(memberFilteredExpression, filteredMemberValue);
            }
        }

        return Expression.Lambda<Func<T, bool>>(memberFilteredExpression!, entityFieldArray.Parameters);
    }

    #region Private

    private static List<PropertyInfo> GetPropertyInfos(Type type, List<string> chainedProperties)
    {
        if (chainedProperties.Count == 0)
            throw new ArgumentException($"Argument {nameof(chainedProperties)} cannot be empty.", nameof(chainedProperties));

        var propertyInfo = type.GetProperty(chainedProperties[0])
            ?? throw new InvalidOperationException($"Property '{chainedProperties[0]}' not found.");

        List<PropertyInfo> propertyInfos = [propertyInfo];
        if (chainedProperties.Count > 1)
        {
            propertyInfos.AddRange(GetPropertyInfos(propertyInfo.PropertyType, chainedProperties.Skip(1).ToList()));
        }

        return propertyInfos;
    }

    /// <summary>
    /// Caso [Text] esteja presente na property, indica que o
    /// tipo da coluna no banco referente a essa property é do tipo TEXT.
    /// <para/>
    /// Nesse caso <c>{ x => x.TextProp }</c> não deve ser retornado,
    /// mas sim <c>{ x => ToString(x.TextProp) }</c> para que a ordenação por
    /// uma coluna TEXT não cause uma exception.
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="propertyInfo"></param>
    /// <returns></returns>
    private static Expression GetPropertyExpression(Expression expression, PropertyInfo propertyInfo)
    {
        var memberExpression = Expression.Property(expression, propertyInfo);

        Expression result = MustConvertToString(propertyInfo)
            ? Expression.Call(typeof(Convert).GetMethod(nameof(Convert.ToString), [typeof(string)])!, memberExpression)
            : memberExpression;

        return result;
    }

    private static bool MustConvertToString(MemberInfo memberInfo)
    {
        return memberInfo.GetCustomAttribute<TextColumnAttribute>() is not null
            || memberInfo.GetCustomAttribute<XmlColumnAttribute>() is not null;
    }

    #endregion Private
}