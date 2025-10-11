using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using static Maxsys.Core.Extensions.JsonExtensions;

namespace Maxsys.Core.Web.Extensions;

public static class IMvcBuilderExtensions
{
    /// <summary>
    /// Atalho para IMvcBuilder.AddJsonOptions(options).<br/>
    /// Configuração para evitar <see cref="JsonException"/> (object cycle), ignorar case e não escrever properties nulas
    /// </summary>
    public static IMvcBuilder ConfigureJsonOptions(this IMvcBuilder builder)
    {
        JSON_DEFAULT_OPTIONS.Converters.Add(new JsonStringEnumConverter());

        builder.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JSON_DEFAULT_OPTIONS.DefaultIgnoreCondition;
            options.JsonSerializerOptions.Encoder = JSON_DEFAULT_OPTIONS.Encoder;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = JSON_DEFAULT_OPTIONS.PropertyNameCaseInsensitive;
            options.JsonSerializerOptions.PropertyNamingPolicy = JSON_DEFAULT_OPTIONS.PropertyNamingPolicy;
            options.JsonSerializerOptions.ReferenceHandler = JSON_DEFAULT_OPTIONS.ReferenceHandler;

            // Exibir Enum literal no swagger: https://stackoverflow.com/a/65318486
            foreach (var converter in JSON_DEFAULT_OPTIONS.Converters)
            {
                options.JsonSerializerOptions.Converters.Add(converter);
            }
        });

        return builder;
    }
}