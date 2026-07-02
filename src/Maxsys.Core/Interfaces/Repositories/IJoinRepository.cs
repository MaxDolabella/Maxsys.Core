using Maxsys.Core.Filtering;
using Maxsys.Core.Sorting;
using System.Linq.Expressions;

namespace Maxsys.Core.Interfaces.Repositories;

/// <summary>
/// Fornece uma interface para um repositório da entidade <typeparamref name="TEntity"/>
/// com join não natural para <typeparamref name="TJoin"/>.<br/>
/// Utiliza <see cref="ColumnFilter"/> para filtragem dinâmica.<br/>
/// <para/>Aviso - "Sempre prefira Composição a Herança": <see href="https://youtu.be/LfiezdBs318?t=890"/>
/// </summary>
/// <typeparam name="TEntity">é a entidade do banco.</typeparam>
/// <typeparam name="TJoin">é o objeto resultante do join não natural.</typeparam>
internal interface IJoinRepository<TEntity, TJoin> : IRepository
    where TEntity : class
    where TJoin : class
{
    #region QTD

    /// <summary>
    /// Obtém a quantidade de registros a partir de uma coleção de <see cref="ColumnFilter"/>.
    /// </summary>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="cancellation">Token de cancelamento.</param>
    /// <returns>A quantidade de registros encontrados.</returns>
    ValueTask<int> CountAsync(ICollection<ColumnFilter> filters, CancellationToken cancellation = default);

    /// <summary>
    /// Verifica se existe algum registro a partir de uma coleção de <see cref="ColumnFilter"/>.
    /// </summary>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="cancellation">Token de cancelamento.</param>
    /// <returns><c>true</c> se existir ao menos um registro; caso contrário, <c>false</c>.</returns>
    ValueTask<bool> AnyAsync(ICollection<ColumnFilter> filters, CancellationToken cancellation = default);

    #endregion QTD

    #region LIST

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TEntity"/> a partir de uma coleção de <see cref="ColumnFilter"/>.
    /// </summary>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="readonly">Se <c>true</c>, aplica <c>AsNoTracking</c> na consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de entidades encontradas.</returns>
    Task<List<TEntity>> ToListAsync(ICollection<ColumnFilter> filters, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TEntity"/> a partir de uma coleção de <see cref="ColumnFilter"/>,
    /// aplicando paginação, ordenação e filtros adicionais via <see cref="ListCriteria"/>.
    /// </summary>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="criteria">Critérios de listagem (paginação, ordenação e filtros).</param>
    /// <param name="readonly">Se <c>true</c>, aplica <c>AsNoTracking</c> na consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de entidades encontradas.</returns>
    Task<List<TEntity>> ToListAsync(ICollection<ColumnFilter> filters, ListCriteria criteria, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TEntity"/> a partir de uma coleção de <see cref="ColumnFilter"/>,
    /// com paginação e ordenação explícitas.
    /// </summary>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="pagination">Configuração de paginação.</param>
    /// <param name="keySelector">Expressão para selecionar a coluna de ordenação.</param>
    /// <param name="sortDirection">Direção da ordenação.</param>
    /// <param name="readonly">Se <c>true</c>, aplica <c>AsNoTracking</c> na consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de entidades encontradas.</returns>
    Task<List<TEntity>> ToListAsync(ICollection<ColumnFilter> filters, Pagination? pagination, Expression<Func<TEntity, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de um <paramref name="predicate"/>,
    /// utilizando AutoMapper para projeção.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="predicate">Expressão de filtro sobre a entidade.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de objetos projetados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de um <paramref name="predicate"/>,
    /// aplicando <see cref="ListCriteria"/> e utilizando AutoMapper para projeção.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="predicate">Expressão de filtro sobre a entidade.</param>
    /// <param name="criteria">Critérios de listagem (paginação, ordenação e filtros).</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de objetos projetados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>>? predicate, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de um <paramref name="predicate"/>,
    /// com paginação e ordenação explícitas, utilizando AutoMapper para projeção.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="predicate">Expressão de filtro sobre a entidade.</param>
    /// <param name="pagination">Configuração de paginação.</param>
    /// <param name="keySelector">Expressão para selecionar a coluna de ordenação.</param>
    /// <param name="sortDirection">Direção da ordenação.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de objetos projetados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>>? predicate, Pagination? pagination, Expression<Func<TDestination, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma coleção de <see cref="ColumnFilter"/>,
    /// sem paginação e sem ordenação.
    /// <para/>
    /// Mapeamento de <typeparamref name="TEntity"/> para <typeparamref name="TDestination"/> obrigatório.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de objetos projetados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(ICollection<ColumnFilter> filters, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma coleção de <see cref="ColumnFilter"/>,
    /// aplicando paginação e ordenação via <see cref="ListCriteria"/>.
    /// <para/>
    /// Mapeamento de <typeparamref name="TEntity"/> para <typeparamref name="TDestination"/> obrigatório.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="criteria">Critérios de listagem (paginação, ordenação e filtros).</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de objetos projetados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(ICollection<ColumnFilter> filters, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma coleção de <see cref="ColumnFilter"/>,
    /// com paginação e ordenação explícitas, utilizando AutoMapper para projeção.
    /// <para/>
    /// Mapeamento de <typeparamref name="TEntity"/> para <typeparamref name="TDestination"/> obrigatório.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="pagination">Configuração de paginação.</param>
    /// <param name="keySelector">Expressão para selecionar a coluna de ordenação.</param>
    /// <param name="sortDirection">Direção da ordenação.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de objetos projetados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(ICollection<ColumnFilter> filters, Pagination? pagination, Expression<Func<TDestination, dynamic>> keySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default) where TDestination : class;

    #endregion LIST

    #region GET

    /// <summary>
    /// Obtém a primeira entidade encontrada a partir de uma coleção de <see cref="ColumnFilter"/>.
    /// </summary>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="readonly">Se <c>true</c>, aplica <c>AsNoTracking</c> na consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>A entidade encontrada ou <c>null</c>.</returns>
    Task<TEntity?> GetAsync(ICollection<ColumnFilter> filters, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém a primeira entidade encontrada a partir de uma coleção de <see cref="ColumnFilter"/>,
    /// incluindo uma propriedade de navegação.
    /// </summary>
    /// <typeparam name="TProperty">Tipo da propriedade de navegação.</typeparam>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="includeNavigation">Expressão para incluir a propriedade de navegação.</param>
    /// <param name="readonly">Se <c>true</c>, aplica <c>AsNoTracking</c> na consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>A entidade encontrada ou <c>null</c>.</returns>
    Task<TEntity?> GetAsync<TProperty>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TProperty>> includeNavigation, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém a primeira entidade encontrada a partir de um <paramref name="predicate"/>
    /// e projeta para <typeparamref name="TDestination"/> utilizando AutoMapper.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="predicate">Expressão de filtro sobre a entidade.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>O objeto projetado ou <c>null</c>.</returns>
    Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém a primeira entidade encontrada a partir de uma coleção de <see cref="ColumnFilter"/>
    /// e projeta para <typeparamref name="TDestination"/> utilizando AutoMapper.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>O objeto projetado ou <c>null</c>.</returns>
    Task<TDestination?> GetAsync<TDestination>(ICollection<ColumnFilter> filters, CancellationToken cancellationToken = default) where TDestination : class;

    #endregion GET
}