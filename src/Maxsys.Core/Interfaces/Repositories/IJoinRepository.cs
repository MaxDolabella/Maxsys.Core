using System.Linq.Expressions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Interfaces.Repositories;

/// <summary>
/// Fornece uma interface para um repositório da entidade <typeparamref name="TEntity"/>.<br/>
/// <para/>Aviso - "Sempre prefira Composição a Herança": <see href="https://youtu.be/LfiezdBs318?t=890"/>
/// </summary>
/// <typeparam name="TEntity">é a entidade do banco.</typeparam>
/// <typeparam name="TJoin">é a entidade do banco.</typeparam>
/// <typeparam name="TFilter">é o tipo do filtro para ser usado nas consultas do banco.</typeparam>
internal interface IJoinRepository<TEntity, TJoin, TFilter> : IRepository
    where TEntity : class
    where TJoin : class
    where TFilter : IFilter<TJoin>
{
    #region QTD

    /// <summary>
    /// Obtém uma quantidade de objetos a partir de um filtro.
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    ValueTask<int> CountAsync(TFilter filters, CancellationToken cancellation = default);

    /// <summary>
    /// Verifica se existe alguma entidade a partir de um filtro.
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    ValueTask<bool> AnyAsync(TFilter filters, CancellationToken cancellation = default);

    #endregion QTD

    #region LIST

    /// <summary>Documentar...</summary>
    Task<List<TEntity>> ToListAsync(TFilter filters, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>Documentar...</summary>
    Task<List<TEntity>> ToListAsync(TFilter filters, ListCriteria criteria, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>Documentar...</summary>
    Task<List<TEntity>> ToListAsync(TFilter filters, Pagination? pagination, Expression<Func<TEntity, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>Documentar...</summary>
    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>Documentar...</summary>
    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>>? predicate, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>Documentar...</summary>
    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>>? predicate, Pagination? pagination, Expression<Func<TDestination, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de um <typeparamref name="TFilter"/>,
    /// sem paginação e sem ordenação.
    /// <para/>
    /// Mapeamento de <typeparamref name="TEntity"/> para <typeparamref name="TDestination"/> obrigatório.
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="filters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de um <typeparamref name="TFilter"/>,
    /// paginado e ordenado utilizando um sort selector para <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="filters"></param>
    /// <param name="criteria"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>Documentar...</summary>
    Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Pagination? pagination, Expression<Func<TDestination, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default) where TDestination : class;

    #endregion LIST

    #region GET

    /// <summary>Documentar...</summary>
    Task<TEntity?> GetAsync(TFilter filters, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>Documentar...</summary>
    Task<TEntity?> GetAsync<TProperty>(TFilter filters, Expression<Func<TEntity, TProperty>> includeNavigation, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém a primeira entidade obtida de acordo com o <paramref name="predicate"/> e converte no objeto <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>Documentar...</summary>
    Task<TDestination?> GetAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class;

    #endregion GET
}