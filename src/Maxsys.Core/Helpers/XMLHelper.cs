using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Maxsys.Core.Helpers;

public static class XMLHelper
{
    private static readonly XmlSerializerNamespaces _emptyNamespaces = new([XmlQualifiedName.Empty]);

    private static readonly XmlWriterSettings _settings = new()
    {
        Indent = false,
        OmitXmlDeclaration = true
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
        using (var writer = XmlWriter.Create(stream, _settings))
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
}