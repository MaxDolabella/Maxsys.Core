using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Maxsys.Data.Extensions;

public static class EntityFrameworkExtensions
{
    extension(IEntityType entityType)
    {
        public Expression<Func<T, bool>> GetIdExpression<T>(object[] ids) where T : class
        {
            if (entityType.ClrType != typeof(T))
            {
                throw new InvalidOperationException("Type of generic and IEntityType are not the same.");
            }

            var primaryKey = entityType.FindPrimaryKey()
                ?? throw new KeyNotFoundException($"Type {entityType.ClrType.Name} has no declared primary key.");

            var keys = primaryKey.Properties;

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
}