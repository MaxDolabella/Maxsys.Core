using System.Linq.Expressions;
using Maxsys.Core.Events;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Interfaces.Services;

/// <summary>
/// Fornece uma interface básica para obtenção de dados.<br/>
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IService<TEntity> : IService
    where TEntity : class
{
    #region EVENTS

    /// <summary>Evento assíncrono que ocorre ao se obter um objeto através de <b><c>GetAsync()</c></b></summary>
    event AsyncEventHandler<ValueEventArgs>? GetCompletedAsync;

    /// <summary>Evento assíncrono que ocorre ao se obter uma lista de objetos através de <b><c>ToListAsync()</c></b></summary>
    event AsyncEventHandler<ValueEventArgs>? ToListCompletedAsync;

    /// <summary>Evento assíncrono que ocorre ao se obter um <see cref="ListDTO{T}"/> através de <b><c>GetListAsync()</c></b></summary>
    event AsyncEventHandler<ValueEventArgs>? GetListCompletedAsync;

    #endregion EVENTS

    #region GET

    Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default);

    Task<TDestination?> GetByIdAsync<TDestination>(object[] ids, CancellationToken cancellationToken = default);

    Task<TDestination?> GetSingleOrDefaultAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<TDestination?> GetSingleOrThrowsAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    #endregion GET

    #region LIST

    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);

    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default);

    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);

    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    Task<ListDTO<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    Task<ListDTO<TDestination>> GetListAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    #endregion LIST

    #region QTY

    /// <summary>
    /// Verifica se existe alguma entidade <typeparamref name="TEntity"/> com o(s) id(s) especificados.
    /// </summary>
    /// <param name="ids">id(s) para verificação.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    ValueTask<bool> IdExistsAsync(object[] ids, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma quantidade de objetos a partir de um filtro.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se existe alguma entidade a partir de um filtro.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    #endregion QTY
}