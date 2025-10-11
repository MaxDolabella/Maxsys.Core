using System.IO;
using System.Text;

namespace Maxsys.Core.Utils;

public sealed class StringWriterWithEncoding : StringWriter
{
    private readonly Encoding _encoding;

    public StringWriterWithEncoding(Encoding encoding)
    {
        _encoding = encoding;
    }

    public override Encoding Encoding => _encoding;
}