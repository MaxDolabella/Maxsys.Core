using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using static Maxsys.Core.Extensions.JsonExtensions;

namespace Maxsys.Core.Web.Extensions;

public static class IMvcBuilderExtensions
{
    /// <summary>
    /// Atalho para IMvcBuilder.AddJsonOptions(options).<br/>
    /// Configuração para evitar <see cref="JsonException"/> (object cycle), ignorar case e não escrever properties nulas.
    /// Adicionar <see cref="JsonStringEnumConverter"/> a <see cref="JSON_DEFAULT_OPTIONS"/>.
    /// </summary>
    public static IMvcBuilder ConfigureJsonOptions(this IMvcBuilder builder)
    {
        JSON_DEFAULT_OPTIONS.Converters.Add(new JsonStringEnumConverter());

        builder.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = .DefaultIgnoreCondition;
            options.JsonSerializerOptions.ReferenceHandler = JSON_DEFAULT_OPTIONS.ReferenceHandler;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = JSON_DEFAULT_OPTIONS.PropertyNameCaseInsensitive;
            options.JsonSerializerOptions.PropertyNamingPolicy = JSON_DEFAULT_OPTIONS.PropertyNamingPolicy;

            // Exibir Enum literal no swagger: https://stackoverflow.com/a/65318486
            foreach (var converter in JSON_DEFAULT_OPTIONS.Converters)
            {
                options.JsonSerializerOptions.Converters.Add(converter);
            }
        });

        return builder;
    }
}