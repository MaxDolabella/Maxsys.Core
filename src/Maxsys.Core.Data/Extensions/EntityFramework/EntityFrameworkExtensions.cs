using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Maxsys.Core.Data.Extensions;

public static class EntityFrameworkExtensions
{
    public static Expression<Func<T, bool>> GetIdExpression<T>(this IEntityType entityType, object[] ids) where T : class
    {
        if (entityType.ClrType != typeof(T))
        {
            throw new InvalidOperationException("Type of generic and IEntityType are not the same.");
        }

        var declaredKeys = entityType.GetDeclaredKeys().Where(k => k.IsPrimaryKey()).ToList();

        // KeyNotFoundException: nenhuma PK Constraint
        if (declaredKeys.Count == 0)
        {
            throw new KeyNotFoundException($"Type {entityType.ClrType.Name} has no declared primary key.");
        }

        // InvalidOperationException: múltiplas PK Constraints
        if (declaredKeys.Count != 1)
        {
            throw new InvalidOperationException($"There are {declaredKeys.Count} declared PK constraints. Must be only one.");
        }

        var keys = declaredKeys[0].Properties;

        // ArgumentException: Qtd de keys e ids passados diferem.
        if (keys.Count != ids.Length)
        {
            throw new ArgumentException($"Type {entityType.ClrType.Name} has {keys.Count} keys, but passed argument has {ids.Length} items.");
        }

        var parameterExpression = Expression.Parameter(typeof(T), "x");

        // Cria uma lista com cada comparação de chave
        var expressions = new List<Expression>();
        for (int i = 0; i < keys.Count; i++)
        {
            var key = keys[i];

            var memberExpression = Expression.Property(parameterExpression, key.Name);
            var constantExpression = Expression.Constant(ids[i]);

            expressions.Add(Expression.Equal(memberExpression, constantExpression));
        }

        // Une cada comparação com um "AND"
        Expression? body = null;
        foreach (var expression in expressions)
        {
            body = body is null ? expression : Expression.And(body, expression);
        }

        return Expression.Lambda<Func<T, bool>>(body!, parameterExpression);
    }
}