namespace Maxsys.Core;

/// <summary>
/// Marca uma propriedade como participante da busca global (Search).
/// <para/>
/// Para propriedades <see langword="string"/> diretas, basta decorar sem parâmetros.
/// Para propriedades aninhadas (objetos complexos), informe o sub-path.
/// </summary>
/// <example>
/// <code>
/// // String direta
/// [Searchable] public string Name { get; set; }
///
/// // Navegação aninhada: gera full path "Location.Country.Name"
/// [Searchable("Country.Name")]
/// [Searchable("City")]
/// public LocationDTO Location { get; set; }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class SearchableAttribute : Attribute
{
    /// <summary>
    /// Sub-path para propriedades aninhadas. <see langword="null"/> para propriedades string diretas.
    /// </summary>
    public string? Path { get; }

    public SearchableAttribute() { }
    public SearchableAttribute(string path) => Path = path;
}
