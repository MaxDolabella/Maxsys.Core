using Maxsys.Swagger.Helpers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Maxsys.Core.Web.Swagger.Filters;

/// <summary>
/// Quando o parâmetro for enum, adiciona à description detalhes dos literals.
/// Cada item do enum, terá na description, o seguinte formato:
/// <para/>
/// {valor} - {literal} ({atributo_description quando tiver})
/// <para/>
/// <example>
/// Exemplo:
/// <code>
/// public enum SampleEnum : byte
/// {
///     [Description("Este é o tipo A.")]
///     TipoA = 1,
///
///     [Description("Este é o tipo B.")]
///     TipoB,
///
///     // Sem description
///     TipoC = 99
/// }
///
/// /*
///     parameter.Description:
///     [
///         "Valores possíveis:",
///         "1 - TipoA (Este é o tipo A.)",
///         "2 - TipoB (Este é o tipo B.)",
///         "99 - TipoC"
///     ]
///
/// */
/// </code>
/// </example>
/// </summary>
public class EnumParameterFilter : IParameterFilter
{
    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        if (context.ParameterInfo.ParameterType.IsEnum)
        {
            var referenceId = parameter.Schema?.Reference?.ReferenceV3;
            var contents = new List<string>
            {
                $"[{context.ParameterInfo.ParameterType.Name}]({referenceId})."
            };

            contents.AddRange(Helper.GetEnumDescriptionsList(context.ParameterInfo.ParameterType));

            var description = string.Join("<br/>", contents);

            parameter.Description = description;
        }
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum)
            return;

        var contents = Helper.GetEnumDescriptionsList(context.Type);

        var description = string.Join("<br/>", contents);

        schema.Description = string.IsNullOrWhiteSpace(schema.Description) ? description : $"{schema.Description}<br/>{description}";
    }
}