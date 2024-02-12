using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Maxsys.Core;

/// <summary>
/// Tipos de resultados (validação e operação)
/// <list type="bullet">
/// <item>
///     <term>0. <see cref="Error"/></term>
///     <description>Erro.</description>
/// </item>
/// <item>
///     <term>1. <see cref="Warning"/></term>
///     <description>Aviso.</description>
/// </item>
/// <item>
///     <term>2. <see cref="Info"/></term>
///     <description>Informação.</description>
/// </item>
/// <item>
///     <term>3. <see cref="Success"/></term>
///     <description>Sucesso.</description>
/// </item>
/// </list>
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ResultTypes : byte
{
    /// <summary>
    /// Erro
    /// </summary>
    [Description("Erro")]
    Error = 0,

    /// <summary>
    /// Aviso
    /// </summary>
    [Description("Aviso")]
    Warning = 1,

    /// <summary>
    /// Informação
    /// </summary>
    [Description("Informação")]
    Info = 2,

    /// <summary>
    /// Sucesso
    /// </summary>
    [Description("Sucesso")]
    Success = 3
}