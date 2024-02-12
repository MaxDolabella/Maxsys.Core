using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using static Maxsys.Core.Extensions.JsonExtensions;

namespace Maxsys.Core.Web.Extensions;

public static class IMvcBuilderExtensions
{
    /// <summary>
    /// Atalho para IMvcBuilder.AddJsonOptions(options).<br/>
    /// Configuração para evitar <see cref="JsonException"/> (object cycle), ignorar case e não escrever properties nulas.
    /// </summary>
    public static IMvcBuilder ConfigureJsonOptions(this IMvcBuilder builder)
    {
        builder.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JSON_DEFAULT_OPTIONS.DefaultIgnoreCondition;
            options.JsonSerializerOptions.ReferenceHandler = JSON_DEFAULT_OPTIONS.ReferenceHandler;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = JSON_DEFAULT_OPTIONS.PropertyNameCaseInsensitive;

            // Exibir Enum literal no swagger: https://stackoverflow.com/a/65318486
            //options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        return builder;
    }
}