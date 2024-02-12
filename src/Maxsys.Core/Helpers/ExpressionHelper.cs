using System.Linq.Expressions;
using System.Reflection;
using Maxsys.Core.Attributes;

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

        Expression result = propertyInfo.GetCustomAttribute<TextAttribute>() is null
            ? memberExpression
            : Expression.Call(typeof(Convert).GetMethod(nameof(Convert.ToString), [typeof(string)])!, memberExpression);

        return result;
    }
}