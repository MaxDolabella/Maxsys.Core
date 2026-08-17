using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Maxsys.Core.Attributes;

namespace Maxsys.Core.Filtering.Helpers;

/// <summary>
/// Provides help methods to handle and create Expressions for filtering entities by search term.
/// </summary>
public static class ExpressionHelper
{
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

    private static bool MustConvertToString(MemberInfo memberInfo)
    {
        return memberInfo.GetCustomAttribute<TextColumnAttribute>() is not null
            || memberInfo.GetCustomAttribute<XmlColumnAttribute>() is not null;
    }

    #endregion Private
}