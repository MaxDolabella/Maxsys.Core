namespace Maxsys.Core.Interfaces.Mapping;

/// <summary>
/// Abstração para mapeamento objeto → objeto (instâncias em memória).<br/>
/// Permite que serviços (ex.: <c>ModelServiceBase</c>) não dependam de um mapeador
/// específico (AutoMapper, Mapster, mapeamento manual...).
/// </summary>
/// <remarks>
/// A implementação padrão baseada em AutoMapper está no pacote <c>Maxsys.Mapping.AutoMapper</c>
/// (registro via <c>AddMaxsysAutoMapper</c>).
/// </remarks>
public interface IObjectMapper
{
    /// <summary>
    /// Mapeia <paramref name="source"/> para uma nova instância de <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TDestination">Tipo de destino do mapeamento.</typeparam>
    /// <param name="source">Objeto de origem.</param>
    TDestination Map<TDestination>(object source);

    /// <summary>
    /// Mapeia <paramref name="source"/> para uma nova instância de <typeparamref name="TDestination"/>
    /// e executa <paramref name="afterMap"/> sobre o resultado (pós-processamento no momento do map).
    /// </summary>
    /// <remarks>
    /// O <paramref name="afterMap"/> roda <b>após</b> o pipeline do mapeador
    /// (no AutoMapper, depois dos <c>AfterMap</c>s configurados em Profile).
    /// Implementação default: <c>Map</c> seguido de <paramref name="afterMap"/> —
    /// adapters customizados não precisam implementar.
    /// </remarks>
    /// <typeparam name="TDestination">Tipo de destino do mapeamento.</typeparam>
    /// <param name="source">Objeto de origem.</param>
    /// <param name="afterMap">Ação executada sobre o objeto mapeado.</param>
    TDestination Map<TDestination>(object source, Action<TDestination> afterMap)
    {
        var destination = Map<TDestination>(source);

        afterMap(destination);

        return destination;
    }

    /// <summary>
    /// Mapeia <paramref name="source"/> sobre a instância existente <paramref name="destination"/>
    /// (mapeamento <i>in-place</i>, típico de operações de update).
    /// </summary>
    /// <typeparam name="TSource">Tipo de origem.</typeparam>
    /// <typeparam name="TDestination">Tipo de destino.</typeparam>
    /// <param name="source">Objeto de origem.</param>
    /// <param name="destination">Instância de destino que receberá os valores.</param>
    /// <returns>A própria instância <paramref name="destination"/>, atualizada.</returns>
    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
}
