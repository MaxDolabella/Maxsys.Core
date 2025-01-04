using Maxsys.Core.Web.Swagger.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Maxsys.Core.Web.Swagger.Extensions;

public static class SwaggerGenOptionsExtensions
{
    /// <summary>
    /// Obtém o assembly ao qual pertence o tipo <typeparamref name="TEntry"/> 
    /// e inclui a documentação XML caso encontre o arquivo (usa convention padrão: [baseDir]/[assemblyName].xml)
    /// </summary>
    /// <typeparam name="TEntry"></typeparam>
    /// <param name="options"></param>
    public static void IncludeXmlComments<TEntry>(this SwaggerGenOptions options)
    {
        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{typeof(TEntry).Assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
            options.IncludeXmlComments(xmlPath);
    }
}