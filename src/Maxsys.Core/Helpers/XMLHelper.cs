using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Maxsys.Core.Utils;

namespace Maxsys.Core.Helpers;

public static class XMLHelper
{
    private static readonly XmlSerializerNamespaces _emptyNamespaces = new([XmlQualifiedName.Empty]);

    /// <summary>
    /// <code>
    /// {
    ///     Indent = false,
    ///     OmitXmlDeclaration = true,
    ///     IndentChars = "\t",
    /// }
    /// </code>
    /// </summary>
    public static readonly XmlWriterSettings DEFAULT_XML_SETTINGS = new()
    {
        Indent = false,
        OmitXmlDeclaration = true,
        IndentChars = "\t",
    };

    public static T? Deserialize<T>(string? xml, string? defaultNamespace = default)
    {
        var xmlSerializer = new XmlSerializer(typeof(T), defaultNamespace);
        using (var reader = new StringReader(xml ?? string.Empty))
        {
            if (defaultNamespace is null)
            {
                using var textReader = new IgnoreNamespaceXmlTextReader(reader);
                return (T?)xmlSerializer.Deserialize(textReader);
            }
            else
            {
                return (T?)xmlSerializer.Deserialize(reader);
            }
        }
    }

    public static string Serialize<T>(T item)
    {
        string xml = string.Empty;
        var xmlSerializer = new XmlSerializer(typeof(T), default(string?));
        using (var stream = new StringWriter())
        using (var writer = XmlWriter.Create(stream, DEFAULT_XML_SETTINGS))
        {
            xmlSerializer.Serialize(writer, item, _emptyNamespaces);
            xml = stream.ToString();
        }

        return xml;
    }

    public static XElement? Read(string? xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
        {
            return null;
        }

        using (var stream = new MemoryStream(Encoding.Default.GetBytes(xml)))
        {
            return XElement.Load(stream);
        }
    }

    public static void ReplaceElement(XElement other, XElement me, XName xName)
    {
        if (other.Element(xName) != me.Element(xName))
        {
            if (other.Element(xName) is null)
            {
                other.Add(me.Element(xName));
            }
            else
            {
                other.Element(xName)!.ReplaceWith(me.Element(xName));
            }
        }
    }

    public static OperationResult<string?> ToXmlString<TXml>(TXml root, Encoding? encoding = null, XmlWriterSettings? settings = null)
        where TXml : class
    {
        var result = new OperationResult<string?>();

        var serializer = new XmlSerializer(typeof(TXml));
        var ns = new XmlSerializerNamespaces();
        ns.Add(string.Empty, string.Empty);

        using (var stringWriter = encoding is null
            ? new StringWriter()
            : new StringWriterWithEncoding(encoding))
        {
            using (var writer = XmlWriter.Create(stringWriter, settings ?? DEFAULT_XML_SETTINGS))
            {
                try
                {
                    serializer.Serialize(writer, root, ns);
                    result.Data = stringWriter.ToString();
                }
                catch (Exception ex)
                {
                    result.AddException(ex, GenericMessages.INVALID_XML);
                }
            }
        }

        return result;
    }

    public static OperationResult ValidateSchema<TXml>(TXml root, string schemaResourceName, XmlSerializerNamespaces? namespaces = null, Encoding? encoding = null)
        where TXml : class
    {
        var schemaResult = ResourcesHelper.GetXmlSchema<TXml>(schemaResourceName);

        return schemaResult.IsValid
            ? ValidateSchema(root, schema: schemaResult.Data, namespaces: namespaces, encoding: encoding)
            : schemaResult;
    }

    public static OperationResult ValidateSchema<TXml>(TXml root, XmlSchema schema, XmlSerializerNamespaces? namespaces = null, Encoding? encoding = null)
        where TXml : class
    {
        var serializer = new XmlSerializer(typeof(TXml));

        if (namespaces is null)
        {
            namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
        }

        #region XmlSchema e XmlReaderSettings

        var settings = new XmlReaderSettings()
        {
            ValidationType = ValidationType.Schema
        };
        settings.Schemas.Add(schema);

        #endregion XmlSchema e XmlReaderSettings

        var result = new OperationResult();

        using (var stream = new MemoryStream())
        {
            using (var writer = new StreamWriter(stream, encoding ?? Encoding.UTF8))
            {
                serializer.Serialize(writer, root, namespaces);

                stream.Seek(0, SeekOrigin.Begin);
                using (var xmlReader = XmlReader.Create(stream, settings))
                {
                    var document = new XmlDocument();

                    try
                    {
                        document.Load(xmlReader);
                    }
                    catch (Exception ex)
                    {
                        result.AddException(ex, GenericMessages.INVALID_XML);
                    }
                }

                stream.Seek(0, SeekOrigin.Begin);
            }
        }

        return result;
    }
}