﻿using System.Linq.Expressions;

namespace Maxsys.Core.Filtering;

/// <summary>
/// Interface para tipificar um objeto como um Filtro de obtenção de entidades.
/// </summary>
public interface IFilter
{ }

/// <summary>
/// Interface para tipificar um objeto como um Filtro de obtenção de entidades
/// onde <typeparamref name="TEntity"/> é a entidade do filtro.
/// </summary>
public interface IFilter<TEntity> : IFilter
{
    List<Expression<Func<TEntity, bool>>> Expressions { get; }

    public void SetExpressions();
}