using System;
using System.Linq.Expressions;

namespace Maxsys.Core.Filtering;

/// <summary>
/// Provides a Filter typification for an object.
/// </summary>
public interface IFilter
{ }

/// <summary>
/// Provides a Filter typification for an object.
/// </summary>
public interface IFilter<TEntity> : IFilter
{
    /// <summary>
    /// Converts this filter into a expression to be used with Linq.
    /// </summary>
    /// <returns></returns>
    Expression<Func<TEntity, bool>> ToExpression();
}