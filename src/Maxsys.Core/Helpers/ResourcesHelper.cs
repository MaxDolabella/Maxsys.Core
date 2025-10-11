using System.IO;
using System.Xml.Schema;

namespace Maxsys.Core.Utils;

/// <summary>
/// Reference documentation <see href="https://code-maze.com/dotnet-embedded-resources/">here</see>.
/// </summary>
public static class ResourcesHelper
{
    /// <typeparam name="TAssemblyReference">qualquer classe contida no assembly onde se deseja buscar o recurso.</typeparam>
    public static Stream? GetEmbeddedResource<TAssemblyReference>(string resourceName)
    {
        return typeof(TAssemblyReference).Assembly.GetManifestResourceStream(resourceName);
    }

    public static string[] ListResourcesInAssembly<TAssemblyReference>()
    {
        var assembly = typeof(TAssemblyReference).Assembly;

        return assembly?.GetManifestResourceNames() ?? [];
    }

    /// <summary>
    /// Obtém um <see cref="XmlSchema"/> a partir de um recurso embutido.
    /// </summary>
    /// <typeparam name="TAssemblyReference"></typeparam>
    /// <param name="resourceName"></param>
    /// <returns></returns>
    public static OperationResult<XmlSchema?> GetXmlSchema<TAssemblyReference>(string resourceName)
    {
        var result = new OperationResult<XmlSchema?>();

        try
        {
            using (var xsdStream = GetEmbeddedResource<TAssemblyReference>(resourceName))
            {
                if (xsdStream is null)
                {
                    throw new Exception($"Resource '{resourceName}' not found.");
                }

                result.Data = XmlSchema.Read(xsdStream, null)
                    ?? throw new DomainException($"Error reading Schema '{resourceName}' from resources.");
            }
        }
        catch (Exception ex)
        {
            result.AddException(ex, GenericMessages.SCHEMA_READING_ERROR);
        }

        return result;
    }
}