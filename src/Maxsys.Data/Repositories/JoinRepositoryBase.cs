using Maxsys.Data.Extensions;
using Maxsys.Core.Extensions;
using Maxsys.Core.Filtering;
using Maxsys.Core.Interfaces.Mapping;
using Maxsys.Core.Interfaces.Repositories;
using Maxsys.Core.Sorting;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Maxsys.Data.Repositories;

/// <summary>
/// Repositório usado quando busca é baseada numa entidade <typeparamref name="TEntity"/>,
/// convertida em um objeto com join não natural <typeparamref name="TJoin"/> e enfim mapeada para o objeto de destino.
/// Utiliza <see cref="ColumnFilter"/> para filtragem dinâmica.
/// </summary>
/// <typeparam name="TEntity">é a entidade do banco.</typeparam>
/// <typeparam name="TJoin">é o objeto resultante do join não natural.</typeparam>
public abstract class JoinRepositoryBase<TEntity, TJoin> : RepositoryBase<TEntity>, IRepository<TEntity>
    where TEntity : class
    where TJoin : class
{
    #region CONSTRUCTOR

    public JoinRepositoryBase(DbContext context, IQueryProjector projector)
        : base(context, projector)
    {
    }

    #endregion CONSTRUCTOR

    #region PROT

    /// <summary>
    /// Chokepoint único para projeção <typeparamref name="TJoin"/> → <typeparamref name="TDestination"/>
    /// via <see cref="IQueryProjector"/>. Subclasses podem sobrescrever para injetar políticas de leitura
    /// (ex.: Field-Level Security) que reescrevam o <c>Select</c> traduzido para SQL.
    /// </summary>
    /// <remarks>
    /// Implementação default: <c>_projector.Project&lt;TDestination&gt;(source)</c>.
    /// Toda projeção interna do join passa por aqui — não chame o projector diretamente
    /// em <see cref="JoinRepositoryBase{TEntity, TJoin}"/>.
    /// </remarks>
    protected virtual IQueryable<TDestination> ApplyJoinProjection<TDestination>(IQueryable<TJoin> source)
        => _projector.Project<TDestination>(source);

    protected IOrderedQueryable<T> ApplyOrderBy<T>(IQueryable<T> query, Expression<Func<T, dynamic>> sortSelector, SortDirection sortDirection)
    {
        return sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortSelector)
            : query.OrderByDescending(sortSelector);
    }

    /// <summary>
    /// Obtém o <see cref="IQueryable{TEntity}"/> base aplicando os <see cref="ColumnFilter"/> informados.
    /// </summary>
    /// <remarks>
    /// <code>
    /// var query = await GetQueryable(predicate: null, @readonly: true, cancellation);
    ///
    /// query = query.ApplyFilters(filters);
    ///
    /// return query;
    /// </code>
    /// </remarks>
    protected virtual async ValueTask<IQueryable<TEntity>> GetQueryable(ICollection<ColumnFilter> filters, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(predicate: null, @readonly: true, cancellationToken);

        return query.ApplyFilters(filters);
    }

    /// <summary>
    /// Obtém o <see cref="IQueryable{TJoin}"/> aplicando os <see cref="ColumnFilter"/> informados,
    /// com ordenação opcional sobre a entidade base.
    /// </summary>
    /// <remarks>
    /// <code>
    /// var baseQuery = await GetQueryable(filters ?? [], @readonly, cancellation);
    ///
    /// if(sortSelector is not null &amp;&amp; sortDirection is not null)
    /// {
    ///     baseQuery = ApplyOrderBy(baseQuery, sortSelector, sortDirection.Value);
    /// }
    ///
    /// var query = EntityToJoinQueryableConvert(baseQuery, filters);
    /// </code>
    /// </remarks>
    protected virtual async ValueTask<IQueryable<TJoin>> GetJoinQueryable(ICollection<ColumnFilter>? filters, Expression<Func<TEntity, dynamic>>? sortSelector = null, SortDirection? sortDirection = null, bool @readonly = true, CancellationToken cancellation = default)
    {
        var baseQuery = await GetQueryable(filters ?? [], @readonly, cancellation);

        if (sortSelector is not null && sortDirection is not null)
        {
            baseQuery = ApplyOrderBy(baseQuery, sortSelector, sortDirection.Value);
        }

        var query = EntityToJoinQueryableConvert(baseQuery, filters);

        return await ValueTask.FromResult(@readonly ? query.AsNoTracking() : query.AsTracking());
    }

    /// <summary>
    /// Obtém o <see cref="IQueryable{TJoin}"/> aplicando um <paramref name="predicate"/> sobre a entidade base,
    /// com ordenação opcional.
    /// </summary>
    /// <remarks>
    /// <code>
    /// var baseQuery = await GetQueryable(predicate, @readonly, cancellation);
    ///
    /// if(sortSelector is not null &amp;&amp; sortDirection is not null)
    /// {
    ///     baseQuery = ApplyOrderBy(baseQuery, sortSelector, sortDirection.Value);
    /// }
    ///
    /// var query = EntityToJoinQueryableConvert(baseQuery, null);
    /// </code>
    /// </remarks>
    protected virtual async ValueTask<IQueryable<TJoin>> GetJoinQueryable(Expression<Func<TEntity, bool>>? predicate, Expression<Func<TEntity, dynamic>>? sortSelector = null, SortDirection? sortDirection = null, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var baseQuery = await GetQueryable(predicate, @readonly, cancellationToken);

        if (sortSelector is not null && sortDirection is not null)
        {
            baseQuery = ApplyOrderBy(baseQuery, sortSelector, sortDirection.Value);
        }

        var query = EntityToJoinQueryableConvert(baseQuery, null);

        return await ValueTask.FromResult(@readonly ? query.AsNoTracking() : query.AsTracking());
    }

    /// <summary>
    /// Converte o <see cref="IQueryable{TEntity}"/> base em um <see cref="IQueryable{TJoin}"/>
    /// aplicando os joins não naturais necessários.
    /// </summary>
    /// <remarks>
    /// <code>
    /// return query.LeftOuterJoin(Context.OtherCollection,
    ///         entity => entity.otherId,
    ///         other => other.Id,
    ///         join => new { Entity = join.Outer, Other = join.Inner })
    ///     .Select(a => new Join
    ///     {
    ///         Entity = a.Entity,
    ///         Other = a.Other
    ///     });
    /// </code>
    /// </remarks>
    /// <param name="query">Query base da entidade.</param>
    /// <param name="filters">Coleção de filtros dinâmicos (pode ser usado para filtros adicionais no join).</param>
    /// <returns>Query com o join aplicado.</returns>
    protected abstract IQueryable<TJoin> EntityToJoinQueryableConvert(IQueryable<TEntity> query, ICollection<ColumnFilter>? filters);

    #endregion PROT

    #region QTY

    /// <summary>
    /// Obtém a quantidade de registros a partir de uma coleção de <see cref="ColumnFilter"/>.
    /// </summary>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="cancellation">Token de cancelamento.</param>
    /// <returns>A quantidade de registros encontrados.</returns>
    public virtual async ValueTask<int> CountAsync(ICollection<ColumnFilter> filters, CancellationToken cancellation = default)
    {
        var query = await GetJoinQueryable(filters, null, null, true, cancellation);

        return await query.CountAsync(cancellation);
    }

    /// <summary>
    /// Verifica se existe algum registro a partir de uma coleção de <see cref="ColumnFilter"/>.
    /// </summary>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="cancellation">Token de cancelamento.</param>
    /// <returns><c>true</c> se existir ao menos um registro; caso contrário, <c>false</c>.</returns>
    public virtual async ValueTask<bool> AnyAsync(ICollection<ColumnFilter> filters, CancellationToken cancellation = default)
    {
        var query = await GetJoinQueryable(filters, null, null, true, cancellation);

        return await query.AnyAsync(cancellation);
    }

    #endregion QTY

    #region LIST

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TEntity"/> a partir de uma coleção de <see cref="ColumnFilter"/>.
    /// </summary>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="readonly">Se <c>true</c>, aplica <c>AsNoTracking</c> na consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de entidades encontradas.</returns>
    public virtual async Task<List<TEntity>> ToListAsync(
        ICollection<ColumnFilter> filters,
        bool @readonly = true,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, @readonly, cancellationToken);

        return await query.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TEntity"/> a partir de uma coleção de <see cref="ColumnFilter"/>,
    /// aplicando paginação, ordenação e filtros adicionais via <see cref="ListCriteria"/>.
    /// </summary>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="criteria">Critérios de listagem (paginação, ordenação e filtros).</param>
    /// <param name="readonly">Se <c>true</c>, aplica <c>AsNoTracking</c> na consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de entidades encontradas.</returns>
    public virtual async Task<List<TEntity>> ToListAsync(
        ICollection<ColumnFilter> filters,
        ListCriteria criteria,
        bool @readonly = true,
        CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(filters, @readonly, cancellationToken))
            .ApplyCriteria(criteria);

        return await query.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TEntity"/> a partir de uma coleção de <see cref="ColumnFilter"/>,
    /// com paginação e ordenação explícitas.
    /// </summary>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="pagination">Configuração de paginação.</param>
    /// <param name="sortSelector">Expressão para selecionar a coluna de ordenação.</param>
    /// <param name="sortDirection">Direção da ordenação.</param>
    /// <param name="readonly">Se <c>true</c>, aplica <c>AsNoTracking</c> na consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de entidades encontradas.</returns>
    public virtual async Task<List<TEntity>> ToListAsync(
        ICollection<ColumnFilter> filters,
        Pagination? pagination,
        Expression<Func<TEntity, dynamic>> sortSelector,
        SortDirection sortDirection = SortDirection.Ascending,
        bool @readonly = true,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, @readonly, cancellationToken);

        var orderedQuery = ApplyOrderBy(query, sortSelector, sortDirection);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    public override async Task<List<TDestination>> ToListAsync<TDestination>(
        Expression<Func<TEntity, bool>>? predicate,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyJoinProjection<TDestination>(await GetJoinQueryable(predicate, null, null, true, cancellationToken));

        return await query.ToListAsync(cancellationToken);
    }

    public override async Task<List<TDestination>> ToListAsync<TDestination>(
        Expression<Func<TEntity, bool>>? predicate,
        ListCriteria criteria,
        CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyJoinProjection<TDestination>(await GetJoinQueryable(predicate, null, null, true, cancellationToken))
            .ApplyCriteria(criteria);

        return await query.ToListAsync(cancellationToken);
    }

    public override async Task<List<TDestination>> ToListAsync<TDestination>(
        Expression<Func<TEntity, bool>>? predicate,
        Pagination? pagination,
        Expression<Func<TDestination, dynamic>> sortSelector,
        SortDirection sortDirection = SortDirection.Ascending,
        CancellationToken cancellationToken = default)

    {
        var query = ApplyJoinProjection<TDestination>(await GetJoinQueryable(predicate, null, null, false, cancellationToken));

        var orderedQuery = ApplyOrderBy(query, sortSelector, sortDirection);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    // ===

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma coleção de <see cref="ColumnFilter"/>,
    /// sem paginação e sem ordenação, utilizando o projetor do repositório (IQueryProjector) para projeção.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de objetos projetados.</returns>
    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        ICollection<ColumnFilter> filters,
        CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyJoinProjection<TDestination>(await GetJoinQueryable(filters, null, null, true, cancellationToken));

        return await query.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma coleção de <see cref="ColumnFilter"/>,
    /// aplicando paginação e ordenação via <see cref="ListCriteria"/>, utilizando o projetor do repositório (IQueryProjector) para projeção.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="criteria">Critérios de listagem (paginação, ordenação e filtros).</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de objetos projetados.</returns>
    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        ICollection<ColumnFilter> filters,
        ListCriteria criteria,
        CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyJoinProjection<TDestination>(await GetJoinQueryable(filters, null, null, true, cancellationToken))
            .ApplyCriteria(criteria);

        return await query.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma coleção de <see cref="ColumnFilter"/>,
    /// com paginação e ordenação explícitas, utilizando o projetor do repositório (IQueryProjector) para projeção.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="pagination">Configuração de paginação.</param>
    /// <param name="sortSelector">Expressão para selecionar a coluna de ordenação.</param>
    /// <param name="sortDirection">Direção da ordenação.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de objetos projetados.</returns>
    public virtual async Task<List<TDestination>> ToListAsync<TDestination>(
        ICollection<ColumnFilter> filters,
        Pagination? pagination,
        Expression<Func<TDestination, dynamic>> sortSelector,
        SortDirection sortDirection = SortDirection.Ascending,
        CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyJoinProjection<TDestination>(await GetJoinQueryable(filters, null, null, false, cancellationToken));

        var orderedQuery = ApplyOrderBy(query, sortSelector, sortDirection);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    #endregion LIST

    #region GET

    /// <summary>
    /// Obtém a primeira entidade projetada para <typeparamref name="TDestination"/>
    /// a partir de uma coleção de <see cref="ColumnFilter"/>, utilizando o projetor do repositório (IQueryProjector).
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>O objeto projetado ou <c>null</c>.</returns>
    public virtual async Task<TDestination?> GetAsync<TDestination>(ICollection<ColumnFilter> filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyJoinProjection<TDestination>(await GetJoinQueryable(filters, null, null, true, cancellationToken));

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public override async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyJoinProjection<TDestination>(await GetJoinQueryable(predicate, null, null, true, cancellationToken));

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém a primeira entidade encontrada a partir de uma coleção de <see cref="ColumnFilter"/>.
    /// </summary>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="readonly">Se <c>true</c>, aplica <c>AsNoTracking</c> na consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>A entidade encontrada ou <c>null</c>.</returns>
    public virtual async Task<TEntity?> GetAsync(ICollection<ColumnFilter> filters, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, @readonly, cancellationToken);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

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
    public virtual async Task<TEntity?> GetAsync<TProperty>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TProperty>> includeNavigation, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, @readonly, cancellationToken);

        return await query.Include(includeNavigation).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém a primeira entidade projetada para <typeparamref name="TDestination"/>
    /// a partir de uma coleção de <see cref="ColumnFilter"/>, com ordenação explícita, utilizando o projetor do repositório (IQueryProjector).
    /// </summary>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="sortSelector">Expressão para selecionar a coluna de ordenação.</param>
    /// <param name="sortDirection">Direção da ordenação.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>O objeto projetado ou <c>null</c>.</returns>
    public virtual async Task<TDestination?> GetAsync<TDestination>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
    {
        var orderedQuery = await GetJoinQueryable(filters, sortSelector, sortDirection, true, cancellationToken);

        return await ApplyJoinProjection<TDestination>(orderedQuery)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public override async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var orderedQuery = await GetJoinQueryable(predicate, sortSelector, sortDirection, true, cancellationToken);

        return await ApplyJoinProjection<TDestination>(orderedQuery)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém a primeira entidade encontrada a partir de uma coleção de <see cref="ColumnFilter"/>,
    /// com ordenação explícita.
    /// </summary>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="sortSelector">Expressão para selecionar a coluna de ordenação.</param>
    /// <param name="sortDirection">Direção da ordenação.</param>
    /// <param name="readonly">Se <c>true</c>, aplica <c>AsNoTracking</c> na consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>A entidade encontrada ou <c>null</c>.</returns>
    public virtual async Task<TEntity?> GetAsync(ICollection<ColumnFilter> filters, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = await GetQueryable(filters, @readonly, cancellationToken);

        var orderedQuery = ApplyOrderBy(query, sortSelector, sortDirection);

        return await orderedQuery.FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém a primeira entidade encontrada a partir de uma coleção de <see cref="ColumnFilter"/>,
    /// incluindo uma propriedade de navegação e com ordenação explícita.
    /// </summary>
    /// <typeparam name="TProperty">Tipo da propriedade de navegação.</typeparam>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="includeNavigation">Expressão para incluir a propriedade de navegação.</param>
    /// <param name="sortSelector">Expressão para selecionar a coluna de ordenação.</param>
    /// <param name="sortDirection">Direção da ordenação.</param>
    /// <param name="readonly">Se <c>true</c>, aplica <c>AsNoTracking</c> na consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>A entidade encontrada ou <c>null</c>.</returns>
    public virtual async Task<TEntity?> GetAsync<TProperty>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TProperty>> includeNavigation, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryable(filters, @readonly, cancellationToken)).Include(includeNavigation);

        var orderedQuery = ApplyOrderBy(query, sortSelector, sortDirection);

        return await orderedQuery.FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Obtém exatamente um registro projetado para <typeparamref name="TDestination"/>
    /// a partir de uma coleção de <see cref="ColumnFilter"/>. Retorna <c>null</c> se nenhum ou mais de um for encontrado.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>O objeto projetado, ou <c>null</c> se nenhum ou mais de um for encontrado.</returns>
    public virtual async Task<TDestination?> GetSingleOrDefaultAsync<TDestination>(ICollection<ColumnFilter> filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyJoinProjection<TDestination>(await GetJoinQueryable(filters, null, null, true, cancellationToken));

        try
        {
            return await query.SingleOrDefaultAsync(cancellationToken);
        }
        catch (Exception)
        {
            return default;
        }
    }

    /// <summary>
    /// Obtém exatamente um registro projetado para <typeparamref name="TDestination"/>
    /// a partir de uma coleção de <see cref="ColumnFilter"/>. Lança exceção se mais de um for encontrado.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>O objeto projetado ou <c>null</c>.</returns>
    public virtual async Task<TDestination?> GetSingleOrThrowsAsync<TDestination>(ICollection<ColumnFilter> filters, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var query = ApplyJoinProjection<TDestination>(await GetJoinQueryable(filters, null, null, true, cancellationToken));

        return await query.SingleOrDefaultAsync(cancellationToken);
    }

    public override async Task<TDestination?> GetByIdAsync<TDestination>(object[] ids, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        var predicate = DbSet.EntityType.GetIdExpression<TEntity>(ids);
        var query = ApplyJoinProjection<TDestination>(await GetJoinQueryable(predicate, null, null, true, cancellationToken));

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    #endregion GET

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma coleção de <see cref="ColumnFilter"/>,
    /// utilizando projeção manual.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="projection">Expressão de projeção manual.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de objetos projetados.</returns>
    public Task<List<TDestination>> ToListAsync<TDestination>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        return ToListAsyncInternal_WithClass(filters, projection, null, cancellationToken);
    }

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma coleção de <see cref="ColumnFilter"/>,
    /// aplicando <see cref="ListCriteria"/> e utilizando projeção manual.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="projection">Expressão de projeção manual.</param>
    /// <param name="criteria">Critérios de listagem (paginação, ordenação e filtros).</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de objetos projetados.</returns>
    public Task<List<TDestination>> ToListAsync<TDestination>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TDestination>> projection, ListCriteria criteria, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        return ToListAsyncInternal_WithClass(filters, projection, criteria, cancellationToken);
    }

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma coleção de <see cref="ColumnFilter"/>,
    /// com paginação e ordenação explícitas, utilizando projeção manual.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="projection">Expressão de projeção manual.</param>
    /// <param name="pagination">Configuração de paginação.</param>
    /// <param name="sortSelector">Expressão para selecionar a coluna de ordenação.</param>
    /// <param name="sortDirection">Direção da ordenação.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Lista de objetos projetados.</returns>
    public Task<List<TDestination>> ToListAsync<TDestination>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TDestination>> projection, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        return ToListAsyncInternal_WithClass(filters, projection, pagination, sortSelector, sortDirection, cancellationToken);
    }

    /// <summary>
    /// Obtém a primeira entidade projetada para <typeparamref name="TDestination"/>
    /// a partir de uma coleção de <see cref="ColumnFilter"/>, utilizando projeção manual.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="filters">Coleção de filtros dinâmicos a serem aplicados na consulta.</param>
    /// <param name="projection">Expressão de projeção manual.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>O objeto projetado ou <c>null</c>.</returns>
    public Task<TDestination?> GetAsync<TDestination>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
        where TDestination : class
    {
        return GetAsyncInternal_WithClass(filters, projection, cancellationToken);
    }

    // Internal helpers to implement projection-based methods using the base entity query
    private async Task<List<TDestination>> ToListAsyncInternal_WithClass<TDestination>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TDestination>> projection, ListCriteria? criteria, CancellationToken cancellationToken)
        where TDestination : class
    {
        var query = (await GetQueryable(filters, true, cancellationToken)).Select(projection);

        if (criteria is not null)
            query = query.ApplyCriteria(criteria);

        return await query.ToListAsync(cancellationToken);
    }

    private async Task<List<TDestination>> ToListAsyncInternal_WithClass<TDestination>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TDestination>> projection, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection, CancellationToken cancellationToken)
        where TDestination : class
    {
        var query = (await GetQueryable(filters, false, cancellationToken)).Select(projection);

        var orderedQuery = sortDirection == SortDirection.Ascending
            ? query.OrderBy(sortSelector)
            : query.OrderByDescending(sortSelector);

        return await orderedQuery.ApplyPagination(pagination).ToListAsync(cancellationToken);
    }

    private async Task<TDestination?> GetAsyncInternal_WithClass<TDestination>(ICollection<ColumnFilter> filters, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken)
        where TDestination : class
    {
        var query = (await GetQueryable(filters, true, cancellationToken)).Select(projection);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }
}