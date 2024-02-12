using System.Linq.Expressions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Interfaces.Repositories;

/// <summary>
/// Fornece uma interface para um repositório da entidade <typeparamref name="TEntity"/>.<br/>
/// <para/>Aviso - "Sempre prefira Composição a Herança": <see href="https://youtu.be/LfiezdBs318?t=890"/>
/// </summary>
/// <typeparam name="TEntity">é a entidade do banco.</typeparam>
/// <typeparam name="TFilter">é o tipo do filtro para ser usado nas consultas do banco.</typeparam>
public interface IRepository<TEntity, TFilter> : IRepository<TEntity>
    where TEntity : class
    where TFilter : IFilter<TEntity>
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

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TEntity"/> a partir de um filtro <typeparamref name="TFilter"/>.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="readonly">
    /// Para alguns ORMs como <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, especifica se a entidade deve ser monitorada.
    /// <br/>
    /// Se <paramref name="readonly"/>=<see langword="false"/>, deve ser monitorada, caso contrário, não.
    /// <para/>Padrão é <see langword="true"/> (somente leitura).
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="IReadOnlyList{TEntity}"/> com os registros filtrados.</returns>
    Task<IReadOnlyList<TEntity>> ToListAsync(TFilter filters, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TEntity"/> a partir de um filtro <typeparamref name="TFilter"/>
    /// aplicando-se paginação e ordenação.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="criteria">contém critérios para obtenção dos items como paginação e lista de ordenações.</param>
    /// <param name="readonly">
    /// Para alguns ORMs como <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, especifica se a entidade deve ser monitorada.
    /// <br/>
    /// Se <paramref name="readonly"/>=<see langword="false"/>, deve ser monitorada, caso contrário, não.
    /// <para/>Padrão é <see langword="true"/> (somente leitura).
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="IReadOnlyList{TEntity}"/> com os registros filtrados.</returns>
    Task<IReadOnlyList<TEntity>> ToListAsync(TFilter filters, ListCriteria criteria, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TEntity"/> a partir de um filtro <typeparamref name="TFilter"/>
    /// aplicando-se paginação e ordenação.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="pagination">contém o índice e a númedo página utilizada na obtenção dos items.</param>
    /// <param name="sortKeySelector">é a propriedade a ser ordenada.</param>
    /// <param name="sortDirection">
    /// é a direção da ordenação.
    /// <para/>Padrão é <see cref="SortDirection.Ascendant"/>.
    /// </param>
    /// <param name="readonly">
    /// Para alguns ORMs como <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, especifica se a entidade deve ser monitorada.
    /// <br/>
    /// Se <paramref name="readonly"/>=<see langword="false"/>, deve ser monitorada, caso contrário, não.
    /// <para/>Padrão é <see langword="true"/> (somente leitura).
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="IReadOnlyList{TEntity}"/> com os registros filtrados.</returns>
    Task<IReadOnlyList<TEntity>> ToListAsync(TFilter filters, Pagination? pagination, Expression<Func<TEntity, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascendant, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma expression.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção dos items.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="IReadOnlyList{TDestination}"/> com os registros filtrados.</returns>
    Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma expression
    /// aplicando-se paginação e ordenação.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção dos items.</param>
    /// <param name="criteria">contém critérios para obtenção dos items como paginação e lista de ordenações.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="IReadOnlyList{TDestination}"/> com os registros filtrados.</returns>
    Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>>? predicate, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma expression
    /// aplicando-se paginação e ordenação.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção dos items.</param>
    /// <param name="pagination">contém o índice e a númedo página utilizada na obtenção dos items.</param>
    /// <param name="sortKeySelector">é a propriedade a ser ordenada.</param>
    /// <param name="sortDirection">
    /// é a direção da ordenação.
    /// <para/>Padrão é <see cref="SortDirection.Ascendant"/>.
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="IReadOnlyList{TDestination}"/> com os registros filtrados.</returns>
    Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>>? predicate, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascendant, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de um filtro <typeparamref name="TFilter"/>.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="IReadOnlyList{TDestination}"/> com os registros filtrados.</returns>
    Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de um filtro <typeparamref name="TFilter"/>
    /// aplicando-se paginação e ordenação.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="criteria">contém critérios para obtenção dos items como paginação e lista de ordenações.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="IReadOnlyList{TDestination}"/> com os registros filtrados.</returns>
    Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de um filtro <typeparamref name="TFilter"/>
    /// aplicando-se paginação e ordenação.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="pagination">contém o índice e a númedo página utilizada na obtenção dos items.</param>
    /// <param name="sortKeySelector">é a propriedade a ser ordenada.</param>
    /// <param name="sortDirection">
    /// é a direção da ordenação.
    /// <para/>Padrão é <see cref="SortDirection.Ascendant"/>.
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="IReadOnlyList{TDestination}"/> com os registros filtrados.</returns>
    Task<IReadOnlyList<TDestination>> ToListAsync<TDestination>(TFilter filters, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascendant, CancellationToken cancellationToken = default) where TDestination : class;

    #endregion LIST

    #region GET

    /// <summary>
    /// Obtém um item <typeparamref name="TDestination"/> a partir de seu id.
    /// <br/>
    /// Caso nenhum item corresponda aos critérios, <see langword="null"/> será retornado.
    /// </summary>
    /// <param name="id">é id para obtenção do item.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>um objeto <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TDestination?> GetByIdAsync<TDestination>(object id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um item <typeparamref name="TDestination"/> a partir de seu(s) id(s). Pode ser usado para obtenção de objeto com chave múltipla.
    /// <br/>
    /// Caso nenhum item corresponda aos critérios, <see langword="null"/> será retornado.
    /// </summary>
    /// <param name="ids">são os ids para obtenção do item.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>um objeto <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TDestination?> GetByIdAsync<TDestination>(object[] ids, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TEntity"/> a partir de um filtro <typeparamref name="TFilter"/>.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="readonly">
    /// Para alguns ORMs como <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, especifica se a entidade deve ser monitorada.
    /// <br/>
    /// Se <paramref name="readonly"/>=<see langword="false"/>, deve ser monitorada, caso contrário, não.
    /// <para/>Padrão é <see langword="true"/> (somente leitura).
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o primeiro item <typeparamref name="TEntity"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TEntity?> GetAsync(TFilter filters, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TEntity"/> a partir de um filtro <typeparamref name="TFilter"/>
    /// incluindo a navegação <typeparamref name="TProperty"/>.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="includeNavigation">é a propriedade de navegação a ser incluída.</param>
    /// <param name="readonly">
    /// Para alguns ORMs como <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, especifica se a entidade deve ser monitorada.
    /// <br/>
    /// Se <paramref name="readonly"/>=<see langword="false"/>, deve ser monitorada, caso contrário, não.
    /// <para/>Padrão é <see langword="true"/> (somente leitura).
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o primeiro item <typeparamref name="TEntity"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TEntity?> GetAsync<TProperty>(TFilter filters, Expression<Func<TEntity, TProperty>> includeNavigation, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TDestination"/> a partir de uma expression.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção do item.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o primeiro item <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TDestination"/> a partir de um filtro <typeparamref name="TFilter"/>.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o primeiro item <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TDestination?> GetAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TDestination"/> a partir de um filtro <typeparamref name="TFilter"/>
    /// e ordenação definida por <paramref name="sortKeySelector"/> e <paramref name="sortDirection"/>.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="sortKeySelector">é a propriedade a ser ordenada.</param>
    /// <param name="sortDirection">
    /// é a direção da ordenação.
    /// <para/>Padrão é <see cref="SortDirection.Ascendant"/>.
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o primeiro item <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TDestination?> GetAsync<TDestination>(TFilter filters, Expression<Func<TEntity, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascendant, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TDestination"/> a partir de uma expression
    /// e ordenação definida por <paramref name="sortKeySelector"/> e <paramref name="sortDirection"/>.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção do item.</param>
    /// <param name="sortKeySelector">é a propriedade a ser ordenada.</param>
    /// <param name="sortDirection">
    /// é a direção da ordenação.
    /// <para/>Padrão é <see cref="SortDirection.Ascendant"/>.
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o primeiro item <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascendant, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TEntity"/> a partir de um filtro <typeparamref name="TFilter"/>
    /// e ordenação definida por <paramref name="sortKeySelector"/> e <paramref name="sortDirection"/>.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="sortKeySelector">é a propriedade a ser ordenada.</param>
    /// <param name="sortDirection">
    /// é a direção da ordenação.
    /// <para/>Padrão é <see cref="SortDirection.Ascendant"/>.
    /// </param>
    /// <param name="readonly">
    /// Para alguns ORMs como <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, especifica se a entidade deve ser monitorada.
    /// <br/>
    /// Se <paramref name="readonly"/>=<see langword="false"/>, deve ser monitorada, caso contrário, não.
    /// <para/>Padrão é <see langword="true"/> (somente leitura).
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o primeiro item <typeparamref name="TEntity"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TEntity?> GetAsync(TFilter filters, Expression<Func<TEntity, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascendant, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TEntity"/> a partir de um filtro <typeparamref name="TFilter"/>
    /// incluindo a navegação <typeparamref name="TProperty"/>
    /// e ordenação definida por <paramref name="sortKeySelector"/> e <paramref name="sortDirection"/>.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="includeNavigation">é a propriedade de navegação a ser incluída.</param>
    /// <param name="sortKeySelector">é a propriedade a ser ordenada.</param>
    /// <param name="sortDirection">
    /// é a direção da ordenação.
    /// <para/>Padrão é <see cref="SortDirection.Ascendant"/>.
    /// </param>
    /// <param name="readonly">
    /// Para alguns ORMs como <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, especifica se a entidade deve ser monitorada.
    /// <br/>
    /// Se <paramref name="readonly"/>=<see langword="false"/>, deve ser monitorada, caso contrário, não.
    /// <para/>Padrão é <see langword="true"/> (somente leitura).
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o primeiro item <typeparamref name="TEntity"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TEntity?> GetAsync<TProperty>(TFilter filters, Expression<Func<TEntity, TProperty>> includeNavigation, Expression<Func<TEntity, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascendant, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o único item <typeparamref name="TDestination"/> a partir de um filtro <typeparamref name="TFilter"/>.
    /// <br/>
    /// Caso nenhum ou mais de um item corresponda aos critérios, <see langword="null"/> será retornado.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o único item <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum ou mais de um item corresponda aos critérios.</returns>
    /// <remarks>Esse método não lança exception.</remarks>
    Task<TDestination?> GetSingleOrDefaultAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém o único item <typeparamref name="TDestination"/> a partir de um filtro <typeparamref name="TFilter"/>.
    /// <br/>
    /// Caso nenhum item corresponda aos critérios, <see langword="null"/> será retornado.
    /// <br/>
    /// Caso mais de um item corresponda aos critérios, uma <see cref="InvalidOperationException"/> será lançada.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o único item <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum ou mais de um item corresponda aos critérios.</returns>
    /// <exception cref="InvalidOperationException"/>
    Task<TDestination?> GetSingleOrThrowsAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class;

    #endregion GET
}