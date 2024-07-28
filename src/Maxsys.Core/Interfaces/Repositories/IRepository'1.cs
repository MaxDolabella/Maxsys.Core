using System.Linq.Expressions;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Interfaces.Repositories;

/// <summary>
/// Fornece uma interface um repositório da entidade <typeparamref name="TEntity"/>.<br/>
/// Possui métodos básicos CRUD.
/// <para/>Aviso - "Sempre prefira Composição a Herança": <see href="https://youtu.be/LfiezdBs318?t=890"/>
/// </summary>
/// <typeparam name="TEntity">é a entidade do banco.</typeparam>
public interface IRepository<TEntity> : IRepository where TEntity : class
{
    #region MOD

    /// <summary>
    /// Adds an object of type <typeparamref name="TEntity"/> in the repository asynchronously.
    /// </summary>
    /// <param name="entity">Is the <typeparamref name="TEntity"/> to add.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is added,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds an <see cref="IEnumerable{T}"/> in the repository asynchronously, where T is <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="entities">Is the <see cref="IEnumerable{T}"/> of <typeparamref name="TEntity"/> to add.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is the <see cref="IEnumerable{T}"/> of <typeparamref name="TEntity"/> is added,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete an object of type <typeparamref name="TEntity"/> from the repository asynchronously.
    /// </summary>
    /// <param name="id">Is the key of the <typeparamref name="TEntity"/> to remove.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is deleted,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> DeleteAsync(object id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete an object of type <typeparamref name="TEntity"/> from the repository asynchronously.
    /// </summary>
    /// <param name="compositeKey">Is the composite key of the <typeparamref name="TEntity"/> to remove.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is deleted,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> DeleteAsync(object[] compositeKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update an object of type <typeparamref name="TEntity"/> in the repository asynchronously.
    /// </summary>
    /// <param name="entity">Is the <typeparamref name="TEntity"/> to update.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is updated,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update an object of type <typeparamref name="TEntity"/> in the repository asynchronously.
    /// </summary>
    /// <param name="entities">Is the <see cref="IEnumerable{T}"/> of <typeparamref name="TEntity"/> to update.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns><see langword="true"/> is <typeparamref name="TEntity"/> is updated,
    /// otherwise, <see langword="false"/></returns>
    ValueTask<bool> UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    #endregion MOD

    #region UTIL

    /// <summary>
    /// Asynchronously returns the number of elements in a sequence that satisfy a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns></returns>
    ValueTask<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellation = default);

    /// <summary>
    /// Asynchronously determines whether a sequence contains any elements that satisfy a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellation = default);

    /// <summary>
    /// Asynchronously determines whether a <typeparamref name="TEntity"/> with specified id(s) exists.
    /// </summary>
    /// <param name="ids">verifying id(s).</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    ValueTask<bool> IdExistsAsync(object[] ids, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se houve mudança de estado da entidade no contexto.
    /// </summary>
    /// <param name="entity">é a instância da entidade a ser verificada.</param>
    /// <param name="added">Opcional. Verifica se a entidade possui estado de 'adicionada'.<br/>Padrão é <see langword="true"/>.</param>
    /// <param name="modified">Opcional. Verifica se a entidade possui estado de 'modificada'.<br/>Padrão é <see langword="true"/>.</param>
    /// <param name="deleted">Opcional. Verifica se a entidade possui estado de 'deletada'.<br/>Padrão é <see langword="true"/>.</param>
    /// <returns><see langword="true"/> ou <see langword="false"/> indicando que a entidade foi alterada ou não, conforme os parâmetros.</returns>
    bool HasChanges(TEntity entity, bool added = true, bool modified = true, bool deleted = true);

    #endregion UTIL

    #region LIST

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TEntity"/> a partir de uma expression.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção dos items.</param>
    /// <param name="readonly">
    /// Para alguns ORMs como <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, especifica se a entidade deve ser monitorada.
    /// <br/>
    /// Se <paramref name="readonly"/>=<see langword="false"/>, deve ser monitorada, caso contrário, não.
    /// <para/>Padrão é <see langword="true"/> (somente leitura).
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="List{TEntity}"/> com os registros filtrados.</returns>
    Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>>? predicate = null, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TEntity"/> a partir de uma expression
    /// aplicando-se paginação e ordenação.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção dos items.</param>
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
    Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>>? predicate, ListCriteria criteria, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TEntity"/> a partir de uma expression
    /// aplicando-se paginação e ordenação.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção dos items.</param>
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
    Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>>? predicate, Pagination? pagination, Expression<Func<TEntity, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma expression.
    /// </summary>
    ///
    /// <param name="projection">uma função de projeção para aplicar a cada elemento.</param>
    /// <param name="predicate">é a condição para obtenção dos items.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="List{TDestination}"/> com os registros filtrados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, TDestination>> projection, Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma expression
    /// aplicando-se paginação e ordenação.
    /// </summary>
    ///
    /// <param name="projection">uma função de projeção para aplicar a cada elemento.</param>
    /// <param name="predicate">é a condição para obtenção dos items.</param>
    /// <param name="criteria">contém critérios para obtenção dos items como paginação e lista de ordenações.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="List{TDestination}"/> com os registros filtrados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, TDestination>> projection, Expression<Func<TEntity, bool>>? predicate, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma expression
    /// aplicando-se paginação e ordenação.
    /// </summary>
    ///
    /// <param name="projection">uma função de projeção para aplicar a cada elemento.</param>
    /// <param name="predicate">é a condição para obtenção dos items.</param>
    /// <param name="pagination">contém o índice e a númedo página utilizada na obtenção dos items.</param>
    /// <param name="sortSelector">é a propriedade a ser ordenada.</param>
    /// <param name="sortDirection">
    /// é a direção da ordenação.
    /// <para/>Padrão é <see cref="SortDirection.Ascending"/>.
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="List{TDestination}"/> com os registros filtrados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, TDestination>> projection, Expression<Func<TEntity, bool>>? predicate, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma expression.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção dos items.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="List{TDestination}"/> com os registros filtrados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma expression
    /// aplicando-se paginação e ordenação.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção dos items.</param>
    /// <param name="criteria">contém critérios para obtenção dos items como paginação e lista de ordenações.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="List{TDestination}"/> com os registros filtrados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>>? predicate, ListCriteria criteria, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém uma lista de <typeparamref name="TDestination"/> a partir de uma expression
    /// aplicando-se paginação e ordenação.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção dos items.</param>
    /// <param name="pagination">contém o índice e a númedo página utilizada na obtenção dos items.</param>
    /// <param name="sortSelector">é a propriedade a ser ordenada.</param>
    /// <param name="sortDirection">
    /// é a direção da ordenação.
    /// <para/>Padrão é <see cref="SortDirection.Ascending"/>.
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>uma <see cref="List{TDestination}"/> com os registros filtrados.</returns>
    Task<List<TDestination>> ToListAsync<TDestination>(Expression<Func<TEntity, bool>>? predicate, Pagination? pagination, Expression<Func<TDestination, dynamic>> sortSelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);

    #endregion LIST

    #region GET

    /// <summary>
    /// Obtém um item <typeparamref name="TEntity"/> a partir de seu id.
    /// <br/>
    /// Caso nenhum item corresponda aos critérios, <see langword="null"/> será retornado.
    /// </summary>
    /// <param name="id">é id para obtenção do item.</param>
    /// <param name="readonly">
    /// Para alguns ORMs como <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, especifica se a entidade deve ser monitorada.
    /// <br/>
    /// Se <paramref name="readonly"/>=<see langword="false"/>, deve ser monitorada, caso contrário, não.
    /// <para/>Padrão é <see langword="true"/> (somente leitura).
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>um objeto <typeparamref name="TEntity"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TEntity?> GetByIdAsync(object id, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um item <typeparamref name="TEntity"/> a partir de seu(s) id(s). Pode ser usado para obtenção de objeto com chave múltipla.
    /// <br/>
    /// Caso nenhum item corresponda aos critérios, <see langword="null"/> será retornado.
    /// </summary>
    /// <param name="ids">são os ids para obtenção do item.</param>
    /// <param name="readonly">
    /// Para alguns ORMs como <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, especifica se a entidade deve ser monitorada.
    /// <br/>
    /// Se <paramref name="readonly"/>=<see langword="false"/>, deve ser monitorada, caso contrário, não.
    /// <para/>Padrão é <see langword="true"/> (somente leitura).
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>um objeto <typeparamref name="TEntity"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TEntity?> GetByIdAsync(object[] ids, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um item <typeparamref name="TDestination"/> a partir de seu id.
    /// <br/>
    /// Caso nenhum item corresponda aos critérios, <see langword="null"/> será retornado.
    /// </summary>
    /// <param name="id">é id para obtenção do item.</param>
    /// <param name="projection">uma função de projeção para aplicar a cada elemento.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>um objeto <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TDestination?> GetByIdAsync<TDestination>(object id, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um item <typeparamref name="TDestination"/> a partir de seu(s) id(s). Pode ser usado para obtenção de objeto com chave múltipla.
    /// <br/>
    /// Caso nenhum item corresponda aos critérios, <see langword="null"/> será retornado.
    /// </summary>
    /// <param name="ids">são os ids para obtenção do item.</param>
    /// <param name="projection">uma função de projeção para aplicar a cada elemento.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>um objeto <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TDestination?> GetByIdAsync<TDestination>(object[] ids, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default);

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
    /// Obtém o primeiro item <typeparamref name="TEntity"/> a partir de uma expression.
    /// <br/>
    /// Caso nenhum item corresponda aos critérios, <see langword="null"/> será retornado.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção do item.</param>
    /// <param name="readonly">
    /// Para alguns ORMs como <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, especifica se a entidade deve ser monitorada.
    /// <br/>
    /// Se <paramref name="readonly"/>=<see langword="false"/>, deve ser monitorada, caso contrário, não.
    /// <para/>Padrão é <see langword="true"/> (somente leitura).
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o primeiro item <typeparamref name="TEntity"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TEntity"/> a partir de uma expression
    /// e ordenação definida por <paramref name="sortKeySelector"/> e <paramref name="sortDirection"/>.
    /// <br/>
    /// Caso nenhum item corresponda aos critérios, <see langword="null"/> será retornado.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção do item.</param>
    /// <param name="sortKeySelector">é a propriedade a ser ordenada.</param>
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
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TEntity"/> a partir de uma expression
    /// incluindo a navegação <typeparamref name="TProperty"/>
    /// e ordenação definida por <paramref name="sortKeySelector"/> e <paramref name="sortDirection"/>.
    /// <br/>
    /// Caso nenhum item corresponda aos critérios, <see langword="null"/> será retornado.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção do item.</param>
    /// <param name="includeNavigation">é a propriedade de navegação a ser incluída.</param>
    /// <param name="sortKeySelector">é a propriedade a ser ordenada.</param>
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
    Task<TEntity?> GetWithIncludeAsync<TProperty>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProperty>> includeNavigation, Expression<Func<TEntity, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascending, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TEntity"/> a partir de uma expression
    /// incluindo a navegação <typeparamref name="TProperty"/>.
    /// <br/>
    /// Caso nenhum item corresponda aos critérios, <see langword="null"/> será retornado.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção do item.</param>
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
    Task<TEntity?> GetWithIncludeAsync<TProperty>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProperty>> includeNavigation, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o único item <typeparamref name="TEntity"/> correspondente a partir de uma expression.
    /// <br/>
    /// Caso nenhum ou mais de um item corresponda aos critérios, <see langword="null"/> será retornado.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção do item.</param>
    /// <param name="readonly">
    /// Para alguns ORMs como <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, especifica se a entidade deve ser monitorada.
    /// <br/>
    /// Se <paramref name="readonly"/>=<see langword="false"/>, deve ser monitorada, caso contrário, não.
    /// <para/>Padrão é <see langword="true"/> (somente leitura).
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o único item <typeparamref name="TEntity"/> ou <see langword="null"/> caso nenhum ou mais de um item corresponda aos critérios.</returns>
    /// <remarks>Esse método não lança exception.</remarks>
    Task<TEntity?> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o único item <typeparamref name="TEntity"/> correspondente a partir de uma expression.
    /// <br/>
    /// Caso nenhum item corresponda aos critérios, <see langword="null"/> será retornado.
    /// <br/>
    /// Caso mais de um item corresponda aos critérios, uma <see cref="InvalidOperationException"/> será lançada.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção do item.</param>
    /// <param name="readonly">
    /// Para alguns ORMs como <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, especifica se a entidade deve ser monitorada.
    /// <br/>
    /// Se <paramref name="readonly"/>=<see langword="false"/>, deve ser monitorada, caso contrário, não.
    /// <para/>Padrão é <see langword="true"/> (somente leitura).
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o único item <typeparamref name="TEntity"/> ou <see langword="null"/> caso nenhum ou mais de um item corresponda aos critérios.</returns>
    /// <exception cref="InvalidOperationException"/>
    Task<TEntity?> GetSingleOrThrowsAsync(Expression<Func<TEntity, bool>> predicate, bool @readonly = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TDestination"/> a partir de uma expression.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção do item.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o primeiro item <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TDestination"/> a partir de uma expression.
    /// <br/>
    /// Caso nenhum item corresponda aos critérios, <see langword="null"/> será retornado.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção do item.</param>
    /// <param name="projection">uma função de projeção para aplicar a cada elemento.</param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o primeiro item <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TDestination"/> a partir de uma expression
    /// e ordenação definida por <paramref name="sortKeySelector"/> e <paramref name="sortDirection"/>.
    /// <br/>
    /// Caso nenhum item corresponda aos critérios, <see langword="null"/> será retornado.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção do item.</param>
    /// <param name="projection">uma função de projeção para aplicar a cada elemento.</param>
    /// <param name="sortKeySelector">é a propriedade a ser ordenada.</param>
    /// <param name="sortDirection">
    /// é a direção da ordenação.
    /// <para/>Padrão é <see cref="SortDirection.Ascending"/>.
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o primeiro item <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, Expression<Func<TEntity, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o primeiro item <typeparamref name="TDestination"/> a partir de uma expression
    /// e ordenação definida por <paramref name="sortKeySelector"/> e <paramref name="sortDirection"/>.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção do item.</param>
    /// <param name="sortKeySelector">é a propriedade a ser ordenada.</param>
    /// <param name="sortDirection">
    /// é a direção da ordenação.
    /// <para/>Padrão é <see cref="SortDirection.Ascending"/>.
    /// </param>
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o primeiro item <typeparamref name="TDestination"/> ou <see langword="null"/> caso nenhum item corresponda aos critérios.</returns>
    Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, dynamic>> sortKeySelector, SortDirection sortDirection = SortDirection.Ascending, CancellationToken cancellationToken = default) where TDestination : class;

    /// <summary>
    /// Obtém o único item <typeparamref name="TDestination"/> correspondente a partir de uma expression.
    /// <br/>
    /// Caso nenhum ou mais de um item corresponda aos critérios, <see langword="null"/> será retornado.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção do item.</param>
    /// Para alguns ORMs como <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, especifica se a entidade deve ser monitorada.
    /// <br/>
    /// <para/>Padrão é <see langword="true"/> (somente leitura).
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o único item <typeparamref name="TEntity"/> ou <see langword="null"/> caso nenhum ou mais de um item corresponda aos critérios.</returns>
    /// <remarks>Esse método não lança exception.</remarks>
    Task<TDestination?> GetSingleOrDefaultAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o único item <typeparamref name="TDestination"/> correspondente a partir de uma expression.
    /// <br/>
    /// Caso nenhum item corresponda aos critérios, <see langword="null"/> será retornado.
    /// <br/>
    /// Caso mais de um item corresponda aos critérios, uma <see cref="InvalidOperationException"/> será lançada.
    /// </summary>
    ///
    /// <param name="predicate">é a condição para obtenção do item.</param>
    /// Para alguns ORMs como <see href="https://docs.microsoft.com/pt-br/ef/core/querying/tracking">
    /// Entity Framework</see>, especifica se a entidade deve ser monitorada.
    /// <br/>
    /// <para/>Padrão é <see langword="true"/> (somente leitura).
    /// <param name="cancellationToken">Um <see cref="CancellationToken"/> para notificar que uma Task deve ser cancelada.</param>
    /// <returns>o único item <typeparamref name="TEntity"/> ou <see langword="null"/> caso nenhum ou mais de um item corresponda aos critérios.</returns>
    /// <exception cref="InvalidOperationException"/>
    Task<TDestination?> GetSingleOrThrowsAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    #endregion GET
}