namespace Maxsys.Core.Interfaces.Mapping;

/// <summary>
/// Abstração para projeção de <see cref="IQueryable"/> em <see cref="IQueryable{T}"/> de destino
/// (composição de <i>expression tree</i> traduzível pelo provedor LINQ — ex.: EF Core → SQL).<br/>
/// Permite que repositórios (ex.: <c>RepositoryBase</c>) não dependam de um mapeador específico.
/// </summary>
/// <remarks>
/// A implementação padrão baseada em AutoMapper (<c>ProjectTo</c>) está no pacote
/// <c>Maxsys.Mapping.AutoMapper</c> (registro via <c>AddMaxsysAutoMapper</c>).
/// A projeção NÃO materializa a query: apenas reescreve o <c>Select</c>.
/// </remarks>
public interface IQueryProjector
{
    /// <summary>
    /// Projeta a query de origem em uma query de <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino da projeção.</typeparam>
    /// <param name="source">Query de origem.</param>
    /// <returns>Query projetada, ainda não materializada.</returns>
    IQueryable<TDestination> Project<TDestination>(IQueryable source);
}
