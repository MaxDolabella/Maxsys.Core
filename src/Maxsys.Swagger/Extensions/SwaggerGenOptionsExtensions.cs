using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Maxsys.Swagger.Extensions;

public static class MaxsysSwaggerGenOptionsExtensions
{
    /// <summary>
    /// Obtém o assembly ao qual pertence o tipo <typeparamref name="TEntry"/>
    /// e inclui a documentação XML caso encontre o arquivo
    /// (usa convention padrão: [baseDir]/[assemblyName].xml).
    /// </summary>
    public static void IncludeXmlComments<TEntry>(this SwaggerGenOptions options)
    {
        var xmlFile = $"{typeof(TEntry).Assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        if (File.Exists(xmlPath))
            options.IncludeXmlComments(xmlPath);
    }
}

