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
    /// <returns>uma <see cref="List{TEntity}"/> com os registros filtrados.</returns>
    Task<List<TEntity>> ToListAsync(TFilter filters, bool @readonly = true, CancellationToken cancellationToken = default);

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
    /// <returns>uma <see cref="List{TEntity}"/> com os registros filtrados.</returns>
    Task<List<TEntity>> ToListAsync(TFilter filters, ListCriteria criteria, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TEntity"/> a partir de um filtro <typeparamref name="TFilter"/>
    /// aplicando-se paginação e ordenação.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="pagination">contém o índice e a númedo página utilizada na obtenção dos items.</param>
    /// <param name="sortSelector">é a propriedade a ser ordenada.</param>
    /// <param name="sortDirection">
    /// é a direção da ordenação.
    /// <para/>Padrão é <see cref="SortDirection.Ascending"/>.
    /// </param>
    /// <param name="readonly">
    /// Para alguns ORMs como <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, especifica se a entidade deve ser monitorada.
    /// <br/>
    /// Se <paramref name="readonly"/>=<see langword="false"/>, deve ser monitorada, caso contrário, não.
    /// <para/>Padrão é <see langword="true"/> (somente leitura).
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="List{TEntity}"/> com os registros filtrados.</returns>
    Task<List<TEntity>> ToListAsync(TFilter filters, Pagination? pagination, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de um filtro <typeparamref name="TFilter"/>.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="List{TDestination}"/> com os registros filtrados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de um filtro <typeparamref name="TFilter"/>
    /// aplicando-se paginação e ordenação.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="criteria">contém critérios para obtenção dos items como paginação e lista de ordenações.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="List{TDestination}"/> com os registros filtrados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de um filtro <typeparamref name="TFilter"/>
    /// aplicando-se paginação e ordenação.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="pagination">contém o índice e a númedo página utilizada na obtenção dos items.</param>
    /// <param name="sortSelector">é a propriedade a ser ordenada.</param>
    /// <param name="sortDirection">
    /// é a direção da ordenação.
    /// <para/>Padrão é <see cref="SortDirection.Ascending"/>.
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="List{TDestination}"/> com os registros filtrados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma expression.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="projection">uma função de projeção para aplicar a cada elemento.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="List{TDestination}"/> com os registros filtrados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma expression
    /// aplicando-se paginação e ordenação.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="projection">uma função de projeção para aplicar a cada elemento.</param>
    /// <param name="criteria">contém critérios para obtenção dos items como paginação e lista de ordenações.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="List{TDestination}"/> com os registros filtrados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma expression
    /// aplicando-se paginação e ordenação.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="projection">uma função de projeção para aplicar a cada elemento.</param>
    /// <param name="pagination">contém o índice e a númedo página utilizada na obtenção dos items.</param>
    /// <param name="sortSelector">é a propriedade a ser ordenada.</param>
    /// <param name="sortDirection">
    /// é a direção da ordenação.
    /// <para/>Padrão é <see cref="SortDirection.Ascending"/>.
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="List{TDestination}"/> com os registros filtrados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);

    #endregion LIST

    #region GET

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
    /// Obtém o primeiro item <typeparamref name="TDestination"/> a partir de um filtro <typeparamref name="TFilter"/>.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/> </remarks>
    /// <returns>o primeiro item <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TDestination?> GetAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TDestination"/> a partir de um filtro <typeparamref name="TFilter"/>.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="projection">uma função de projeção para aplicar a cada elemento.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o primeiro item <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TDestination?> GetAsync<TDestination>(TFilter filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TDestination"/> a partir de um filtro <typeparamref name="TFilter"/>
    /// e ordenação definida por <paramref name="sortSelector"/> e <paramref name="sortDirection"/>.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="sortSelector">é a propriedade a ser ordenada.</param>
    /// <param name="sortDirection">
    /// é a direção da ordenação.
    /// <para/>Padrão é <see cref="SortDirection.Ascending"/>.
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/> </remarks>
    /// <returns>o primeiro item <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TDestination?> GetAsync<TDestination>(TFilter filters, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TEntity"/> a partir de um filtro <typeparamref name="TFilter"/>
    /// e ordenação definida por <paramref name="sortSelector"/> e <paramref name="sortDirection"/>.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="sortSelector">é a propriedade a ser ordenada.</param>
    /// <param name="sortDirection">
    /// é a direção da ordenação.
    /// <para/>Padrão é <see cref="SortDirection.Ascending"/>.
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
    Task<TEntity?> GetAsync(TFilter filters, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TEntity"/> a partir de um filtro <typeparamref name="TFilter"/>
    /// incluindo a navegação <typeparamref name="TProperty"/>
    /// e ordenação definida por <paramref name="sortSelector"/> e <paramref name="sortDirection"/>.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="includeNavigation">é a propriedade de navegação a ser incluída.</param>
    /// <param name="sortSelector">é a propriedade a ser ordenada.</param>
    /// <param name="sortDirection">
    /// é a direção da ordenação.
    /// <para/>Padrão é <see cref="SortDirection.Ascending"/>.
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
    Task<TEntity?> GetAsync<TProperty>(TFilter filters, Expression<Func<TEntity, TProperty>> includeNavigation, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o único item <typeparamref name="TDestination"/> a partir de um filtro <typeparamref name="TFilter"/>.
    /// <br/>
    /// Caso nenhum ou mais de um item corresponda aos critérios, <see langword="null"/> será retornado.
    /// </summary>
    ///
    /// <param name="filters">contém as condições para obtenção dos items.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o único item <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum ou mais de um item corresponda aos critérios.</returns>
    /// <remarks>
    ///     Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/>
    ///     <br/>
    ///     Esse método não lança exception.
    /// </remarks>
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
    /// <remarks>Mapeamento necessário: <typeparamref name="TEntity"/> → <typeparamref name="TDestination"/> </remarks>
    /// <returns>o único item <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum ou mais de um item corresponda aos critérios.</returns>
    /// <exception cref="InvalidOperationException"/>
    Task<TDestination?> GetSingleOrThrowsAsync<TDestination>(TFilter filters, CancellationToken cancellationToken = default) where TDestination : class;

    #endregion GET
}