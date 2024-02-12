using System.Linq.Expressions;
using Maxsys.Core.Events;
using Maxsys.Core.Filtering;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Interfaces.Services;

/// <summary>
/// Fornece uma interface básica para obtenção de dados.<br/>
/// </summary>
/// <typeparam name="TFilter"></typeparam>
public interface IService<TFilter> : IService
    where TFilter : IFilter
{
    #region EVENTS

    /// <summary>Evento que ocorre ao se obter um objeto através de GetAsync()</summary>
    event ValueEventHandler? GetCompleted;

    /// <summary>Evento que ocorre ao se obter uma lista de objetos através de ToListAsync()</summary>
    event ValueEventHandler? ToListCompleted;

    /// <summary>Evento que ocorre ao se obter um <see cref="ListDTO{T}"/> através de GetListAsync()</summary>
    event ValueEventHandler? GetListCompleted;

    /// <summary>Evento assíncrono que ocorre ao se obter um objeto através de GetAsync()</summary>
    event AsyncEventHandler<ValueEventArgs>? GetCompletedAsync;

    /// <summary>Evento assíncrono que ocorre ao se obter uma lista de objetos através de ToListAsync()</summary>
    event AsyncEventHandler<ValueEventArgs>? ToListCompletedAsync;

    /// <summary>Evento assíncrono que ocorre ao se obter um <see cref="ListDTO{T}"/> através de GetListAsync()</summary>
    event AsyncEventHandler<ValueEventArgs>? GetListCompletedAsync;

    #endregion EVENTS

#pragma warning disable CS1735 // XML comment has a typeparamref tag, but there is no type parameter by that name

    #region GET

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/></remarks>
    Task<TDestination?> GetAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class;

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/></remarks>
    Task<TDestination?> GetByIdAsync<TDestination>(object[] ids, CancellationToken cancellationToken = default) where TDestination : class;

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/></remarks>
    Task<TDestination?> GetSingleOrDefaultAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class;

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/></remarks>
    Task<TDestination?> GetSingleOrThrowsAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class;

    #endregion GET

    #region LIST

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/></remarks>
    Task<ListDTO<TDestination>> GetListAsync<TDestination>(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/></remarks>
    Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class;

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/></remarks>
    Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/></remarks>
    Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(TFilter filters, Pagination? pagination, Expression<Func<TDestination, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascendant, CancellationToken cancellationToken = default) where TDestination : class;

    #endregion LIST

    #region QTY

    /// <summary>
    /// Obtém uma quantidade de objetos a partir de um filtro.
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> CountAsync(TFilter? filters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se existe alguma entidade a partir de um filtro.
    /// </summary>
    /// <param name="filters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> AnyAsync(TFilter? filters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se existe alguma entidade <typeparamref name="TEntity"/> com o(s) id(s) especificados.
    /// </summary>
    /// <param name="ids">id(s) para verificação.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    ValueTask<bool> IdExistsAsync(object[] ids, CancellationToken cancellationToken = default);

    #endregion QTY

#pragma warning restore CS1735 // XML comment has a typeparamref tag, but there is no type parameter by that name
}