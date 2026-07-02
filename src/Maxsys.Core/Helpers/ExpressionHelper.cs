using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
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

    /// <summary>
    /// Inverso de <see cref="GetMemberAccessExpression{T}"/>: extrai o caminho completo da propriedade
    /// a partir de uma expression, retornando uma string em dot notation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expression"></param>
    /// <remarks>
    /// Para uma expression "<c>x => x.Address.State.Name</c>", por exemplo,
    /// será retornada a string "<c>Address.State.Name</c>".
    /// </remarks>
    public static string GetMemberPath<T>(Expression<Func<T, dynamic>> expression)
    {
        // Desempacota o Convert gerado pelo compilador: {x => convert(x.Prop, Object)}
        Expression body = expression.Body is UnaryExpression { NodeType: ExpressionType.Convert } unary
            ? unary.Operand
            : expression.Body;

        return GetMemberPathFromExpression(body);
    }

    /// <summary>
    /// Percorre recursivamente uma <see cref="MemberExpression"/> aninhada
    /// e monta o caminho em dot notation.
    /// </summary>
    private static string GetMemberPathFromExpression(Expression expression)
    {
        if (expression is ParameterExpression)
            return string.Empty;

        if (expression is MemberExpression member)
        {
            var parentPath = GetMemberPathFromExpression(member.Expression!);
            return string.IsNullOrEmpty(parentPath) ? member.Member.Name : $"{parentPath}.{member.Member.Name}";
        }

        throw new InvalidOperationException(
            $"Expression node '{expression.NodeType}' is not supported. Only member access expressions are allowed.");
    }

    #region Private

    private static List<PropertyInfo> GetPropertyInfos(Type type, List<string> chainedProperties)
    {
        if (chainedProperties.Count == 0)
            throw new ArgumentException($"Argument {nameof(chainedProperties)} cannot be empty.", nameof(chainedProperties));

        var propertyInfo = type.GetProperty(chainedProperties[0], BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
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

    #region Column Filters

    /// <summary>
    /// Constrói uma <see cref="Expression{TDelegate}"/> do tipo <c>Func&lt;TEntity, bool&gt;</c>
    /// a partir de um <see cref="ColumnFilter"/>, resolvendo o caminho da propriedade (incluindo dot notation)
    /// e aplicando a comparação correspondente ao <see cref="ColumnFilter.MatchMode"/>.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade.</typeparam>
    /// <param name="filter">Filtro de coluna contendo field, value e matchMode.</param>
    /// <returns>Expression pronta para ser aplicada em um <see cref="IQueryable{T}.Where"/>.</returns>
    /// <exception cref="NotSupportedException">Quando o <see cref="ColumnFilter.MatchMode"/> não é suportado.</exception>
    public static Expression<Func<T, bool>> BuildColumnFilterExpression<T>(ColumnFilter filter)
        where T : class
    {
        var parameter = Expression.Parameter(typeof(T), "x");

        // Reutiliza GetPropertyInfos (existente) para resolver dot notation: "consignee.name" → [Consignee, Name]
        var chainedProperties = filter.Field.Split('.', StringSplitOptions.RemoveEmptyEntries).ToList();
        var propertyInfos = GetPropertyInfos(typeof(T), chainedProperties);

        // Reutiliza GetPropertyExpression (existente) para construir o acesso ao membro,
        // respeitando [TextColumn] e [XmlColumn] automaticamente.
        Expression memberAccess = GetPropertyExpression(parameter, propertyInfos[0]);
        foreach (var propInfo in propertyInfos.Skip(1))
        {
            memberAccess = GetPropertyExpression(memberAccess, propInfo);
        }

        // Tipo real da propriedade final (para conversão de valores)
        var propertyType = propertyInfos[^1].PropertyType;

        // Constrói a comparação com base no matchMode
#pragma warning disable CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.
#pragma warning disable CS8604 // Possible null reference argument.
        Expression body = filter.MatchMode switch
        {
            // ── String ──
            FilterMatchModes.Contains => BuildStringCall(memberAccess, nameof(string.Contains), filter.Value),
            FilterMatchModes.StartsWith => BuildStringCall(memberAccess, nameof(string.StartsWith), filter.Value),
            FilterMatchModes.EndsWith => BuildStringCall(memberAccess, nameof(string.EndsWith), filter.Value),

            // ── Igualdade ──
            FilterMatchModes.Equals => Expression.Equal(memberAccess, ConvertToConstant(filter.Value, propertyType)),
            FilterMatchModes.NotEquals => Expression.NotEqual(memberAccess, ConvertToConstant(filter.Value, propertyType)),

            // ── Comparação numérica ──
            FilterMatchModes.Gt => Expression.GreaterThan(memberAccess, ConvertToConstant(filter.Value, propertyType)),
            FilterMatchModes.Gte => Expression.GreaterThanOrEqual(memberAccess, ConvertToConstant(filter.Value, propertyType)),
            FilterMatchModes.Lt => Expression.LessThan(memberAccess, ConvertToConstant(filter.Value, propertyType)),
            FilterMatchModes.Lte => Expression.LessThanOrEqual(memberAccess, ConvertToConstant(filter.Value, propertyType)),

            // ── Range ──
            FilterMatchModes.Between => BuildBetween(memberAccess, filter.Value, propertyType),

            // ── Coleção ──
            FilterMatchModes.In => BuildContains(memberAccess, filter.Value, propertyType, negate: false),
            FilterMatchModes.NotIn => BuildContains(memberAccess, filter.Value, propertyType, negate: true),

            // ── Data ──
            FilterMatchModes.DateIs => BuildDateComparison(memberAccess, filter.Value, ExpressionType.Equal),
            FilterMatchModes.DateIsNot => BuildDateComparison(memberAccess, filter.Value, ExpressionType.NotEqual),
            FilterMatchModes.DateBefore => BuildDateComparison(memberAccess, filter.Value, ExpressionType.LessThan),
            FilterMatchModes.DateAfter => BuildDateComparison(memberAccess, filter.Value, ExpressionType.GreaterThan),

            // _ => throw new NotSupportedException($"MatchMode '{filter.MatchMode}' is not supported.")
            // Sem o _ => throw — se amanhã adicionar um novo valor no enum, o compilador gera warning CS8509 avisando
            // que o switch não é exaustivo.
        };
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    #endregion Column Filters

    #region Column Filter Helpers

    /// <summary>
    /// Gera uma chamada a um método de <see cref="string"/> (<c>Contains</c>, <c>StartsWith</c> ou <c>EndsWith</c>)
    /// com null-check: <c>x.Prop != null &amp;&amp; x.Prop.Method(value)</c>.
    /// </summary>
    private static Expression BuildStringCall(Expression member, string methodName, object value)
    {
        var method = typeof(string).GetMethod(methodName, [typeof(string)])!;
        var constant = Expression.Constant(Convert.ToString(value), typeof(string));

        // Null safety: evita NullReferenceException em propriedades nullable
        var notNull = Expression.NotEqual(member, Expression.Constant(null, typeof(string)));
        var call = Expression.Call(member, method, constant);

        return Expression.AndAlso(notNull, call);
    }

    /// <summary>
    /// Gera <c>collection.Contains(x.Prop)</c> ou sua negação,
    /// convertendo o <paramref name="value"/> (esperado como <see cref="JsonElement"/> array) para uma lista tipada.
    /// </summary>
    private static Expression BuildContains(Expression member, object value, Type propertyType, bool negate)
    {
        var list = ConvertJsonToList(value, propertyType);

        // Enumerable.Contains<T>(IEnumerable<T>, T)
        var containsMethod = typeof(Enumerable)
            .GetMethods()
            .First(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2)
            .MakeGenericMethod(propertyType);

        Expression call = Expression.Call(containsMethod, Expression.Constant(list), member);

        return negate ? Expression.Not(call) : call;
    }

    /// <summary>
    /// Gera <c>x.Prop &gt;= min &amp;&amp; x.Prop &lt;= max</c> a partir de um array <c>[min, max]</c>.
    /// </summary>
    private static Expression BuildBetween(Expression member, object value, Type propertyType)
    {
        if (value is not JsonElement json || json.ValueKind != JsonValueKind.Array)
            throw new ArgumentException($"MatchMode 'between' requires a [min, max] array. Received: {value}");

        var elements = json.EnumerateArray().ToArray();
        if (elements.Length != 2)
            throw new ArgumentException($"MatchMode 'between' requires exactly 2 elements. Received: {elements.Length}");

        var min = ConvertToConstant(elements[0], propertyType);
        var max = ConvertToConstant(elements[1], propertyType);

        return Expression.AndAlso(
            Expression.GreaterThanOrEqual(member, min),
            Expression.LessThanOrEqual(member, max));
    }

    /// <summary>
    /// Gera uma comparação de data, convertendo o <paramref name="value"/> para
    /// <see cref="DateTime"/> ou <see cref="DateTimeOffset"/> conforme o tipo real da propriedade.
    /// Aceita DateTime, DateTimeOffset, JsonElement e string (ISO 8601).
    /// <para/>
    /// Usa <see cref="WrapAsParameterExpression"/> ao invés de <see cref="Expression.Constant(object?, Type)"/>
    /// para forçar o EF Core a parametrizar o valor (evita inline como literal SQL, que causa
    /// erro de conversão em SQL Server com DATEFORMAT dmy).
    /// </summary>
    private static Expression BuildDateComparison(Expression member, object value, ExpressionType comparison)
    {
        // Pega o tipo da propriedade no banco
        var underlyingType = Nullable.GetUnderlyingType(member.Type) ?? member.Type;

        object dateValue = (value, underlyingType == typeof(DateTimeOffset)) switch
        {
            (DateTimeOffset dto, true) => dto,
            (DateTimeOffset dto, false) => dto.DateTime,
            (DateTime dt, true) => new DateTimeOffset(dt, TimeSpan.Zero),
            (DateTime dt, false) => dt,
            _ => underlyingType == typeof(DateTimeOffset)
                ? DateTimeOffset.Parse(ExtractRawString(value), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
                : (object)DateTime.Parse(ExtractRawString(value), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
        };

        var parameterExpr = WrapAsParameterExpression(dateValue, member.Type);

        return Expression.MakeBinary(comparison, member, parameterExpr);
    }

    /// <summary>
    /// Extrai a string bruta de um valor, tratando <see cref="JsonElement"/> adequadamente
    /// para evitar problemas de cultura no <see cref="object.ToString"/>.
    /// </summary>
    private static string ExtractRawString(object value)
    {
        return value is JsonElement json && json.ValueKind == JsonValueKind.String
            ? json.GetString()!
            : value.ToString()!;
    }

    /// <summary>
    /// Encapsula um valor em um field access sobre um objeto wrapper, simulando o padrão de closure
    /// do compilador C#. Isso força o EF Core a gerar um <c>SqlParameter</c> tipado ao invés de
    /// inline o valor como literal string no SQL — evitando erros de conversão de data em
    /// SQL Server com <c>SET DATEFORMAT dmy</c>.
    /// </summary>
    /// <remarks>
    /// <c>Expression.Constant(dateValue, typeof(DateTime))</c> gera SQL literal: <c>WHERE col > '2026-02-26T00:00:00'</c><br/>
    /// <c>WrapAsParameterExpression(dateValue, typeof(DateTime))</c> gera SQL parametrizado: <c>WHERE col > @__p_0</c>
    /// </remarks>
    private static Expression WrapAsParameterExpression(object value, Type targetType)
    {
        var holderType = typeof(ValueHolder<>).MakeGenericType(targetType);
        var holder = Activator.CreateInstance(holderType)!;
        holderType.GetField(nameof(ValueHolder<object>.Value))!.SetValue(holder, value);

        return Expression.Field(Expression.Constant(holder), nameof(ValueHolder<object>.Value));
    }

    /// <summary>
    /// Converte um valor (possivelmente <see cref="JsonElement"/>) para um <see cref="ConstantExpression"/>
    /// do <paramref name="targetType"/> especificado, respeitando tipos nullable.
    /// </summary>
    private static ConstantExpression ConvertToConstant(object value, Type targetType)
    {
        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        // JsonElement precisa ser extraído como string antes da conversão
        var rawValue = value is JsonElement json ? json.ToString() : value;

        object converted;
        if (underlyingType == typeof(Guid))
            converted = Guid.Parse(rawValue.ToString()!);
        else if (underlyingType.IsEnum)
            converted = ConvertToEnum(underlyingType, rawValue);
        else if (underlyingType == typeof(DateTimeOffset))
            converted = DateTimeOffset.Parse(rawValue.ToString()!, CultureInfo.InvariantCulture);
        else
            converted = Convert.ChangeType(rawValue, underlyingType, CultureInfo.InvariantCulture);

        return Expression.Constant(converted, targetType);
    }

    /// <summary>
    /// Converte um valor bruto (string ou numérico) em um enum do tipo especificado.
    /// Aceita tanto o valor numérico ("1") quanto o nome textual ("Active"), case-insensitive.
    /// </summary>
    private static object ConvertToEnum(Type enumType, object? rawValue)
    {
        var str = rawValue?.ToString();
        if (string.IsNullOrEmpty(str))
            throw new ArgumentException($"Cannot convert null/empty value to enum '{enumType.Name}'.");

        // Valor numérico: "1", "2" → Enum.ToObject
        if (long.TryParse(str, out var numericValue))
            return Enum.ToObject(enumType, numericValue);

        // Nome textual: "Active", "active" → Enum.Parse
        return Enum.Parse(enumType, str, ignoreCase: true);
    }

    /// <summary>
    /// Converte um <see cref="JsonElement"/> do tipo array em uma <see cref="IList"/> tipada
    /// compatível com <see cref="Enumerable.Contains{T}(IEnumerable{T}, T)"/>.
    /// </summary>
    private static IList ConvertJsonToList(object value, Type elementType)
    {
        var underlyingType = Nullable.GetUnderlyingType(elementType) ?? elementType;
        var listType = typeof(List<>).MakeGenericType(elementType);
        var list = (IList)Activator.CreateInstance(listType)!;

        if (value is JsonElement jsonArray && jsonArray.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in jsonArray.EnumerateArray())
            {
                var raw = item.ToString();
                object converted;
                if (underlyingType == typeof(Guid))
                    converted = Guid.Parse(raw!);
                else if (underlyingType.IsEnum)
                    converted = ConvertToEnum(underlyingType, raw);
                else if (underlyingType == typeof(DateTimeOffset))
                    converted = DateTimeOffset.Parse(raw!, CultureInfo.InvariantCulture);
                else
                    converted = Convert.ChangeType(raw, underlyingType, CultureInfo.InvariantCulture);

                list.Add(converted);
            }
        }
        else
        {
            throw new ArgumentException($"Expected a JSON array for 'in'/'notIn' matchMode. Received: {value}");
        }

        return list;
    }

    #endregion Column Filter Helpers

    /// <summary>
    /// Wrapper genérico utilizado por <see cref="WrapAsParameterExpression"/> para simular
    /// uma closure e forçar parametrização pelo EF Core.
    /// </summary>
    private sealed class ValueHolder<T>
    {
        public T Value = default!;
    }
}