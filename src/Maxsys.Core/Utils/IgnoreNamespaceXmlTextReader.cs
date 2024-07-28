using System.IO;

namespace System.Xml;

/// <summary>
/// <para>C# Ignoring Namespaces in XML when Deserializing</para>
/// <see href="https://briancaos.wordpress.com/2022/07/13/c-ignoring-namespaces-in-xml-when-deserializing/"/>
/// </summary>
public class IgnoreNamespaceXmlTextReader : XmlTextReader
{
    public IgnoreNamespaceXmlTextReader(TextReader reader) : base(reader)
    { }

    public override string NamespaceURI => string.Empty;
}